using UnityEngine;

public class PaddleController : MonoBehaviour {
    public float speed = 0.001f;
    private Rigidbody2D rb;
    public float leftBoundary = -8.85f;
    public float rightBoundary = 8.85f;
    private float paddleHalfWidth;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddleHalfWidth = GetComponent<BoxCollider2D>().bounds.size.x / 2;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(moveInput * speed * 0.6f, 0);

        // position of boundaries
        float clampedX = Mathf.Clamp(
            rb.position.x + movement.x * Time.fixedDeltaTime,
            leftBoundary + paddleHalfWidth,
            rightBoundary - paddleHalfWidth
        );

        Vector2 targetPos = new Vector2(clampedX, rb.position.y);
        rb.MovePosition(targetPos);
    }
}