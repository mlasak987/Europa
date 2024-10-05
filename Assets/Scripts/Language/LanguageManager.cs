using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Europa.Language
{
    public class LanguageManager
    {
        public static Dictionary<string, string> languageDictionary = new();
        public static string Language { get; private set; }

        public static void LoadLanguage(string code)
        {
            string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Languages"), $"{code}.json");
            string jsonText = File.ReadAllText(filePath);

            Language = code;
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);

            TextTranslation.UpdateAllTexts(code);
            DropdownTranslation.UpdateAllTexts(code);
        }

        public static string GetTranslation(string key)
        {
            if (languageDictionary.TryGetValue(key, out string value))
            {
                return value;
            }

            return "N/A";
        }
    }
}
