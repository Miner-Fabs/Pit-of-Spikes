using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public string levelID;

    [SerializeField] GameObject pauseMenu;

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        if (levelID == "troll")
        {
            PlayerPrefs.SetInt("trollCheckpointID", 0);
            PlayerPrefs.Save();
        }
        else if (levelID == "spire")
        {
            PlayerPrefs.SetInt("spireCheckpointID", 0);
            PlayerPrefs.Save();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
