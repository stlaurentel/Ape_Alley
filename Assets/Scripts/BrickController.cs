using UnityEngine;

public class BrickController : MonoBehaviour {
    public int points = 10;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ball")) {
            BreakoutGameManager.Instance.AddScore(points);
            Destroy(gameObject);
        }
    }
}
