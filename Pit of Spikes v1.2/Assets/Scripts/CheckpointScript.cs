using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public SpriteRenderer thisCheckpointRenderer;

    public SpriteRenderer[] allCheckpointRenderers;

    public Sprite checkOn;
    public Sprite checkOff;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") & thisCheckpointRenderer.sprite != checkOn)
        {
            foreach (SpriteRenderer thisCPR in allCheckpointRenderers)
            {
                thisCPR.sprite = checkOff;
            }
            SoundManager.instance.PlayCheckpointSound();
            thisCheckpointRenderer.sprite = checkOn;
            playerMovement.respawnPoint = this.gameObject;
        }
    }

}
