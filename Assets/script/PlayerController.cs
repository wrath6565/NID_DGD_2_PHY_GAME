using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float airControlMultiplier = 0.4f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on Player!");
        }
    }

    void Update()
    {
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float controlMultiplier = isGrounded ? 1f : airControlMultiplier;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * moveSpeed * controlMultiplier;

        rb.linearVelocity = velocity;
    }

    void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        isGrounded = Physics.Raycast(
            ray,
            groundCheckDistance,
            groundLayer
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * groundCheckDistance
        );
    }
}
