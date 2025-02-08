using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct Kitchenobjectso_Gameobject
    {
        public KitchenObjectSo kitchenObjectSo;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject PlateKitchenObject;
    [SerializeField] private List<Kitchenobjectso_Gameobject> kitchenobjectsoGameobjectList;
    

    private void Start()
    {
        PlateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (Kitchenobjectso_Gameobject kitchenobjectsoGameobject in kitchenobjectsoGameobjectList)
        {
            kitchenobjectsoGameobject.gameObject.SetActive(false);
        }
        
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (Kitchenobjectso_Gameobject kitchenobjectsoGameobject in kitchenobjectsoGameobjectList)
        {
            if (kitchenobjectsoGameobject.kitchenObjectSo == e.kitchenObjectSo)
            {
                kitchenobjectsoGameobject.gameObject.SetActive(true);
            }
        }
    }
}
