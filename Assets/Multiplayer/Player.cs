using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviourPun
{
    public GameObject ShowGotData;
    public GameObject GameManager;
    public GameObject Script;

    System.Random rnd;

    private void Awake()
    {
        
        //destroy the controller if the player is not controlled by me
        if (!photonView.IsMine)
        {
            //do something for your game object
        }

    }
    private void Start()
    {
        
        rnd = new System.Random();
        ShowGotData = GameObject.Find("GotData");
        GameManager = GameObject.Find("GameManager");
        Script = GameObject.Find("script");

    }


    private void Update()
    {



    }

    void FixedUpdate()
    {


    }

    private void LateUpdate()
    {

    }

    public static void RefreshInstance(ref Player player, Player Prefab)
    {
        Debug.Log("RefreshInstance");
        var position = Vector3.zero;
        var rotation = Quaternion.identity;
        if (player != null)
        {
            PhotonNetwork.Destroy(player.gameObject);
        }

        player = PhotonNetwork.Instantiate(Prefab.gameObject.name, position, rotation).GetComponent<Player>();

    }


    public void OnSendDataButtonClick()
    {
        Debug.Log(photonView.ViewID);
        //Remote Procedure call
        photonView.RPC("SendData", RpcTarget.All, photonView.ViewID, rnd.Next(1, 50));
    }

    [PunRPC]
    void SendData(int senderViewId, int value, PhotonMessageInfo info)
    {
        Debug.Log(senderViewId.ToString() + "   " + photonView.ViewID);
        Debug.Log(value);
        Debug.Log(photonView.IsMine);

        if (!photonView.IsMine)
        {
            ShowGotData.GetComponent<TextMeshProUGUI>().SetText(value.ToString());
        }

        //Properties
        //PhotonNetwork.LocalPlayer.CustomProperties.Add("state", "dead");
        //access by: PhotonNetwork.PlayerListOthers[0].CustomProperties["state"]
    } 
    public void TurnChange(Vector3 startPoint, Vector3 endPoint, bool turn)
    {
        Debug.Log(photonView.ViewID);
        //Remote Procedure call
        photonView.RPC("ChangeTurn", RpcTarget.All, photonView.ViewID, startPoint, endPoint, turn);
    }

    [PunRPC]
    void ChangeTurn(int senderViewId, Vector3 startPoint, Vector3 endPoint, bool turn, PhotonMessageInfo info)
    {
       // Debug.Log(senderViewId.ToString() + "   " + photonView.ViewID);
       // Debug.Log(photonView.IsMine);

        if (!photonView.IsMine)
        {
            Debug.Log("Whats my turn: "+ !turn);
            //reflect players turn to other player view
            //Script.GetComponent<DrawLine>().ReflectOtherNetworkPlayerTurn(startPoint, endPoint, !PhotonNetwork.IsMasterClient);
            //GameManager.GetComponent<GameManager>().Turn = !turn;
           // GameManager.GetComponent<GameManager>().TurnText.GetComponent<Animator>().Play("TurnChange");
            GameManager.GetComponent<GameManager>().OnOpponentTurnComplete(startPoint, endPoint, turn);
        }

        //Properties
        //PhotonNetwork.LocalPlayer.CustomProperties.Add("state", "dead");
        //access by: PhotonNetwork.PlayerListOthers[0].CustomProperties["state"]
    }

    public void IncrementScore()
    {
        Debug.Log(photonView.ViewID);
        GameManager.GetComponent<GameManager>().OnOwnScoreUpdate();
        photonView.RPC("OponentIncrementScore", RpcTarget.All, photonView.ViewID);
       
        //Remote Procedure call
        
    }

    [PunRPC]
    void OponentIncrementScore(int senderViewId, PhotonMessageInfo info)
    {
        Debug.Log(senderViewId.ToString() + "   " + photonView.ViewID);
        if (!photonView.IsMine)
        {

            GameManager.GetComponent<GameManager>().OnOpponentScoreUpdate();
        }

        //Properties
        //PhotonNetwork.LocalPlayer.CustomProperties.Add("state", "dead");
        //access by: PhotonNetwork.PlayerListOthers[0].CustomProperties["state"]
    }

    public void PlayerLeftGame()
    {
        Debug.Log(photonView.ViewID);
        //Remote Procedure call
        photonView.RPC("OnOpponentLeftGame", RpcTarget.All, photonView.ViewID);
    }

    [PunRPC]
    void OnOpponentLeftGame(int senderViewId, PhotonMessageInfo info)
    {

        if (!photonView.IsMine)
        {
            GameManager.GetComponent<GameManager>().OpponentLeftGamePanel.SetActive(true);
        }
    }

    public void PlayAgainSendRequest()
    {
        photonView.RPC("PlayAgainGotRequest", RpcTarget.All, photonView.ViewID);
    }

    [PunRPC]
    void PlayAgainGotRequest(int senderViewId, PhotonMessageInfo info)
    {
        if (!photonView.IsMine)
        {
            GameManager.GetComponent<GameManager>().OnPlayAgainReceiveRequest();
        }
    }
    
    public void PlayAgainAcceptedRequestConfirmationSend()
    {

        photonView.RPC("PlayAgainAcceptedConfirm", RpcTarget.All, photonView.ViewID);
    }

    [PunRPC]
    void PlayAgainAcceptedConfirm(int senderViewId, PhotonMessageInfo info)
    {
        if (!photonView.IsMine)
        {
            GameManager.GetComponent<GameManager>().OnPlayAgainOpponentRequestAccepted();
        }
    }
    public void TimeEndedMessageSend()
    {

        photonView.RPC("TimeEndedMessageReceive", RpcTarget.All, photonView.ViewID);
    }

    [PunRPC]
    void TimeEndedMessageReceive(int senderViewId, PhotonMessageInfo info)
    {
        if (!photonView.IsMine)
        {
            GameManager.GetComponent<GameManager>().onOpponetTimeFinish();
        }
    }
}