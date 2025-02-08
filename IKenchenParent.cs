using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKenchenParent
{
    public Transform GetKitchenObjectFollowTransForm();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HasKitchenobject();

}
