using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTempLate; 

    private void Awake()
    {
        iconTempLate.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTempLate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSo kitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList())
        {            
            Transform iconTransform = Instantiate(iconTempLate, transform);
            iconTransform.gameObject.SetActive(true);    
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSo);
        }
    }
}
