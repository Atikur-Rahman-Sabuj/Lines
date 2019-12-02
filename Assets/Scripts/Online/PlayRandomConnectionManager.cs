using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayRandomConnectionManager : MonoBehaviourPunCallbacks
{
    public bool TriesToConnectToMaster;
    public bool TriesToConnectToRoom;
    private string PlayerName;
    void Start()
    {
        PlayerName = PlayerPrefs.GetString(GetComponent<Constants>().ONLINEGAMEPLAYERNAME, "");
        PlayerName = PlayerName == "" ? "Player" : PlayerName;
        PhotonNetwork.NickName = PlayerName;
        PhotonNetwork.GameVersion = "v1";
        try
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
       
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TriesToConnectToMaster = false;
        Debug.Log("Connected to Master!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
        Debug.Log(cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        base.OnCreateRoomFailed(returnCode, message);
        TriesToConnectToRoom = false;
    }

    public override void OnJoinedRoom()
    {

        base.OnJoinedRoom();
        TriesToConnectToRoom = false;
        Debug.Log("test");
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SceneManager.LoadScene("OnlineGame");
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        SceneManager.LoadScene("OnlineGame");

    }

}
