using UnityEngine;
using TMPro;

public class DinoGameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    private int score = 0;
    private bool isGameOver = false;
    
    void Start()
    {
        InvokeRepeating("IncreaseScore", 1f, 0.5f);
    }

    void IncreaseScore() {
        if (!isGameOver) {
            score++;
            scoreText.text = "Score: " + score;
        }
    }

    public void GameOver() {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("DinosaurMinigame");
    }
}
