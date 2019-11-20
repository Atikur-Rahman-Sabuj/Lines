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


    public void SetPlayers()
    {
        Player1 = new Player("Player1", Color.yellow);
        Player2 = new Player("Player2", Color.cyan);
    }
}
