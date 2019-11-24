using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviourPun
{
    public GameObject ShowGotData;
    [HideInInspector]
    public InputStr Input;
    public struct InputStr
    {
        public float LookX;
        public float LookZ;
        public float RunX;
        public float RunZ;
        public bool Jump;
    }
    System.Random rnd;

    private void Awake()
    {
        rnd = new System.Random();
        ShowGotData = GameObject.Find("GotData");
        //destroy the controller if the player is not controlled by me
        if (!photonView.IsMine)
        {
            //do something for your game object
        }
            
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
            position = player.transform.position;
            rotation = player.transform.rotation;
            PhotonNetwork.Destroy(player.gameObject);
        }

        player = PhotonNetwork.Instantiate(Prefab.gameObject.name, position, rotation).GetComponent<Player>();
    }


    public void OnSendDataButtonClick()
    {
        Debug.Log(photonView.ViewID);
        //Remote Procedure call
        photonView.RPC("SendData", RpcTarget.All, photonView.ViewID, rnd.Next(1,50));
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
}
