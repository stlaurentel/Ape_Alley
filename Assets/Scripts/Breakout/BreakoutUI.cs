using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;

public class BreakoutUI : MonoBehaviour
{
    public string minigameSceneName = "BreakoutMinigame";
    public GameObject winTextUI; 
    public GameObject loseTextUI;
    private bool gameEnded = false;

    public void QuitMinigame() {
        SceneManager.UnloadSceneAsync(minigameSceneName);
        ResumePlayerMovement();
        BreakoutMinigameTrigger.Instance.canClickSpace = true;
        Debug.Log("canClickSpace = true");
    }

    private void ResumePlayerMovement() {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("Player");
        if (localPlayer != null && localPlayer.GetComponent<PhotonView>().IsMine) {
            localPlayer.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    private IEnumerator WinGameCoroutine()
    {
        if (winTextUI != null)
        {
            winTextUI.SetActive(true); // Show the win UI
        }
        
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        
        QuitMinigame();
        PlayerUIManager.Instance.UpdateBananaCounter(1);
        PlayerUIManager.Instance.ShowMessage("You've collected your first banana! Press [E] to open your inventory!");
    }

    private IEnumerator LoseGameCoroutine()
    {
        if (loseTextUI != null)
        {
            loseTextUI.SetActive(true); // Show the win UI
        }
        
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        
        QuitMinigame();
    }

    void Start() {
        if (winTextUI != null) {
            winTextUI.SetActive(false);
        }
        if (loseTextUI != null) {
            loseTextUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameEnded && GameObject.FindGameObjectWithTag("Brick") == null) 
        {
            gameEnded = true;
            StartCoroutine(WinGameCoroutine());
        }
        if (!gameEnded && GameObject.FindGameObjectWithTag("Ball") == null && BreakoutGameManager.Instance.score < 300) 
        {
            gameEnded = true;
            StartCoroutine(LoseGameCoroutine());
        }
        else if (!gameEnded && GameObject.FindGameObjectWithTag("Ball") == null && BreakoutGameManager.Instance.score > 300){
            gameEnded = true;
            StartCoroutine(WinGameCoroutine());
        }
    } 
}
