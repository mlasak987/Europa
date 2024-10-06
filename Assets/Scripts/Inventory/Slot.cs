using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Europa.Inventory
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] public Image image;
        private Item item;
        private Canvas canvas;

        private Vector3 movPos;
        public static Item movItem;
        public static Slot movSlot;
        private bool drag = false;
        private int index;

        public bool IsEmpty { get; private set; }

        private void Awake()
        {
            IsEmpty = true;
        }

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
        }

        private void OnMouseOver()
        {
            if (item != null && !drag) Inventory.Singleton.DisplayInfo(item);
            if (movItem != null && !drag && movSlot != this) { SetItem(movItem); movSlot = null; movItem = null; movPos = Vector3.zero; }
        }

        private void OnMouseExit()
        {
            if (item != null) Inventory.Singleton.HideInfo();
        }

        private void OnMouseDown()
        {
            if (item == null) return;

            InventoryHeader header = GetComponentInParent<InventoryHeader>();
            index = header.transform.GetSiblingIndex();
            header.transform.SetAsLastSibling();
            Inventory.Singleton.itemInfo.transform.SetAsLastSibling();

            movSlot = null; movItem = null; movPos = Vector3.zero;
            movPos = transform.position;
            transform.localScale *= 1.1f;

            drag = true;
        }

        private void OnMouseDrag()
        {
            if (item == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 movePos);
            transform.position = canvas.transform.TransformPoint(movePos);            
        }

        private void OnMouseUp()
        {
            if (item == null || !drag) return;
            Inventory.Singleton.HideInfo();
            GetComponentInParent<InventoryHeader>().transform.SetSiblingIndex(index);

            movItem = item;
            RemoveItem();

            transform.localScale = Vector3.one;
            transform.position = movPos;

            movSlot = this;
            drag = false;

            StartCoroutine(AutoDisable());
        }

        IEnumerator AutoDisable()
        {
            yield return new WaitForSeconds(0.05f);

            if (movItem != null)
            {
                SetItem(movItem);
                movSlot = null; movItem = null; movPos = Vector3.zero;
            }
        }

        public void SetItem(Item item)
        {
            this.item = item;
            IsEmpty = false;
            image.sprite = item.Sprite;
            image.gameObject.SetActive(true);
        }

        public void RemoveItem()
        {
            image.gameObject.SetActive(true);
            item = null;
            IsEmpty = true;
            image.gameObject.SetActive(false);
        }

        public Item GetItem()
        {
            return item;
        }
    }
}
