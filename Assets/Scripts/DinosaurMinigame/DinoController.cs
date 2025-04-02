using UnityEngine;

public class DinoController : MonoBehaviour
{
    public float jumpForce = 15f;
    public float duckSpeed = 5f;
    private Rigidbody2D rb;
    private bool isDucking = false;
    private Vector2 originalSize;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSize = GetComponent<BoxCollider2D>().size;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isDucking && rb.linearVelocity.y == 0) {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (!isDucking && rb.linearVelocity.y == 0) {
                isDucking = true;
                GetComponent<BoxCollider2D>().size = new Vector2(originalSize.x, originalSize.y / 2);
            }
        }
        else if (isDucking) {
            isDucking = false;
            GetComponent<BoxCollider2D>().size = originalSize;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Obstacle"))
    {
        FindFirstObjectByType<DinoGameManager>().GameOver();
    }
    }
}
