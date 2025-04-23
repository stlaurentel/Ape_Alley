using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private float bottomBoundary = -5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    void Update() {
        lastVelocity = rb.linearVelocity;
        if (transform.position.y < bottomBoundary) {
            Destroy(gameObject);
        }
    }

    void LaunchBall()
    {
        rb.linearVelocity = Vector2.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || 
            collision.gameObject.CompareTag("Paddle") || 
            collision.gameObject.CompareTag("Brick"))
        {
            Debug.Log("ball collide");
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
}