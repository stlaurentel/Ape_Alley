using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    void Update() {
        lastVelocity = rb.linearVelocity;
    }

    void LaunchBall()
    {
        rb.linearVelocity = Vector2.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Maintain constant speed while bouncing
        float currentSpeed = lastVelocity.magnitude;
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.linearVelocity = direction * Mathf.Max(currentSpeed, speed);

        // Special paddle handling
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Add directional influence based on hit position
            float hitPosition = (transform.position.x - collision.transform.position.x) / 
                               collision.collider.bounds.size.x;
            direction = new Vector2(hitPosition, 1).normalized;
            rb.linearVelocity = direction * speed;
        }

        // Brick destruction
        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject);
            BreakoutGameManager.Instance.AddScore(10);
        }
    }
}