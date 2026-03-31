using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string LevelName;

    private void Start()
    {
        SelectLevel_None();
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
    }

    public void SelectLevel_Spire()
    {
        LevelName = "Spire Level";
    }

    public void OpenLevel()
    {
        if (LevelName != "None")
        {
            SceneManager.LoadSceneAsync(LevelName);
        }
    }

    public void RageQuit()
    {
        Application.Quit();
    }

}
