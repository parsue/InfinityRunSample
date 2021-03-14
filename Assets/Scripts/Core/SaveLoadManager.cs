using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager
{
    private static readonly string root = string.Concat(Application.persistentDataPath, "/saves");

    public static bool Save<T>(string saveName, T saveData) where T : IGameData
    {
        if (!Directory.Exists(root))
        {
            Directory.CreateDirectory(root);
            Debug.LogWarningFormat("Create Directory {0}", root);
        }

        string path = string.Concat(root, "/", saveName, ".save");
        FileStream fs = File.Create(path);
        BinaryFormatter formatter = GetBinaryFormatter();
        try
        {
            formatter.Serialize(fs, saveData);
            fs.Close();
            return true;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to Save file at {0}", path);
            fs.Close();
            return false;
        }
    }

    public static object Load(string saveName)
    {
        string path = string.Concat(root, "/", saveName, ".save");
        if (!File.Exists(path)) return null;

        FileStream fs = File.OpenRead(path);
        BinaryFormatter formatter = GetBinaryFormatter();
        try
        {
            object save = formatter.Deserialize(fs);
            fs.Close();
            return save;
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            Debug.LogError(e.StackTrace);
            fs.Close();
            return null;
        }
    }

    private static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter;
    }
}
