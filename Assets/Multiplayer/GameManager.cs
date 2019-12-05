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
    public GameObject GotoHomePanel;
    public GameObject OpponentLeftGamePanel;
    public GameObject WinPanel;
    public GameObject LoosePanel;
    public GameObject DrawPanel;
    public GameObject PlayAgainPanel;
    [HideInInspector]
    public Player LocalPlayer;
    public bool Turn;
    private String PlayerName;
    private String OpponentName;
    public int OwnScore;
    public int OpponentScore;
    public int TotalBox;
    private void Awake()
    {
        //if (!PhotonNetwork.IsConnected)
        //{
        //    SceneManager.LoadScene("MultiplayeJoin");
        //    return;
        //}
    }

    // Use this for initialization
    void Start()
    {
        TotalBox = 4;
        GotoHomePanel.SetActive(false);
        OpponentLeftGamePanel.SetActive(false);
        PlayerName = PlayerPrefs.GetString(GetComponent<Constants>().ONLINEGAMEPLAYERNAME, "");
        PlayerName = PlayerName == "" ? "Me" : PlayerName;

        OpponentName = PlayerPrefs.GetString(GetComponent<Constants>().ONLINEGAMEOPPONENTPLAYERNAME, "");
        OpponentName = OpponentName == "" ? "Opponent" : OpponentName;

        TmProSetText(PlayerLabel1, PlayerName);
        TmProSetText(PlayerLabel2, OpponentName);
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
        Debug.Log("OnPlayerEnterRoom");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left room");
        PhotonNetwork.Disconnect();
        
        SceneManager.LoadScene("MainMenu");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        SceneManager.LoadScene("MainMenu");
    }
    public void IncrementScoreForBoxDrawn()
    {
        LocalPlayer.GetComponent<Player>().IncrementScore();
    }
    public void OnOpponentScoreUpdate()
    {
        OpponentScore++;
        TmProSetText(PlayerScore2, OpponentScore.ToString());
        CheckResult();
    }
    private void CheckResult()
    {
        if ((OwnScore + OpponentScore) >= TotalBox)
        {
            if (OwnScore > OpponentScore)
            {
                OnWin(OwnScore, OpponentScore);
            }
            else if (OwnScore < OpponentScore)
            {
                OnLoose(OwnScore, OpponentScore);
            }
            else
            {
                OnDraw(OwnScore, OpponentScore);
            }
        }
    }
    public void OnOwnScoreUpdate()
    {
        OwnScore++;
        TmProSetText(PlayerScore1, OwnScore.ToString());
        CheckResult();
    }
    public void OnWin(int ownScore, int opponentScore)
    {
        WinPanel.SetActive(true);
    }
    public void OnLoose(int ownScore, int opponenetScore)
    {
        LoosePanel.SetActive(true);
    }
    public void OnDraw(int ownScore, int opponenetScore)
    {
        DrawPanel.SetActive(true);
    }
    public void OnEndGameHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnPlayAgainSendRequest()
    {
        LocalPlayer.PlayAgainSendRequest();
    }
    public void OnPlayAgainReceiveRequest()
    {
        PlayAgainPanel.SetActive(true);
    }
    public void OnPlayAgainAcceptRequest()
    {
        LocalPlayer.PlayAgainAcceptedRequestConfirmationSend();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnPlayAgainRequestDeclined()
    {
        PlayAgainPanel.SetActive(false);
    }
    public void OnPlayAgainOpponentRequestAccepted()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void TmProSetText(GameObject gameObject, string value)
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText(value);
    }
    public void OnHomeButtonClick()
    {
        GotoHomePanel.SetActive(true);
    }
    public void OnHomeConfirm()
    {
        LocalPlayer.PlayerLeftGame();
        PhotonNetwork.LeaveRoom();
    }
    public void OnGoHomeCancel()
    {
        GotoHomePanel.SetActive(false);
    }
}