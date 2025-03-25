using UnityEngine;
using UnityEngine.UI;

public class BreakoutGameManager : MonoBehaviour {
    public static BreakoutGameManager Instance;
    public Text scoreText;
    private int score;

    void Awake() {
        if (Instance == null) Instance = this;
    }

    public void AddScore(int points) {
        score += points;
        scoreText.text = $"Score: {score}";
    }
}
