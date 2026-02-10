using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public Collider2D flagCollider;
    private bool applausePlayed;

    public PlayerMovement PlayerMovement;
    public LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        applausePlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagCollider.IsTouchingLayers(playerLayer) && !applausePlayed)
        {
            applausePlayed = true;
            SoundManager.instance.PlayApplause();
        }
    }
}
