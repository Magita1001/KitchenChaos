using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenobject())
        {
            player.GetKitchenObject().DestorySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }   
}
