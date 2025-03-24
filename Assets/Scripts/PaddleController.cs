using UnityEngine;

public class PaddleController : MonoBehaviour {
    public float speed = 10f;
    private Rigidbody2D rb;

    void Start() => rb = GetComponent<Rigidbody2D>();

    void Update() {
        float move = Input.GetAxis("Horizontal") * speed;
        rb.linearVelocity = new Vector2(move, 0);
    }
}