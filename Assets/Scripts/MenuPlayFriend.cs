using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayFriend : MonoBehaviour
{
    public void OnInsideGameHomeClick()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        SceneManager.LoadScene("MainMenu");
    }
}
