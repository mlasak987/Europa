using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Europa.Language
{
	public class DropdownTranslation : MonoBehaviour
	{
        public static List<DropdownTranslation> list = new();

        [SerializeField] private string[] keys;

        private string language;
        private TMP_Dropdown dropdown;

        private void OnEnable()
        {
            if (dropdown == null) dropdown = GetComponent<TMP_Dropdown>();
            if (!list.Contains(this)) list.Add(this);
            UpdateText();
        }

        private void UpdateText()
        {
            if (LanguageManager.Language == language) return;

            int value = dropdown.value;
            dropdown.ClearOptions();

            List<string> options = new();
            foreach (string key in keys)
            {
                options.Add(LanguageManager.GetTranslation(key));
            }

            dropdown.AddOptions(options);
            dropdown.value = value;
        }

        public static void UpdateAllTexts(string code)
        {
            foreach(DropdownTranslation translation in list)
            {
                translation.UpdateText();
                translation.language = code;
            }
        }
    }
}
