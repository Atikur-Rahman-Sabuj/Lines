using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkConnectionManager : MonoBehaviourPunCallbacks
{
    public GameObject BtnConnectMaster;
    public GameObject BtnConnectRoom;

    public bool TriesToConnectToMaster;
    public bool TriesToConnectToRoom;
    private string ROOMNAME = "my-custom-room-1";

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
    }

    void OnHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if(BtnConnectMaster!=null)
        BtnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
        if(BtnConnectRoom!=null)
        BtnConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
    }

    public void OnClickConnectToMaster()
    {
        //Settings (all optional and only for tutorial purpose)
        //PhotonNetwork.OfflineMode = false;           //true would "fake" an online connection
        PhotonNetwork.NickName = "PlayerName";       //to set a player name
        PhotonNetwork.AutomaticallySyncScene = true; //to call PhotonNetwork.LoadLevel()
        //PhotonNetwork.GameVersion = "v1";            //only people with the same game version can play together

        TriesToConnectToMaster = true;
        //PhotonNetwork.ConnectToMaster(ip,port,appid); //manual connection
        PhotonNetwork.ConnectUsingSettings();           //automatic connection based on the config file in Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TriesToConnectToMaster = false;
        Debug.Log("Connected to Master!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
        Debug.Log(cause);
    }

    public void OnClickConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        TriesToConnectToRoom = true;
        //PhotonNetwork.CreateRoom("Peter's Game 1"); //Create a specific Room - Error: OnCreateRoomFailed
        PhotonNetwork.JoinRoom(ROOMNAME);   //Join a specific Room   - Error: OnJoinRoomFailed  
        //PhotonNetwork.JoinRandomRoom();               //Join a random Room     - Error: OnJoinRandomRoomFailed  
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Room join fail message: " + message);
        //no room available
        //create a room (null as a name means "does not matter")
        PhotonNetwork.CreateRoom(ROOMNAME, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Room join fail message: " + message);
        //no room available
        //create a room (null as a name means "does not matter")
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2,  });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        base.OnCreateRoomFailed(returnCode, message);
        TriesToConnectToRoom = false;
    }

    public override void OnJoinedRoom()
    {
        if (TriesToConnectToRoom)
        {
            base.OnJoinedRoom();
            TriesToConnectToRoom = false;
            Debug.Log("test");
            Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name);
            SceneManager.LoadScene("OnlineGame");
        }
        
    }
   
}