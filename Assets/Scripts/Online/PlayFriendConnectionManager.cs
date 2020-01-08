using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayFriendConnectionManager : MonoBehaviourPunCallbacks
{
    public GameObject MainCanvas;

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
    private bool isGoingHome;
    void Start()
    {
        isGoingHome = false;
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
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
        FindObjectOfType<AudioManager>().Play("button_click");
        isGoingHome = true;
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        StartCoroutine(CoroutineLoadScene("MainMenu"));
       // SceneManager.LoadScene("MainMenu");
    }
    public void OnTryAgainClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        FailedPanel.SetActive(false);
        MainCanvas.GetComponent<Animator>().SetTrigger("IP_enter");
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
        FindObjectOfType<AudioManager>().Play("button_click");

        try
        {
            if(!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            MainCanvas.GetComponent<Animator>().SetTrigger("CGP_enter");
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
        FindObjectOfType<AudioManager>().Play("button_click");
        try
        {
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            MainCanvas.GetComponent<Animator>().SetTrigger("JGP_enter");
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
        FindObjectOfType<AudioManager>().Play("button_click");
        GameName = InputGameNameToCreate.text;
        if (GameName.Equals(""))
            return;
        PhotonNetwork.CreateRoom(GameName, new RoomOptions { MaxPlayers = 2 });
        StartCoroutine(CoroutineDeactiveObject(CreateGamePanel, "CGP_leave"));
        //CreateGamePanel.SetActive(false);
        LoadingPanel.SetActive(true);
    }
    public void OnJoinGameclick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        GameName = InputGameNameToJoin.text;
        if (GameName.Equals(""))
            return;
        PhotonNetwork.JoinRoom(GameName);
        StartCoroutine(CoroutineDeactiveObject(JoinGamePanel, "JGP_leave"));
        //JoinGamePanel.SetActive(false);
        LoadingPanel.SetActive(true);
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        if (!isGoingHome)
        {
            InitialPanel.SetActive(true);
            MainCanvas.GetComponent<Animator>().SetTrigger("IP_enter");
            HideAllPanelsExceptInitial();
        }
        
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        base.OnCreateRoomFailed(returnCode, message);
        LoadingPanel.SetActive(false);
        TextErrorMessage.text = "Couldn't create game, Please try again!";
        MainCanvas.GetComponent<Animator>().SetTrigger("FP_enter");
        FailedPanel.SetActive(true);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created");
        LoadingPanel.SetActive(false);
        TextGameName.text = GameName;
        MainCanvas.GetComponent<Animator>().SetTrigger("GCP_enter");
        GameCreatedPanel.SetActive(true);
    }

    public void OnCreateAgainClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        GameCreatedPanel.SetActive(false);
        MainCanvas.GetComponent<Animator>().SetTrigger("IP_enter");
        InitialPanel.SetActive(true);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
        TextErrorMessage.text = "Couldn't join game, Please try again!";
        LoadingPanel.SetActive(false);
        MainCanvas.GetComponent<Animator>().SetTrigger("FP_enter");
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
            StartCoroutine(CoroutineLoadScene("OnlineGame"));
            //SceneManager.LoadScene("OnlineGame");
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        string opponentName = newPlayer.NickName;
        PlayerPrefs.SetString(GetComponent<Constants>().ONLINEGAMEOPPONENTPLAYERNAME, opponentName);
        StartCoroutine(CoroutineLoadScene("OnlineGame"));
        //SceneManager.LoadScene("OnlineGame");

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
    public IEnumerator CoroutineLoadScene(string sceneName)
    {
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator CoroutineDeactiveObject(GameObject panelName, string triggetName)
    {
        MainCanvas.GetComponent<Animator>().SetTrigger(triggetName);
        yield return new WaitForSeconds(.5f);
        panelName.SetActive(false);
    }

}
