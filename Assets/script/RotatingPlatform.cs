using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 180f;
    public Vector3 rotationAxis = Vector3.forward;

    void FixedUpdate()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.fixedDeltaTime);
    }
}
