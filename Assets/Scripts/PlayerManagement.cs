using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public class Player
    {
        public string Name;
        public Color Color;
        public int Score;
        public Player(string Name, Color Color)
        {
            this.Name = Name;
            this.Color = Color;
            Score = 0;
        }
    }
    public Player Player1;
    public Player Player2;

    public void SetPlayers(bool isPlayingWithMobile)
    {
        if (isPlayingWithMobile)
        {
            SetMobileGamePlayer();
        }
        else
        {
            SetFrienGamePlayers();
        }
    }
    public void SetFrienGamePlayers()
    {
        string name1 = PlayerPrefs.GetString(GetComponent<Constants>().FRIENDGAMEPLAYERNAME1, "Player 1");
        string name2 = PlayerPrefs.GetString(GetComponent<Constants>().FRIENDGAMEPLAYERNAME2, "Player 2");

        Player1 = new Player(name1, Color.yellow);
        Player2 = new Player(name2, Color.cyan);
    }

    public void SetMobileGamePlayer()
    {
        string name1 = PlayerPrefs.GetString(GetComponent<Constants>().MOBILEGAMEPLAYERNAME, "Player 1");
        string name2 ="Mobile";

        Player1 = new Player(name1, Color.yellow);
        Player2 = new Player(name2, Color.cyan);
    }
}
