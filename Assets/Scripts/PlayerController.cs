
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.18f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Moving Platform")]
    [SerializeField] private string platformTag = "MovingPlatform";

    private Rigidbody2D rb;
    private float horizontal;
    private bool isGrounded;

    private Transform currentPlatform;
    private Vector2 previousPlatformPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        isGrounded = groundCheck != null &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                groundRadius,
                groundLayer
            );

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            DetachFromPlatform();
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }
    }

    private void FixedUpdate()
    {
        // Переносим персонажа вместе с платформой.
        if (currentPlatform != null)
        {
            Vector2 platformPosition = currentPlatform.position;
            Vector2 platformDelta =
                platformPosition - previousPlatformPosition;

            rb.position += platformDelta;
            previousPlatformPosition = platformPosition;
        }

        // Обычное горизонтальное движение.
        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryAttachToPlatform(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryAttachToPlatform(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentPlatform == collision.transform)
        {
            DetachFromPlatform();
        }
    }

    private void TryAttachToPlatform(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(platformTag))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Проверяем, что персонаж стоит сверху платформы.
            if (contact.normal.y > 0.5f)
            {
                if (currentPlatform != collision.transform)
                {
                    currentPlatform = collision.transform;
                    previousPlatformPosition = currentPlatform.position;
                }

                return;
            }
        }
    }

    private void DetachFromPlatform()
    {
        currentPlatform = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
