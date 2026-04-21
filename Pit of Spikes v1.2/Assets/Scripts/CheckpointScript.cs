using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public SpriteRenderer thisCheckpointRender;

    public GameObject[] checkpoints;

    public Sprite checkOn;
    public Sprite checkOff;


    private void Awake()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    public void DisableCheckpoint(bool a)
    {
        SoundManager.instance.PlayCheckpointSound();
        thisCheckpointRender.sprite = checkOff;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject thisCP in checkpoints)          // this does not run. ahgahghaghahghh
            {
                thisCP.BroadcastMessage("DisableCheckpoint", true);
            }
            thisCheckpointRender.sprite = checkOn;
            playerMovement.respawnPoint = this.gameObject;
        }
    }

}
