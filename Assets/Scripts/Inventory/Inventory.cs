using Europa.Utils;
using System.Collections.Generic;
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

        [HideInInspector] public Slot[] Slots;

        [SerializeField] public List<Item> AvaiableItems;
        [SerializeField] public int MaxItems = 25;
        public int ItemsCount { get; private set; }
        [SerializeField] public ItemInfo itemInfo;
        [SerializeField] public Item EmptyItem;

        private void Awake()
        {
            Singleton = this;
            ItemsCount = 0;
        }

        private void Start()
        {
            Slots = FindObjectOfType<ItemContainer>().GetComponentsInChildren<Slot>();
            Player.Player.Singleton.Resume();
        }

        public void AddItem(Item item)
        {
            foreach (Slot slot in Slots)
                if (slot.IsEmpty) { slot.SetItem(item); break; }
        }

        public void RemoveItem(int at) => Slots[at].RemoveItem();
           
        public void PickUpItem(GameObject pickUpObject)
        {
            if (ItemsCount >= MaxItems) return;

            ColectableItem pickItem = pickUpObject.GetComponent<ColectableItem>();
            ToDestroy toDestroy = pickUpObject.GetComponentInParent<ToDestroy>();
            Destroy(toDestroy.gameObject);
            AddItem(AvaiableItems[pickItem.itemId]);
        }

        public void DisplayInfo(Item infoItem)
        {
            itemInfo.gameObject.SetActive(true);
            itemInfo.UpdateText($"<size=35>{infoItem.Name}</size>\n<size=24> Weight: {infoItem.Weight} kg</size>");
        }

        public void HideInfo()
        {
            itemInfo.gameObject.SetActive(false);
        }
    }
}