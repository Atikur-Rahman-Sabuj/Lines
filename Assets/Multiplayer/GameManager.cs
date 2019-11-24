using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UC Game Manager")]

    public Player PlayerPrefab;

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
    public void OnTurnComplete()
    {
        LocalPlayer.TurnChange();
        Turn = false;
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
        SceneManager.LoadScene("MultiplayeJoin");
    }
}