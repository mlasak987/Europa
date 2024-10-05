using UnityEngine;

namespace Europa.Inventory
{
    public class ColectableItem : MonoBehaviour
    {
        [SerializeField] public int itemId;

        private void Awake()
        {
            tag = "Colectable";
        }
    }
}