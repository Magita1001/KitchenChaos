using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSo kitchenObjectSo;
    }

    [SerializeField] private List<KitchenObjectSo> validKitchenObjectSOList;
    private List<KitchenObjectSo> kitchenObjectSoList;
    private void Awake()
    {
        kitchenObjectSoList = new List<KitchenObjectSo>();
    }
    public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSo))
        {
            //(试图加入盘子中的)不是有效成员
            return false;
        }
        if (kitchenObjectSoList.Contains(kitchenObjectSo))
        {
            //说明盘子里已经包含这个对象
            return false;
        }
        else
        {
            kitchenObjectSoList.Add(kitchenObjectSo);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSo = kitchenObjectSo
            });
            return true;
        }
        
    }
    public List<KitchenObjectSo> GetKitchenObjectSOList()
    {
        return kitchenObjectSoList;
    }
}
