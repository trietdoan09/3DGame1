using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryUi inventoryUi;
    public int inventorySize = 10;

    private void Start()
    {
        inventoryUi.InitializeInventoryUi(inventorySize);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryUi.isActiveAndEnabled == false)
            {
                inventoryUi.ShowInventoryUI();
            }
            else
            {
                inventoryUi.HideInventoryUI();
            }
        }
    }
    
}
