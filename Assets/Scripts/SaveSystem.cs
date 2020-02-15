using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SaveProgress(string gameType, string gameLevel, string opponentName, int myScore, int opponentScore)
    {
        List<PlayerData> playerDatas = SaveSystem.LoadProgress();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.myfiletype";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameType, gameLevel, opponentName, myScore, opponentScore);
        
        if (playerDatas == null)
        {
            playerDatas = new List<PlayerData>();
        }
        if (playerDatas.Count > 49)
        {
            playerDatas = playerDatas.GetRange(0, 49);
        }
        playerDatas.Insert(0, data);
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
    public static void RemoveProgress()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.myfiletype";
        FileStream stream = new FileStream(path, FileMode.Create);
        var playerDatas = new List<PlayerData>();
        formatter.Serialize(stream, playerDatas);
        stream.Close();
    }
}
