using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string gameType;
    public int myScore;
    public int opponentScore;

    public PlayerData(string _gameType, int _myScore, int _opponentScore)
    {
        gameType = _gameType;
        myScore = _myScore;
        opponentScore = _opponentScore;
    }
}
