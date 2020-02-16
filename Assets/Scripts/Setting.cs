using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Setting : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject ButtonSoundOff;
    public GameObject ButtonSoundOn;
    public GameObject ButtonNotificationOn;
    public GameObject ButtonNotificationOff;

    public Transform ListContainer;
    public GameObject ListItemPrefab;

    public GameObject GameHistoryPanel;



    private bool IsSoundOn;
    private bool IsNotificationOn;

    public class ListItem
    {
        public GameObject ItemObject;
        public TextMeshProUGUI GameType;
        public TextMeshProUGUI GameLevel;
        public TextMeshProUGUI OpponentName;
        public TextMeshProUGUI MyScore;
        public TextMeshProUGUI OpponentScore;
        public ListItem(GameObject SourcePrefab, Transform Parent, PlayerData playerData)
        {
            ItemObject = Instantiate(SourcePrefab, Parent);
            //TextObject = ItemObject.GetComponentInChildren<TextMeshProUGUI>();
            GameType = ItemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            GameLevel = ItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            OpponentName = ItemObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            MyScore = ItemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            OpponentScore = ItemObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            if (playerData.gameType != null)
            {
                GameType.SetText("Type: " + playerData.gameType.ToString());
            }

            if (playerData.gameLevel != null)
            {
                GameLevel.SetText("Level: " + playerData.gameLevel.ToString());
            }
            if (playerData.opponentName != null)
            {
                OpponentName.SetText("Opponent: " + playerData.opponentName.ToString());
            }
            MyScore.SetText("My score: " + playerData.myScore.ToString());
            OpponentScore.SetText("Opponent score: " + playerData.opponentScore.ToString());
        }
    }


    void Start()
    {
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
        IsSoundOn = PlayerPrefs.GetInt(GetComponent<Constants>().SETTINGSOUND, 1) == 1;
        IsNotificationOn = PlayerPrefs.GetInt(GetComponent<Constants>().SETTINGNOTIFICATION, 1) == 1;
        Debug.Log(IsSoundOn);
        if (IsSoundOn)
        {
            ButtonSoundOff.SetActive(true);
            ButtonSoundOn.SetActive(false);
        }
        else
        {
            ButtonSoundOff.SetActive(false);
            ButtonSoundOn.SetActive(true);
        }
        if (IsNotificationOn)
        {
            ButtonNotificationOff.SetActive(true);
            ButtonNotificationOn.SetActive(false);
        }
        else
        {
            ButtonNotificationOff.SetActive(false);
            ButtonNotificationOn.SetActive(true);
        }
        LoadGameHistory();
    }

    public void LoadGameHistory()
    {
        List<PlayerData> playerDatas = SaveSystem.LoadProgress();
        playerDatas.Reverse();
        playerDatas.ForEach(playerData =>
        {
            new ListItem(ListItemPrefab, ListContainer, playerData);
        });

    }


    public void OnSoundButtonClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        Debug.Log("Click");
        PlayerPrefs.SetInt(GetComponent<Constants>().SETTINGSOUND, IsSoundOn?0:1);
        IsSoundOn = !IsSoundOn;
        if (IsSoundOn)
        {
            StartCoroutine(CoroutineDeactiveActiveObject(ButtonSoundOff, ButtonSoundOn, "Sound_off"));
        }
        else
        {
            StartCoroutine(CoroutineDeactiveActiveObject(ButtonSoundOn, ButtonSoundOff, "Sound_on"));
        }
    }
    public void OnHomeClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        StartCoroutine(CoroutineLoadScene("MainMenu"));
    }
    public void OnNotificationButtonClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        Debug.Log("Click");
        PlayerPrefs.SetInt(GetComponent<Constants>().SETTINGNOTIFICATION, IsNotificationOn ? 0 : 1);
        IsNotificationOn = !IsNotificationOn;
        if (IsNotificationOn)
        {
            StartCoroutine(CoroutineDeactiveActiveObject(ButtonNotificationOff, ButtonNotificationOn, "Notification_off"));
        }
        else
        {
            StartCoroutine(CoroutineDeactiveActiveObject(ButtonNotificationOn, ButtonNotificationOff, "Notification_on"));
        }
    }
    public void OnShowHistoryClick()
    {
        GameHistoryPanel.SetActive(true);
    }
    public void OnHideHistoryClick()
    {
        GameHistoryPanel.SetActive(false);
    }
    public IEnumerator CoroutineLoadScene(string sceneName)
    {
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_end");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator CoroutineDeactiveObject(GameObject panelName, string triggetName)
    {
        MainCanvas.GetComponent<Animator>().SetTrigger(triggetName);
        yield return new WaitForSeconds(.5f);
        panelName.SetActive(false);
    }
    public IEnumerator CoroutineDeactiveActiveObject(GameObject activeObject, GameObject deactiveObject, string triggetName)
    {
        MainCanvas.GetComponent<Animator>().SetTrigger(triggetName);
        activeObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        deactiveObject.SetActive(false);
    }
}
