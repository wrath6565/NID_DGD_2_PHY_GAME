using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;

    private Rigidbody rb;
    private Vector3 startPosition;

    [Header("Death")]
    public float deathY = -10f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float airControlMultiplier = 0.4f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on Player!");
        }

        audioSource = GetComponentInChildren<AudioSource>();


        if (audioSource == null)
        {
            Debug.LogError("AudioSource missing on Player!");
        }

        startPosition = transform.position;
    }


    void Update()
    {
        CheckGround();

        // Respawn if player falls
        if (transform.position.y < deathY)
        {
            Respawn();
            return;
        }

        // Jump input
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && isGrounded)
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

        // 🔊 Play jump sound
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }


    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * groundCheckDistance
        );
    }

    void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;

        // Reset platforms (world reset, not timer/UI)
        PlatformSpawner spawner = FindObjectOfType<PlatformSpawner>();
        if (spawner != null)
        {
            spawner.ResetSpawner();
        }
    }
}
