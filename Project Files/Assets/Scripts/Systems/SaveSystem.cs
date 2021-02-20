using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int       defaultStage = 1;
    public static int       defaultIndex = 0;
    public static int       defaultEnabledArms = 0;
    public static Vector3   defaultPosition = new Vector3(0, 0, 0);

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

    public static void SaveSettings(GameSettings settings)
    {
        BinaryFormatter formatter   = new BinaryFormatter();
        string path                 = Application.persistentDataPath + "settings.detached";
        if (File.Exists(path)) { File.Delete(path); }
        FileStream stream           = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, settings);
        stream.Close();
    }

    public static GameSettings LoadSettings()
    {
        string path = Application.persistentDataPath + "settings.detached";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter   = new BinaryFormatter();
            FileStream stream           = new FileStream(path, FileMode.Open);
            GameSettings settings       = formatter.Deserialize(stream) as GameSettings;
            stream.Close();
            return settings;
        }
        else
        {
            Debug.Log("Settings file not found in " + path);
            return null;
        }
    }
}
