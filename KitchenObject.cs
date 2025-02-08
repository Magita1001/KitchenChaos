using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    //预制体上挂载的厨房脚本，返回关于自身的一些信息
    [SerializeField] private KitchenObjectSo kitchenObjectSo;

    private IKenchenParent kitchenObjectParent;

    /// <summary>
    /// 返回当前厨房物体对象的信息
    /// </summary>
    /// <returns></returns>
    public KitchenObjectSo GetKitchenObjectSo()
    {
        return kitchenObjectSo;
    }

    /// <summary>
    /// 设置新的目标父类，位置
    /// </summary>
    /// <param name="kitchenObjectParent"></param>
    public void SetKitchenObjectParent(IKenchenParent kitchenObjectParent)
    {
        //把当前父类置空，随后更新当前的父类
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenobject())
        {
            Debug.LogError("IKenchenParent已经有了一个kitchenObject");
        }

        //把灶台内的厨房对象数据改为this
        kitchenObjectParent.SetKitchenObject(this);
        //修改自身位置到新的目标位置下
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransForm();
        transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 返回自身所在的目标的信息
    /// </summary>
    /// <returns></returns>
    public IKenchenParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    /// <summary>
    /// 摧毁自己
    /// </summary>
    public void DestorySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(this.gameObject);
    }

    public static KitchenObject SpawnKitchenobject(KitchenObjectSo kitchenObjectSo,IKenchenParent kenchenParent)
    {
        Transform KitchenObjectTransfrom = Instantiate(kitchenObjectSo.perfeb);
        KitchenObject kitchenObject = KitchenObjectTransfrom.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kenchenParent);
       
        return kitchenObject;   
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
