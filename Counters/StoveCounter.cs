using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgessChangendEventArgs> OnProgressChanged; 
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public  enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State state;

    //private void Start()
    //{
    //    StartCoroutine(HanderFryTimer());
    //}
    //private IEnumerator HanderFryTimer()
    //{
    //    yield return new WaitForSeconds(1f);
    //}
    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenobject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });                    
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestorySelf();
                        KitchenObject.SpawnKitchenobject(fryingRecipeSO.output, this);                                                
                        state = State.Fried;
                        burningTimer = 0;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                    {
                        progressNormalized = (float)burningTimer / burningRecipeSO.BurningTimeMax
                    });
                    if (burningTimer >= burningRecipeSO.BurningTimeMax)
                    {
                        GetKitchenObject().DestorySelf();
                        KitchenObject.SpawnKitchenobject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
                default:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
         if (!HasKitchenobject())
        {
            //如果玩家手上有厨房对象，并且可以切片，则可以放置到自身
            if (player.HasKitchenobject())
            {
                if (HasRecipWithInput(player.GetKitchenObject().GetKitchenObjectSo())) 
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());

                    state = State.Frying;
                    fryingTimer = 0;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }                
            }
        }
        else
        {
            //玩家手中有厨房对象的话
            if (player.HasKitchenobject())
            {
                //判断是否是盘子对象
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo())) 
                    {
                        GetKitchenObject().DestorySelf();
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }                    
                }
            }
            else
            {                
                //如果没有厨房对象，则给玩家
                this.GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        return cuttingRecipeSO != null;
    }

    /// <summary>
    /// 返回切片后的对象
    /// </summary>
    /// <param name="inputKitchenObjectSo"></param>
    /// <returns></returns>
    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }        
    }    

    /// <summary>
    /// 判断是否有对应配方，有就返回切割配方
    /// </summary>
    /// <param name="InputkitchenObjectSo"></param>
    /// <returns></returns>
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSo InputkitchenObjectSo)
    {
        foreach (FryingRecipeSO FryingRecipeSO in fryingRecipeSOArray)
        {
            if (FryingRecipeSO.input == InputkitchenObjectSo) 
            {
                return FryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSo InputkitchenObjectSo)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == InputkitchenObjectSo) 
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried; 
    }
}
