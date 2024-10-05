using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Europa.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] public Image spriteImage;
        [SerializeField] public TMP_Text nameText;
        [SerializeField] public TMP_Text weightText;
    }
}