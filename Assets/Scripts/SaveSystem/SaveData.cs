using System;
using Europa.Utils;

namespace Europa.SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public UInt64 GameId;

        public float Energy;
        public float Oxygen;
        public float Water;
        public float Food;

        public int Minutes, Days;

        public SaveData(GameManager gameManager) 
        {
            GameId = gameManager.GameId;
        }
    }
}
