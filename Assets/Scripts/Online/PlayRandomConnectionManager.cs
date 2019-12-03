using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayRandomConnectionManager : MonoBehaviourPunCallbacks
{
    public GameObject LoadingPanel;
    public GameObject NotLoadingPanel;

    private string PlayerName;
    private bool isWaitingForPlayer;
    private int counter;
    void Start()
    {
        counter = 0;
        isWaitingForPlayer = false;
        LoadingPanel.SetActive(true);
        NotLoadingPanel.SetActive(false);
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

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (isWaitingForPlayer)
        {
            Debug.Log(counter);
            counter++;
            if (counter > 1000)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                isWaitingForPlayer = false;
                LoadingPanel.SetActive(false);
                NotLoadingPanel.SetActive(true);
            }
        }
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        LoadingPanel.SetActive(false);
        NotLoadingPanel.SetActive(true);
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
        LoadingPanel.SetActive(false);
        NotLoadingPanel.SetActive(true);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created");
        isWaitingForPlayer = true;
        counter = 0;
    }
    public override void OnJoinedRoom()
    {

        base.OnJoinedRoom();
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Photon.Realtime.Player masterPlayer = PhotonNetwork.CurrentRoom.GetPlayer( PhotonNetwork.CurrentRoom.MasterClientId);
            PlayerPrefs.SetString(GetComponent<Constants>().ONLINEGAMEOPPONENTPLAYERNAME, masterPlayer.NickName);
            SceneManager.LoadScene("OnlineGame");
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        string opponentName = newPlayer.NickName;
        PlayerPrefs.SetString(GetComponent<Constants>().ONLINEGAMEOPPONENTPLAYERNAME, opponentName);
        SceneManager.LoadScene("OnlineGame");

    }

}
