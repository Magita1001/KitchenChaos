using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemove;

    [SerializeField] private KitchenObjectSo plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private float platesSpawnedAmount;
    private float platesSpawnedAmountMax = 4f;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer >= spawnPlateTimerMax)
        {            
            spawnPlateTimer = 0f;
            if (GameManager.Instance.IsPlayGame() && platesSpawnedAmount <= platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenobject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenobject(plateKitchenObjectSO, player);
                OnPlateRemove?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
