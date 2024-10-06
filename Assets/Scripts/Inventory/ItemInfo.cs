using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Europa.Inventory
{
    public class ItemInfo : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0f, 50f, 0f);
        [SerializeField] private float padding = 25f;

        private RectTransform itemInfo;
        [SerializeField] private TMP_Text infoText;

        private void Awake()
        {
            itemInfo = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!gameObject.activeInHierarchy) { return; }

            CanvasScaler scaler = GetComponentInParent<CanvasScaler>();
            itemInfo.anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height) / 4f;
        }

        public void UpdateText(string text)
        {
            infoText.text = text;
        }
    }
}