﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject ParentCanvas;
    public GameObject MainCanvas;
    public GameObject PlayWithMobileCanvas;
    public GameObject PlayWithFriendCanvas;
    public GameObject PlayOnlineCanvas;

    [Header("Inputs")]
    public TMP_InputField InputMobilePlayer;
    public TMP_InputField InputFriendPlayer1;
    public TMP_InputField InputFriendPlayer2;
    public TMP_InputField InputOnlinePlayer;

    private Constants constans;

    private void Start()
    {
        constans = GetComponent<Constants>();
        InputMobilePlayer.text = PlayerPrefs.GetString(constans.MOBILEGAMEPLAYERNAME, "");
        InputFriendPlayer1.text = PlayerPrefs.GetString(constans.FRIENDGAMEPLAYERNAME1, "");
        InputFriendPlayer2.text = PlayerPrefs.GetString(constans.FRIENDGAMEPLAYERNAME2, "");
        InputOnlinePlayer.text = PlayerPrefs.GetString(constans.ONLINEGAMEPLAYERNAME, "");
        ParentCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
        
    }

    public void OnPlayWithMobileClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        MainCanvas.SetActive(false);
        PlayWithMobileCanvas.SetActive(true);
        ParentCanvas.GetComponent<Animator>().SetTrigger("MGMP_enter");
        
        
    }
    public void OnPlayWithFriend()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        MainCanvas.SetActive(false);
        ParentCanvas.GetComponent<Animator>().SetTrigger("PWFP_enter");
        PlayWithFriendCanvas.SetActive(true);
    }

    public void OnHomeClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        PlayWithMobileCanvas.SetActive(false);
        PlayWithFriendCanvas.SetActive(false);
        PlayOnlineCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        ParentCanvas.GetComponent<Animator>().SetTrigger("MMP_enter");
    }


    public void OnPlayOnlineClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        MainCanvas.SetActive(false);
        PlayOnlineCanvas.SetActive(true);
        ParentCanvas.GetComponent<Animator>().SetTrigger("POP_enter");
    }   
    public void OnOnlinePlayRandomClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name = InputOnlinePlayer.text;
        PlayerPrefs.SetString(GetComponent<Constants>().ONLINEGAMEPLAYERNAME, name);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 3);
        StartCoroutine(CoroutineLoadScene("OnlinePlayRandomLobby"));
        //SceneManager.LoadScene("OnlinePlayRandomLobby");
    }
    public void OnOnlinePlayWithFriendClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name = InputOnlinePlayer.text;
        PlayerPrefs.SetString(GetComponent<Constants>().ONLINEGAMEPLAYERNAME, name);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 3);
        StartCoroutine(CoroutineLoadScene("OnlinePlayFriendLobby"));
        //SceneManager.LoadScene("OnlinePlayFriendLobby");

    }
    public void OnPlayMobileSixClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name = InputMobilePlayer.text;

        PlayerPrefs.SetString(GetComponent<Constants>().MOBILEGAMEPLAYERNAME, name);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 2);
        StartCoroutine(CoroutineLoadScene("PlayMobile"));
        //SceneManager.LoadScene("PlayMobile");
    }
    public void OnPlayMobileEightClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name = InputMobilePlayer.text;

        PlayerPrefs.SetString(GetComponent<Constants>().MOBILEGAMEPLAYERNAME, name);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 8);
        StartCoroutine(CoroutineLoadScene("PlayMobile"));
        //SceneManager.LoadScene("PlayMobile");
    }
    public void OnPlayFriendSixClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name1 =  InputFriendPlayer1.text;
        string name2 = InputFriendPlayer2.text;

        PlayerPrefs.SetString(GetComponent<Constants>().FRIENDGAMEPLAYERNAME1, name1);
        PlayerPrefs.SetString(GetComponent<Constants>().FRIENDGAMEPLAYERNAME2, name2);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 2);
        StartCoroutine(CoroutineLoadScene("PlayFriend"));
        //SceneManager.LoadScene("PlayFriend");
    }
    public void OnPlayFriendEightClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name1 = InputFriendPlayer1.text;
        string name2 = InputFriendPlayer2.text;

        PlayerPrefs.SetString(GetComponent<Constants>().FRIENDGAMEPLAYERNAME1, name1);
        PlayerPrefs.SetString(GetComponent<Constants>().FRIENDGAMEPLAYERNAME2, name2);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 8);
        StartCoroutine(CoroutineLoadScene("PlayFriend"));
       // SceneManager.LoadScene("PlayFriend");
    }

    public IEnumerator CoroutineLoadScene(string sceneName)
    {
        ParentCanvas.GetComponent<Animator>().SetTrigger("Scene_end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }


}
