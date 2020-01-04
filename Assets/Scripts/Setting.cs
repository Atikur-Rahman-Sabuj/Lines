using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject ButtonSoundOff;
    public GameObject ButtonSoundOn;
    public GameObject ButtonNotificationOn;
    public GameObject ButtonNotificationOff;

    private bool IsSoundOn;
    private bool IsNotificationOn;
    // Start is called before the first frame update
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
    }


    public void OnSoundButtonClick()
    {
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
        StartCoroutine(CoroutineLoadScene("MainMenu"));
    }
    public void OnNotificationButtonClick()
    {
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
