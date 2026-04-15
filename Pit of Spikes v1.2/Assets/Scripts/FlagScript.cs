using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FlagScript : MonoBehaviour
{
    public Collider2D flagCollider;
    private bool flagTouched;

    public PlayerMovement PlayerMovement;
    public LayerMask playerLayer;
    public GameHUDHandler GameHUDHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flagTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagCollider.IsTouchingLayers(playerLayer) && !flagTouched)
        {
            flagTouched = true;
            PlayerMovement.OnDisableInput();
            Time.timeScale = 0.1f;
            StartCoroutine(levelEndSequence());
        }
    }
    IEnumerator levelEndSequence()
    {
        yield return new WaitForSecondsRealtime(1);
        SoundManager.instance.PlayApplause();
        GameHUDHandler.ShowWinText();
        yield return new WaitForSecondsRealtime(16);
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
