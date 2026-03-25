using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void OpenLevelSelect() 
    {
        
    }

    public void TestLevelOpen()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void RageQuit()
    {
        Application.Quit();
    }

}
