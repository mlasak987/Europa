using UnityEngine;

namespace Europa.Inventory
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string itemName;
        public float itemWeight;
        public Sprite sprite;
        public GameObject prefab;
    }
}
