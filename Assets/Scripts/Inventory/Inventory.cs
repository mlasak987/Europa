using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Europa.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private static Inventory _singleton;
        public static Inventory Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null) _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(Inventory)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }

        public List<Item> Items { get; private set; }

        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private Transform inventoryPanelPrefab;
        [SerializeField] public List<Item> AvaiableItems;
        [SerializeField] public int MaxItems = 32;

        private void Awake()
        {
            Singleton = this;
            Items = new();
        }

        public void AddItem(Item item)
        {
            if (Items.Count >= MaxItems) return;
            Items.Add(item);
            GameObject obj = Instantiate(inventoryItemPrefab, inventoryPanelPrefab);

            InventoryItem invItem = obj.GetComponent<InventoryItem>();
            invItem.nameText.text = item.name;
            invItem.spriteImage.sprite = item.sprite;
            invItem.weightText.text = $"{item.itemWeight} kg";
        }

        public void PickUpItem(GameObject pickUpObject)
        {
            if (Items.Count >= MaxItems) return;

            ColectableItem pickItem = pickUpObject.GetComponent<ColectableItem>();
            Destroy(pickUpObject);
            AddItem(AvaiableItems[pickItem.itemId]);
        }
    }
}