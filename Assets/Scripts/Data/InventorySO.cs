using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<DataInventoryItem> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, DataInventoryItem>> OnInventoryUpdated;

        public void Initialze()
        {
            inventoryItems = new List<DataInventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(DataInventoryItem.GetEmptyItem());
            }
        }
        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemSate = null)
        {
            if(item.IsStackable == false)
            {
                for(int i = 0; i < inventoryItems.Count; i++)
                {
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirsFreetSlot(item, 1, itemSate);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackAbleItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirsFreetSlot(ItemSO item, int quantity, List<ItemParameter> itemSate = null)
        {
            DataInventoryItem newItem = new DataInventoryItem
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemSate == null ? item.DefaultParametersList : itemSate)
            };
            for(int i = 0; i <inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackAbleItem(ItemSO item, int quantity)
        {
            for(int i=0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if(inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;
                    if(quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while(quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirsFreetSlot(item, newQuantity);
            }
            return quantity;
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if(inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty)
                    return;
                int reminder = inventoryItems[itemIndex].quantity - amount;
                if (reminder <= 0)
                    inventoryItems[itemIndex] = DataInventoryItem.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangQuantity(reminder);
                InformAboutChange();
            }
        }

        public void AddItem(DataInventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, DataInventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, DataInventoryItem> returnValue = new Dictionary<int, DataInventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public DataInventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        internal void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            DataInventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }
    [Serializable]
    public struct DataInventoryItem
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;
        public DataInventoryItem ChangQuantity(int newQuantity)
        {
            return new DataInventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }
        public static DataInventoryItem GetEmptyItem()
            => new DataInventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>()
            };
    }
}