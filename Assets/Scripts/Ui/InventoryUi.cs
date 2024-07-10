using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    List<InventoryItem> listOfUiItem = new List<InventoryItem>();

    public void InitializeInventoryUi(int inventorySize)
    {
        for(int i=0; i < inventorySize; i++)
        {
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(contentPanel);
            listOfUiItem.Add(item);
        }
    }
    public void ShowInventoryUI()
    {
        gameObject.SetActive(true);
    }
    public void HideInventoryUI()
    {
        gameObject.SetActive(false);
    }
}
