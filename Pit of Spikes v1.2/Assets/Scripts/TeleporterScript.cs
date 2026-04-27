using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    public GameObject TeleportExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.PlayTeleportSound();
            other.transform.position = TeleportExit.transform.position;
        }
    }

}
