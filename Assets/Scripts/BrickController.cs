using UnityEngine;

public class BrickController : MonoBehaviour {
    public int points = 10;
    private BreakoutGameManager gameManager;

    private void Start()
    {
        //gameManager = GetComponent<BreakoutGameManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ball")) {
            //gameManager.AddScore(points);
            Destroy(gameObject);
        }
    }
}
