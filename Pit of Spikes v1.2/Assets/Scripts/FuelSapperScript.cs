using UnityEngine;
using UnityEngine.Tilemaps;

public class FuelSapperScript : MonoBehaviour
{
    public TilemapCollider2D fuelSapperCollider;

    public PlayerMovement PlayerMovement;
    public LayerMask playerLayer;

    // FixedUpdate is called once per physics frame at a more consistent rate than Update
    void FixedUpdate()
    {
        if (fuelSapperCollider.IsTouchingLayers(playerLayer))
        {
            PlayerMovement.jetFuelL = 0;
            PlayerMovement.jetFuelR = 0;
            PlayerMovement.OnFuelChangeL?.Invoke();
            PlayerMovement.OnFuelChangeR?.Invoke();
        }
    }
}
