using UnityEngine;

public class PaddleController : MonoBehaviour {
    public float speed = 10f;
    private Rigidbody2D rb;
    public float leftBoundary = -8.85f;
    public float rightBoundary = 8.85f;
    private float paddleHalfWidth;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddleHalfWidth = GetComponent<BoxCollider2D>().bounds.size.x / 2;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, 0);
        
        // position of boundaries
        float clampedX = Mathf.Clamp(
            transform.position.x,
            leftBoundary + paddleHalfWidth,
            rightBoundary - paddleHalfWidth
        );
        transform.position = new Vector2(clampedX, transform.position.y);
    }
}