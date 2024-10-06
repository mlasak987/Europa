using UnityEngine;

namespace Europa.Inventory
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string Name;
        public float Weight;
        public Sprite Sprite;
        public GameObject Prefab;
    }
}
