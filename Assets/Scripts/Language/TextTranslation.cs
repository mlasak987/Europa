using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Europa.Language
{
	public class TextTranslation : MonoBehaviour
	{
        public static List<TextTranslation> list = new();

        [SerializeField] private string key;

        private string language;
        private TMP_Text text;

        private void OnEnable()
        {
            if (text == null) text = GetComponent<TMP_Text>();
            if (!list.Contains(this)) list.Add(this);
            UpdateText();
        }

        private void Start()
        {
            list.Add(this);
        }

        private void UpdateText()
        {
            if (LanguageManager.Language == language) return;
            text.text = LanguageManager.GetTranslation(key);
        }

        public static void UpdateAllTexts(string code)
        {
            foreach(TextTranslation translation in list)
            {
                translation.UpdateText();
                translation.language = code;
            }
        }

        private void OnDestroy()
        {
            list.Remove(this);
        }
    }
}
