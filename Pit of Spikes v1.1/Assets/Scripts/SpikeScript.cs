using System;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public Collider2D spikeCollider;

    public PlayerMovement PlayerMovement;
    public LayerMask playerLayer;

    // FixedUpdate is called once per physics frame at a more consistent rate than Update
    void FixedUpdate()
    {
        if (spikeCollider.IsTouchingLayers(playerLayer))
        {
            PlayerMovement.OnTouchSpikes();
        }
    }
}
