using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject ParentCanvas;
    public GameObject MainCanvas;
    public GameObject PlayWithMobileCanvas;
    public GameObject PlayWithFriendCanvas;
    public GameObject PlayOnlineCanvas;
    public GameObject ExitPanel;

    [Header("Inputs")]
    public TMP_InputField InputMobilePlayer;
    public TMP_InputField InputFriendPlayer1;
    public TMP_InputField InputFriendPlayer2;
    public TMP_InputField InputOnlinePlayer;
    [Header("Buttons")]
    public GameObject HomeButton;
    public GameObject Easy;
    public GameObject Hard;

    private Constants constans;
    private string difficultyLevel;

    private void Start()
    {
        constans = GetComponent<Constants>();
        InputMobilePlayer.text = PlayerPrefs.GetString(constans.MOBILEGAMEPLAYERNAME, "");
        InputFriendPlayer1.text = PlayerPrefs.GetString(constans.FRIENDGAMEPLAYERNAME1, "");
        InputFriendPlayer2.text = PlayerPrefs.GetString(constans.FRIENDGAMEPLAYERNAME2, "");
        InputOnlinePlayer.text = PlayerPrefs.GetString(constans.ONLINEGAMEPLAYERNAME, "");
        ParentCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
        difficultyLevel = PlayerPrefs.GetString(GetComponent<Constants>().MOBILEGAMEMODE, GetComponent<Constants>().MOBILEGAMEMODEEASY);
        if (difficultyLevel.Equals(GetComponent<Constants>().MOBILEGAMEMODEEASY))
        {
            OnEasyClick();
        }
        else
        {
            OnHardClick();
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(true);
        }
        if (MainCanvas.activeSelf)
        {
            HomeButton.SetActive(false);
        }
        else
        {
            HomeButton.SetActive(true);
        }
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
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 7);
        StartCoroutine(CoroutineLoadScene("OnlinePlayRandomLobby"));
        //SceneManager.LoadScene("OnlinePlayRandomLobby");
    }
    public void OnOnlinePlayWithFriendClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name = InputOnlinePlayer.text;
        PlayerPrefs.SetString(GetComponent<Constants>().ONLINEGAMEPLAYERNAME, name);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 7);
        StartCoroutine(CoroutineLoadScene("OnlinePlayFriendLobby"));
        //SceneManager.LoadScene("OnlinePlayFriendLobby");

    }
    public void OnPlayMobileSixClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        string name = InputMobilePlayer.text;

        PlayerPrefs.SetString(GetComponent<Constants>().MOBILEGAMEPLAYERNAME, name);
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 6);
        StartCoroutine(CoroutineLoadScene("PlayMobile"));
        //SceneManager.LoadScene("PlayMobile");
    }

    public void OnEasyClick()
    {
        PlayerPrefs.SetString(GetComponent<Constants>().MOBILEGAMEMODE, GetComponent<Constants>().MOBILEGAMEMODEEASY);
        Easy.GetComponent<Image>().fillCenter = true;
        Easy.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        Hard.GetComponent<Image>().fillCenter = false;
        Color color;
        if (ColorUtility.TryParseHtmlString("#004355", out color))
        {
            Hard.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }
    }
    public void OnHardClick()
    {
        PlayerPrefs.SetString(GetComponent<Constants>().MOBILEGAMEMODE, GetComponent<Constants>().MOBILEGAMEMODEHARD);
        Easy.GetComponent<Image>().fillCenter = false;
        Color color;
        if (ColorUtility.TryParseHtmlString("#004355", out color))
        {
            Easy.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }
        Hard.GetComponent<Image>().fillCenter = true;
        Hard.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
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
        PlayerPrefs.SetInt(GetComponent<Constants>().TOTALPOINTS, 6);
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
    public void OnSettingClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        StartCoroutine(CoroutineLoadScene("Setting"));
    }
    public void OnExitCancelClick()
    {
        ExitPanel.SetActive(false);
    }
    public void OnExitConfirmClick()
    {
        Application.Quit();
    }
    public IEnumerator CoroutineLoadScene(string sceneName)
    {
        ParentCanvas.GetComponent<Animator>().SetTrigger("Scene_end");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }

    public void OnSaveProgressClick()
    {
        SaveSystem.SaveProgress("type2", "hard", "local", 20, 14);
    } 
    public void OnLoadProgressClick()
    {
        List<PlayerData> playerDatas = SaveSystem.LoadProgress();
        playerDatas.ForEach(playerData =>
        {
            Debug.Log(playerData.gameType + "TT" +  playerData.gameLevel + "TT" + playerData.opponentName + "TT" + playerData.myScore + "TT" + playerData.opponentScore);
        }); 
    }
    public void OnResetProgressClick()
    {
        SaveSystem.RemoveProgress();
    }

}
