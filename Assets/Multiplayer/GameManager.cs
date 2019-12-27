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
    public GameObject Script;
    public Player PlayerPrefab;
    [Header("UI")]
    public GameObject MainCanvas;
    public GameObject PlayerLabel1;
    public GameObject PlayerLabel2;
    public GameObject PlayerScore1;
    public GameObject PlayerScore2;
    public GameObject TurnText;
    public GameObject GotoHomePanel;
    public GameObject OpponentLeftGamePanel;
    public GameObject WinPanel;
    public GameObject LoosePanel;
    public GameObject DrawPanel;
    public GameObject TimeFinishPanel;
    public GameObject PlayAgainPanel;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI TimePanelText;
    public TextMeshProUGUI ResultText;
    [HideInInspector]
    public Player LocalPlayer;
    public bool Turn;
    private String PlayerName;
    private String OpponentName;
    public int OwnScore;
    public int OpponentScore;
    public int TotalBox;
    public DrawLine drawLine;
    private Boolean ShowTime;
    private int RemainingTime = 60;
    private float waitTime = 1.0f;
    private float timer = 0.0f;
    private bool CheckTime = true;

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
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
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
        //TurnText.GetComponent<Animator>().SetBool("newTurn", true);
        TurnText.GetComponent<Animator>().Play("TurnChange");
        InitializeTime();
    }

    private void InitializeTime()
    {
        RemainingTime = 60;
        waitTime = 1.0f;
        timer = 0.0f;
        TimeText.SetText(RemainingTime.ToString());
        TimeText.gameObject.SetActive(true);


    }



    public void onClickButtonSendData()
    {
        LocalPlayer.OnSendDataButtonClick();
    }
    public void OnTurnComplete(Vector3 startPoint, Vector3 endPoint, bool turn)
    {

        LocalPlayer.TurnChange(startPoint, endPoint, turn);
        Turn = turn;
        InitializeTime();
        TurnText.GetComponent<Animator>().Play("TurnChange");
    }
    public void OnOpponentTurnComplete(Vector3 startPoint, Vector3 endPoint, bool turn)
    {

        Script.GetComponent<DrawLine>().ReflectOtherNetworkPlayerTurn(startPoint, endPoint, !PhotonNetwork.IsMasterClient);
        Turn = !turn;
        TurnText.GetComponent<Animator>().Play("TurnChange");
        InitializeTime();
    }
    // Update is called once per frame
    void Update()
    {
        if (Turn)
        {
            TmProSetText(TurnText, "Your turn");
        }
        else
        {
            TmProSetText(TurnText, OpponentName + "'s turn");
        }
        if (CheckTime)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                RemainingTime--;
                if (RemainingTime < 0 && Turn == true)
                {
                    onTimeFinish();
                }
                if (RemainingTime < -30 && Turn == false)
                {
                    onOpponetTimeFinish();
                }
                timer = timer - waitTime;
                TimeText.SetText(RemainingTime.ToString());
            }
        }

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
        Debug.Log("OnPlayerEnterRoom");
    }
    private void onTimeFinish()
    {
        TimeFinishPanel.SetActive(true);
        CheckTime = false;
        TimeText.SetText("");
        TimePanelText.SetText("Time out! You loose the game!");
        LocalPlayer.TimeEndedMessageSend();
    }

    public void onOpponetTimeFinish()
    {
        TimeFinishPanel.SetActive(true);
        CheckTime = false;
        TimeText.SetText("");
        TimePanelText.SetText(PlayerName+"'s time out! You won the game!");
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
        
        //SceneManager.LoadScene("MainMenu");
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
            TimeText.gameObject.SetActive(false);
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
        ResultText.text = "Congratulations! You won!";
        MainCanvas.GetComponent<Animator>().SetTrigger("RP_enter");
        WinPanel.SetActive(true);
    }
    public void OnLoose(int ownScore, int opponenetScore)
    {
        ResultText.text = "Better luck next time!";
        MainCanvas.GetComponent<Animator>().SetTrigger("RP_enter");
        WinPanel.SetActive(true);
        //LoosePanel.SetActive(true);
    }
    public void OnDraw(int ownScore, int opponenetScore)
    {
        ResultText.text = "Congratulations! You both won!";
        MainCanvas.GetComponent<Animator>().SetTrigger("RP_enter");
        WinPanel.SetActive(true);
        //DrawPanel.SetActive(true);
    }
    public void OnEndGameHomeClick()
    {
        StartCoroutine(CoroutineLoadScene("MainMenu"));
       // SceneManager.LoadScene("MainMenu");
    }
    public void OnPlayAgainSendRequest()
    {
        LocalPlayer.PlayAgainSendRequest();
    }
    public void OnPlayAgainReceiveRequest()
    {
        MainCanvas.GetComponent<Animator>().SetTrigger("PARP_enter");
        PlayAgainPanel.SetActive(true);
    }
    public void OnPlayAgainAcceptRequest()
    {
        LocalPlayer.PlayAgainAcceptedRequestConfirmationSend();
        StartCoroutine(CoroutineDeactiveObject(WinPanel, "RP_leave"));
        StartCoroutine(CoroutineDeactiveObject(PlayAgainPanel, "PARP_leave"));
        //WinPanel.SetActive(false);
        //PlayAgainPanel.SetActive(false);
        OwnScore = 0;
        OpponentScore = 0;
        TmProSetText(PlayerScore1, "0");
        TmProSetText(PlayerScore2, "0");
        drawLine.OnlineRestart();
    }
    public void OnPlayAgainRequestDeclined()
    {
        StartCoroutine(CoroutineDeactiveObject(PlayAgainPanel, "PARP_leave"));
        //PlayAgainPanel.SetActive(false);
    }
    public void OnPlayAgainOpponentRequestAccepted()
    {
        WinPanel.SetActive(false);
        LoosePanel.SetActive(false);
        DrawPanel.SetActive(false);
        PlayAgainPanel.SetActive(false);
        OwnScore = 0;
        OpponentScore = 0;
        TmProSetText(PlayerScore1, "0");
        TmProSetText(PlayerScore2, "0");
        drawLine.OnlineRestart();
    }
    void TmProSetText(GameObject gameObject, string value)
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText(value);
    }
    public void OnHomeButtonClick()
    {
        MainCanvas.GetComponent<Animator>().SetTrigger("HCP_enter");
        GotoHomePanel.SetActive(true);
    }
    public void OnHomeConfirm()
    {
        LocalPlayer.PlayerLeftGame();
        PhotonNetwork.LeaveRoom();
    }
    public void OnGoHomeCancel()
    {
        StartCoroutine(CoroutineDeactiveObject(GotoHomePanel, "HCP_leave"));
        //GotoHomePanel.SetActive(false);
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