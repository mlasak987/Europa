using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Europa.Utils;
using UnityEngine;

namespace Europa.SaveSystem
{
    public static class SaveSystem
    {
        public static string SaveName = null;
        public static DateTime lastSave = DateTime.MinValue;
        public static UInt64 GameId = 0;

        public static void SaveGame(string name, GameManager gameManager)
        {
            if (!Directory.Exists(Application.persistentDataPath + $"/saves/")) Directory.CreateDirectory(Application.persistentDataPath + $"/saves/");
            string path = Application.persistentDataPath + $"/saves/{name}.dat";
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.OpenOrCreate);

            lastSave = DateTime.Now;
            GameId = gameManager.GameId;
            SaveName = name;
            SaveData data = new(gameManager);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static SaveData LoadData(string name)
        {
            if (!Directory.Exists(Application.persistentDataPath + $"/saves/")) Directory.CreateDirectory(Application.persistentDataPath + $"/saves/");
            string path = Application.persistentDataPath + $"/saves/{name}.dat";
            if (!File.Exists(path)) { Debug.LogError($"Error occured while trying to load {name}, file does not exist!"); return null; }
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }

        public static void LoadGame(string name, ref GameManager gameManager)
        {
            SaveData data = LoadData(name);

            gameManager.GameId = data.GameId;
        }
    }
}