using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeScript : MonoBehaviour
{
    public TilemapCollider2D spikeCollider;

    public PlayerMovement PlayerMovement;
    public LayerMask playerLayer;

    // FixedUpdate is called once per physics frame at a more consistent rate than Update
    void FixedUpdate()
    {
        if (spikeCollider.IsTouchingLayers(playerLayer))
        {
            PlayerMovement.KillPlayer();
        }
    }
}
