using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public SpriteRenderer thisCheckpointRenderer;

    public SpriteRenderer[] allCheckpointRenderers;

    public string levelID;
    public int checkpointID;

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

            if (levelID == "troll")
            {
                PlayerPrefs.SetInt("trollCheckpointID", checkpointID);
                PlayerPrefs.Save();
            }
            if (levelID == "spire")
            {
                PlayerPrefs.SetInt("spireCheckpointID", checkpointID);
                PlayerPrefs.Save();
            }

        }
    }

}
