using System;
using System.Linq;
using UnityEngine;

namespace Europa.Utils
{
    public class GameManager : MonoBehaviour
    {
        public UInt64 GameId = 0;
        public TimeSystem TimeSystem { get; private set; }

        private void Awake()
        {
            TimeSystem = GetComponentInChildren<TimeSystem>();
            if (TimeSystem == null) Debug.LogError($"TimeSystem not initialized corectly!");
        }

        private void Start()
        {
            if (SaveSystem.SaveSystem.SaveName == null && GameId == 0)
            {
                string a = DateTime.Now.ToString();
                a = new string((from c in a where char.IsDigit(c) select c).ToArray());
                GameId = UInt64.Parse(a) % 99999999977;
            }
        }

        public void ResetGame()
        {
        }
    }
}
