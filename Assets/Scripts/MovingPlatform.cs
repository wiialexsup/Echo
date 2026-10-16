
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float distance = 4f;
    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 platformVelocity;

    public Vector2 PlatformVelocity => platformVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        startPosition = rb.position;
    }

    private void FixedUpdate()
    {
        Vector2 oldPosition = rb.position;

        float x = startPosition.x +
                  Mathf.Sin(Time.fixedTime * speed) * distance;

        Vector2 newPosition = new Vector2(x, startPosition.y);

        platformVelocity = (newPosition - oldPosition) /
                           Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}
