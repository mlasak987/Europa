using Europa.Language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Europa.Utils
{
    public class JournalItem : MonoBehaviour
    {
        [SerializeField] string translationKey;
        [SerializeField] int fontSize = 36;
        [SerializeField] GameObject[] expandObjects;
        [SerializeField] public int tab = 0;
        private TMP_Text text;

        private void OnEnable()
        {
            text = GetComponent<TMP_Text>();
            text.margin = new Vector4(10 + tab * 50, 5, 0, 5);
            text.fontSize = fontSize;
            if (expandObjects.Length == 0 || (expandObjects.Length == 1 && expandObjects[0] == this) || !Player.Player.Singleton.UnlockedLore[transform.GetSiblingIndex()]) text.text = $"{LanguageManager.GetTranslation(translationKey)}";
            else text.text = $"{LanguageManager.GetTranslation(translationKey)} +";

            GetComponentInParent<VerticalLayoutGroup>().SetLayoutVertical();
        }

        private void Start()
        {
            if (tab == 0) Expand();
        }

        public void Expand()
        {
            if (expandObjects.Length == 0 || (expandObjects.Length == 1 && expandObjects[0] == this) || !Player.Player.Singleton.UnlockedLore[transform.GetSiblingIndex()]) return;

            foreach (GameObject expandObject in expandObjects)
            {
                if (!expandObject.activeInHierarchy && expandObject.GetComponent<JournalItem>().tab > tab + 1) continue;

                expandObject.SetActive(!expandObject.activeInHierarchy);
                if (expandObject.activeInHierarchy) text.text = $"{LanguageManager.GetTranslation(translationKey)} -";
                else text.text = $"{LanguageManager.GetTranslation(translationKey)} +";
            }
        }
    }
}
