using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UC Game Manager")]

    public Player PlayerPrefab;
    [Header("UI")]
    public GameObject PlayerLabel1;
    public GameObject PlayerLabel2;
    public GameObject PlayerScore1;
    public GameObject PlayerScore2;
    [HideInInspector]
    public Player LocalPlayer;
    public bool Turn;
    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MultiplayeJoin");
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        TmProSetText(PlayerLabel1, "Me");
        TmProSetText(PlayerLabel2, "Opponent");
        TmProSetText(PlayerScore1, "0");
        TmProSetText(PlayerScore2, "0");
        if (PhotonNetwork.IsMasterClient)
        {
            Turn = true;
        }
        else
        {
            Turn = false;
        }
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
    }
    public void onClickButtonSendData()
    {
        LocalPlayer.OnSendDataButtonClick();
    }
    public void OnTurnComplete(Vector3 startPoint, Vector3 endPoint, bool turn)
    {

        LocalPlayer.TurnChange( startPoint, endPoint, turn);
        Turn = turn;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
    }
    public void OnLeaveRoomButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left room");
        PhotonNetwork.Disconnect();
        
        //SceneManager.LoadScene("MultiplayeJoin");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        SceneManager.LoadScene("OnlineLobby");
    }
    public void IncrementScoreForBoxDrawn()
    {
        LocalPlayer.GetComponent<Player>().IncrementScore();
    }
    public void OnOpponentScoreUpdate(int opponentScore)
    {
        TmProSetText(PlayerScore2, opponentScore.ToString());
    }

    public void OnOwnScoreUpdate(int ownScore)
    {
        TmProSetText(PlayerScore1, ownScore.ToString());
    }
    void TmProSetText(GameObject gameObject, string value)
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText(value);
    }
}