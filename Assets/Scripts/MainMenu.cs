using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI hScore;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        hScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("highscore");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
