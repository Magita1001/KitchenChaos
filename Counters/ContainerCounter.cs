using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSo kitchenObjectSo;
    public event EventHandler OnPlayerGrabbedObject;

    /// <summary>
    /// 生成灶台对应的物体
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        //如果玩家手中为空，生成道具随后交给玩家
        if (!player.HasKitchenobject())
        {
            KitchenObject.SpawnKitchenobject(kitchenObjectSo, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
