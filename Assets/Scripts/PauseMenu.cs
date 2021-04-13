using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public SC_CharacterController player;
    public GameObject playerUI;
    public GameObject weaponUI;
    public GameObject scoreText;
    public TextMeshProUGUI hScore;

    public void Start()
    {
        hScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("highscore");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        scoreText.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerUI.GetComponent<SC_EnemySpawner>().enabled = true;
        weaponUI.GetComponent<SC_WeaponManager>().enabled = true;
        player.canMove = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        scoreText.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        GameIsPaused = true;
        playerUI.GetComponent<SC_EnemySpawner>().enabled = false;
        weaponUI.GetComponent<SC_WeaponManager>().enabled = false;
        player.canMove = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
