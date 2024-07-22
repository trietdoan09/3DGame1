using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] private InventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private InventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;

        List<InventoryItem> listOfUiItem = new List<InventoryItem>();

        private int currentDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;
        private void Awake()
        {
            HideInventoryUI();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(contentPanel);
                listOfUiItem.Add(item);
                item.OnItemClicked += HandleItemSelection;
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        internal void ResetAllItem()
        {
            foreach(var item in listOfUiItem)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItem();
            listOfUiItem[itemIndex].Select();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUiItem.Count > itemIndex)
            {
                listOfUiItem[itemIndex].SetData(itemImage, itemQuantity);
            }
        }
        private void HandleShowItemActions(InventoryItem inventoryItemUI)
        {
            int index = listOfUiItem.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(InventoryItem inventoryItemUI)
        {
            ResetDraggtedItem();
        }

        private void HandleBeginDrag(InventoryItem inventoryItemUI)
        {
            int index = listOfUiItem.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }
        private void HandleSwap(InventoryItem inventoryItemUI)
        {
            int index = listOfUiItem.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggtedItem()
        {
            mouseFollower.Toggle(false);
            currentDraggedItemIndex = -1;
            return;
        }

        private void HandleItemSelection(InventoryItem inventoryItemUI)
        {
            int index = listOfUiItem.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void ShowInventoryUI()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItem();
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUiItem[itemIndex].transform.position;
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }

        private void DeselectAllItem()
        {
            foreach (var item in listOfUiItem)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void HideInventoryUI()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggtedItem();
        }
    }
}