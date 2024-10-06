using System.Collections.Generic;
using UnityEngine;

namespace Europa.Inventory
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private Transform slotContainer;
        [HideInInspector] public List<Item> ContainerSlots;

        private Slot[] ContainerUISlots;

        private void Start()
        {
            ContainerUISlots = slotContainer.GetComponentsInChildren<Slot>();
            if (ContainerUISlots.Length == 0) Debug.LogError("No container UI detected!");

            for (int i = 0; i < ContainerUISlots.Length; i++) ContainerSlots.Add(Inventory.Singleton.EmptyItem);
        }

        public void LoadSlots()
        {
            for (int i = 0; i < ContainerSlots.Count; i++)
                if(ContainerSlots[i] != Inventory.Singleton.EmptyItem) ContainerUISlots[i].SetItem(ContainerSlots[i]); else ContainerUISlots[i].RemoveItem();
        }

        public void SaveSlots()
        {
            for (int i = 0; i < ContainerUISlots.Length; i++)
                if (!ContainerUISlots[i].IsEmpty) ContainerSlots[i] = ContainerUISlots[i].GetItem(); else ContainerSlots[i] = Inventory.Singleton.EmptyItem;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CameraCollider")) Player.Player.Singleton.InteractWithContainer(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("CameraCollider")) Player.Player.Singleton.DisableInteractionWithContainer();
        }
    }
}