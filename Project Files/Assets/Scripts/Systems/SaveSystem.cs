﻿using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int       initialStage = 1;
    public static int       initialIndex = 0;
    public static int       initialEnabledArms = 0;
    public static Vector3   initialPosition = new Vector3(-16.5f, -54, 0);

    public static void SaveGame(SaveData data)
    {
        BinaryFormatter formatter   = new BinaryFormatter();
        string path                 = Application.persistentDataPath + "/save.detached";
        FileStream stream           = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/save.detached";

        if (File.Exists(path))
        {
            BinaryFormatter formatter   = new BinaryFormatter();
            FileStream stream           = new FileStream(path, FileMode.Open);
            SaveData data               = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/save.detached";
        File.Delete(path);
    }
}
