using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKenchenParent
{
    public static event EventHandler OnAnyObjecePlacedHere;
    [SerializeField] private Transform CounterTopPoint;    
    private KitchenObject kitchenObject;

    public static void ResetStaticData()
    {
        OnAnyObjecePlacedHere = null;
    }

    public virtual void Interact(Player player)
    {

    }   
    public virtual void InteractAlternate(Player player)
    {

    }


    public Transform GetKitchenObjectFollowTransForm()
    {
        return CounterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnAnyObjecePlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenobject()
    {
        return kitchenObject != null;
    }
}
