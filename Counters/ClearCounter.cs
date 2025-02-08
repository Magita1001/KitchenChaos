using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 灶台对象
/// </summary>
public class ClearCounter : BaseCounter
{
    public KitchenObjectSo kitchenObjectSo;



    /// <summary>
    /// 关于灶台上物体的交互
    /// </summary>
    public override void Interact(Player player)
    {
        if (!HasKitchenobject())
        {
            //如果灶台上没有厨房对象
            if (player.HasKitchenobject())
            {
                //如果玩家手上有厨房对象，放到自己身上
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //玩家手上有厨房对象的话
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
                else
                {
                    //玩家拿的不是盘子对象，并且自己身上有盘子
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //把玩家手中的对象尝试加入盘中
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            player.GetKitchenObject().DestorySelf();
                        }
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }    
}
