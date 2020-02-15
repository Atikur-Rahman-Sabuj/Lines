using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string gameType;
    public string gameLevel;
    public string opponentName;
    public int myScore;
    public int opponentScore;

    public PlayerData(string _gameType, string _gameLevel, string _opponentName, int _myScore, int _opponentScore)
    {
        gameType = _gameType;
        gameLevel = _gameLevel;
        opponentName = _opponentName;
        myScore = _myScore;
        opponentScore = _opponentScore;
    }
}
