using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SaveProgress(string gameType, int myScore, int opponentScore)
    {
        List<PlayerData> playerDatas = SaveSystem.LoadProgress();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.myfiletype";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameType, myScore, opponentScore);
        
        if (playerDatas == null)
        {
            playerDatas = new List<PlayerData>();
        }
        playerDatas.Add(data);
        formatter.Serialize(stream, playerDatas);
        stream.Close();
    }
    public static List<PlayerData> LoadProgress()
    {
        string path = Application.persistentDataPath + "/progress.myfiletype";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                List<PlayerData> data = formatter.Deserialize(stream) as List<PlayerData>;
                stream.Close();
                return data;
            }
            catch (System.Exception)
            {
                stream.Close();
                return null;
            }
            
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
