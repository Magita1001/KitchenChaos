using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgessChangendEventArgs> OnProgressChanged;    
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;

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
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
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
                    }                    
                }
            }
            else
            {                
                //如果没有厨房对象，则给玩家
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        //如果有东西在这个台子上，并且可以切割，切掉
        if (HasKitchenobject() && HasRecipWithInput(GetKitchenObject().GetKitchenObjectSo())) 
        {
            //判断是否达到切割最大值
            cuttingProgress++;
            OnCut?.Invoke(this,EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgessChangendEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            //达到了则替换成切割后的对象
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSo outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());
                GetKitchenObject().DestorySelf();
                KitchenObject.SpawnKitchenobject(outputKitchenObjectSo, this);
            }   
            
        }
    }
    /// <summary>
    /// 判断能否切割
    /// </summary>
    /// <param name="kitchenObjectSo"></param>
    /// <returns></returns>
    private bool HasRecipWithInput(KitchenObjectSo kitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSo);
        return cuttingRecipeSO != null;
    }
    /// <summary>
    /// 返回切片后的对象
    /// </summary>
    /// <param name="inputKitchenObjectSo"></param>
    /// <returns></returns>
    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
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
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSo InputkitchenObjectSo)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == InputkitchenObjectSo) 
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
