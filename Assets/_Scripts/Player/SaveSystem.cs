using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int saveSlot = -1;

    public static void SavePlayer (PlayerController playerController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/player_slot{saveSlot}.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer (int slot)
    {
        if (slot == -1) return null;

        saveSlot = slot;
        string path = Application.persistentDataPath + $"/player_slot{slot}.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }

        return null;
    }

    public static void ClearSave(int slot)
    {
        string path = Application.persistentDataPath + $"/player_slot{slot}.txt";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save file for slot {slot} deleted.");
        }
    }
}
