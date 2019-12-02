using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayMobile : MonoBehaviour
{
    public void OnInsideGameHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
