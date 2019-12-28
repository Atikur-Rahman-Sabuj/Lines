using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayRandomConnectionManager : MonoBehaviourPunCallbacks
{
    public GameObject MainCanvas;
    public GameObject LoadingPanel;
    public GameObject NotLoadingPanel;

    private string PlayerName;
    private bool isWaitingForPlayer;
    private int counter;
    private bool isGoingHome;
    void Start()
    {
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
        counter = 0;
        isGoingHome = false;
        isWaitingForPlayer = false;
        LoadingPanel.SetActive(true);
        NotLoadingPanel.SetActive(false);
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
        isGoingHome = true;
        if (PhotonNetwork.InRoom)
        PhotonNetwork.LeaveRoom();
        if(PhotonNetwork.IsConnected)
        PhotonNetwork.Disconnect();
        
        StartCoroutine(CoroutineLoadScene("MainMenu"));
        //SceneManager.LoadScene("MainMenu");
    }
    public void OnTryAgainClick()
    {
        counter = 0;
        isWaitingForPlayer = false;
        LoadingPanel.SetActive(true);
        StartCoroutine(CoroutineDeactiveObject(NotLoadingPanel, "NLP_leave"));
        //NotLoadingPanel.SetActive(false);
        try
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
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
        if (!isGoingHome)
        {
            LoadingPanel.SetActive(false);
            MainCanvas.GetComponent<Animator>().SetTrigger("NLP_enter");
            NotLoadingPanel.SetActive(true);
        }
        
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
        MainCanvas.GetComponent<Animator>().SetTrigger("NLP_enter");
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
