using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UC Game Manager")]

    public Player PlayerPrefab;

    [HideInInspector]
    public Player LocalPlayer;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MultiplayeJoin");
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
    }
    public void onClickButtonSendData()
    {
        LocalPlayer.OnSendDataButtonClick();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
    }
}
