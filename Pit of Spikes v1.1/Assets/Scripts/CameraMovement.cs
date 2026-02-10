using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;

    private void LateUpdate()
    {
        Vector3 playerPosition = playerTransform.position;
        transform.position = new(playerPosition.x, playerPosition.y, transform.position.z);
    }
}
