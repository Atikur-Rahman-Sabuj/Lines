using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerPlayMobile : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject ConfirmHomePanel;
    public GameObject WinningPanel;
    public TextMeshProUGUI WinningText;
    public TextMeshProUGUI FirstPlayerNameO;
    public TextMeshProUGUI SecondPlayerNameO;
    public TextMeshProUGUI FirstPlayerScoreO;
    public TextMeshProUGUI SecondPlayerScoreO;
    public GameObject Script;
    public GameObject Turn;
    private string FirstPlayerName;
    private string SecondPlayerName;
    private bool IsFirstPlayerTurn;
    private int FirstPlayerScore;
    private int SecondPlayerScore;
    private int TotalScore;
    private void Start()
    {
        FirstPlayerName = PlayerPrefs.GetString(Script.GetComponent<Constants>().MOBILEGAMEPLAYERNAME);
        SecondPlayerName = "Mobile";
        FirstPlayerNameO.SetText(FirstPlayerName);
        SecondPlayerNameO.SetText(SecondPlayerName);
        FirstPlayerScoreO.SetText("0");
        SecondPlayerScoreO.SetText("0");
        PlayerSwitch(true);
        int TotalPointInEachSide = PlayerPrefs.GetInt(Script.GetComponent<Constants>().TOTALPOINTS, 6);
        TotalScore = (TotalPointInEachSide - 1) * (TotalPointInEachSide - 1);
        MainCanvas.GetComponent<Animator>().SetTrigger("Scene_start");
    }
    public void onHomeClick()
    {
        ConfirmHomePanel.SetActive(true);
    }
    public void onGoHomeConfirm()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void onGoHomeCancel()
    {
        ConfirmHomePanel.SetActive(false);
    }

    public void onPlayAgainClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PlayerSwitch(bool isFirstPlayerTurn)
    {
        IsFirstPlayerTurn = isFirstPlayerTurn;
        if (isFirstPlayerTurn)
        {
            Turn.GetComponent<TextMeshProUGUI>().SetText(FirstPlayerName + "'s turn");
        }
        else
        {
            Turn.GetComponent<TextMeshProUGUI>().SetText(SecondPlayerName + "'s turn");
        }
        Turn.GetComponent<Animator>().Play("TurnChange");

    }
    public void OnScoreUpdate(bool isFirstPlayerScore)
    {
        if (isFirstPlayerScore)
        {
            FirstPlayerScoreO.SetText((++FirstPlayerScore).ToString());
        }
        else
        {
            SecondPlayerScoreO.SetText((++SecondPlayerScore).ToString());
        }

        if (TotalScore <= (FirstPlayerScore + SecondPlayerScore))
        {
            if (FirstPlayerScore > SecondPlayerScore)
            {
                WinningText.SetText(FirstPlayerName + " Won!!");
                WinningPanel.SetActive(true);
            }
            else if (SecondPlayerScore > FirstPlayerScore)
            {
                WinningText.SetText(SecondPlayerName + " Won!!");
                WinningPanel.SetActive(true);
            }
            else
            {
                WinningText.SetText("Congratulations, You both won!!");
                WinningPanel.SetActive(true);
            }
        }

    }
}
