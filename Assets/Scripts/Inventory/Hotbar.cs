using System.Collections.Generic;
using UnityEngine;

namespace Europa.Inventory
{
    public class Hotbar : MonoBehaviour
    {
        public static List<Item> Items;
        private Slot[] slots;

        [SerializeField] bool isInventory = false;

        private void Start()
        {
            slots = GetComponentsInChildren<Slot>();
            if (Items == null)
            {
                Items = new();
                for (int i = 0; i < 4; i++) Items.Add(Inventory.Singleton.EmptyItem);
            }
        }

        public void UpdateSlots()
        {
            if (isInventory) for (int i = 0; i < Items.Count; i++)
                    if (!slots[i].IsEmpty) Items[i] = slots[i].GetItem();
                    else Items[i] = Inventory.Singleton.EmptyItem;

            for (int i = 0; i < Items.Count; i++)
                if (Items[i] != Inventory.Singleton.EmptyItem) slots[i].SetItem(Items[i]);
                else slots[i].RemoveItem();
        }
    }
}