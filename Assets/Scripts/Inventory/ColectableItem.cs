using UnityEngine;
using Europa.Player;

namespace Europa.Inventory
{
    public class ColectableItem : MonoBehaviour
    {
        [SerializeField] public int itemId;

        private void Awake()
        {
            tag = "Outline";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CameraCollider")) Player.Player.Singleton.EnablePickUp(gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("CameraCollider")) Player.Player.Singleton.DisablePickUp();
        }
    }
}