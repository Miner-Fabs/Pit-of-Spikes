using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string LevelName;
    public GameObject continueButton;

    private void Start()
    {
        SelectLevel_None();
        if (PlayerPrefs.GetString("setupDone") != "true")
        {
            PlayerPrefs.SetInt("continueTroll", 1);
            PlayerPrefs.SetInt("continueSpire", 1);
            PlayerPrefs.SetInt("trollCheckpointID", 0);
            PlayerPrefs.SetInt("spireCheckpointID", 0);
            PlayerPrefs.SetString("setupDone", "true");
            PlayerPrefs.Save();
        }
    }

    public void SelectLevel_None()
    {
        LevelName = "None";
    }

    public void SelectLevel_Pit()
    {
        LevelName = "Pit Level";
    }

    public void SelectLevel_Troll()
    {
        LevelName = "Troll Level";
        if (PlayerPrefs.GetInt("trollCheckpointID") != 0)
        {
            continueButton.SetActive(true);
        }
    }

    public void SelectLevel_Spire()
    {
        LevelName = "Spire Level";
    }

    public void OpenLevel()
    {
        if (LevelName != "None")
        {
            if (LevelName == "Troll Level")
            {
                PlayerPrefs.SetInt("trollCheckpointID", 0);
                PlayerPrefs.Save();
            }
            else if (LevelName == "Spire Level")
            {
                PlayerPrefs.SetInt("spireCheckpointID", 0);
                PlayerPrefs.Save();
            }
            SceneManager.LoadSceneAsync(LevelName);
        }
    }
    public void ContinueLevel()
    {
        if(LevelName != "None")
        {
            if (LevelName == "Troll Level")
            {
                PlayerPrefs.SetInt("continueTroll", 0);
                PlayerPrefs.Save();
            }
            else if (LevelName == "Spire Level")
            {
                PlayerPrefs.SetInt("continueSpire", 0);
                PlayerPrefs.Save();
            }
            SceneManager.LoadSceneAsync(LevelName);
        }
    }

    public void RageQuit()
    {
        Application.Quit();
    }

}
