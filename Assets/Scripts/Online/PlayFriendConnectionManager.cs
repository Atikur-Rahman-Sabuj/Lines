using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayFriendConnectionManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    public GameObject InitialPanel;
    public GameObject CreateGamePanel;
    public GameObject GameCreatedPanel;
    public GameObject JoinGamePanel;
    public GameObject FailedPanel;
    public GameObject LoadingPanel;
    [Header("Text Inputs")]
    public TMP_InputField InputGameNameToCreate;
    public TMP_InputField InputGameNameToJoin;
    [Header("Text Outputs")]
    public TextMeshProUGUI TextGameName;
    public TextMeshProUGUI TextErrorMessage;

    private string PlayerName;
    private string GameName;
    private bool isWaitingForPlayer;
    private int counter;
    void Start()
    {
        PlayerName = PlayerPrefs.GetString(GetComponent<Constants>().ONLINEGAMEPLAYERNAME, "");
        PlayerName = PlayerName == "" ? "Player" : PlayerName;
        PhotonNetwork.NickName = PlayerName;
        PhotonNetwork.GameVersion = "v1";
        PhotonNetwork.AutomaticallySyncScene = true;
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

    }

    public void OnHomeClick()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
    public void OnTryAgainClick()
    {
        FailedPanel.SetActive(false);
        InitialPanel.SetActive(true);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master!");
    }
    //event when user click on create game on initial plane
    public void OnInitialCreateGameClick()
    {

        try
        {
            if(!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            CreateGamePanel.SetActive(true);
            InitialPanel.SetActive(false);
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }

        
    }
    //event when user click on join game on initial plane
    public void OnJoinGameInitialClick()
    {
        try
        {
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            JoinGamePanel.SetActive(true);
            InitialPanel.SetActive(false);
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
        
    }
    public void OnCreateGameClick()
    {
        GameName = InputGameNameToCreate.text;
        if (GameName.Equals(""))
            return;
        PhotonNetwork.CreateRoom(GameName, new RoomOptions { MaxPlayers = 2 });
        CreateGamePanel.SetActive(false);
        LoadingPanel.SetActive(true);
    }
    public void OnJoinGameclick()
    {
        GameName = InputGameNameToJoin.text;
        if (GameName.Equals(""))
            return;
        PhotonNetwork.JoinRoom(GameName);
        JoinGamePanel.SetActive(false);
        LoadingPanel.SetActive(true);
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        InitialPanel.SetActive(true);
        HideAllPanelsExceptInitial();
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        base.OnCreateRoomFailed(returnCode, message);
        LoadingPanel.SetActive(false);
        TextErrorMessage.text = "Couldn't create game, Please try again!";
        FailedPanel.SetActive(true);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created");
        LoadingPanel.SetActive(false);
        TextGameName.text = GameName;
        GameCreatedPanel.SetActive(true);
    }

    public void OnCreateAgainClick()
    {
        if(PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        GameCreatedPanel.SetActive(false);
        InitialPanel.SetActive(true);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
        TextErrorMessage.text = "Couldn't join game, Please try again!";
        FailedPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {

        base.OnJoinedRoom();
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Photon.Realtime.Player masterPlayer = PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.MasterClientId);
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
    private void ConnectToServer()
    {
        try
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        catch (System.Exception)
        {

            Debug.Log("Failed to connect");
        }
    }
    private void HideAllPanelsExceptInitial()
    {
         CreateGamePanel.SetActive(false);
        GameCreatedPanel.SetActive(false);
        JoinGamePanel.SetActive(false);
        FailedPanel.SetActive(false);
        LoadingPanel.SetActive(false);
    }

}
