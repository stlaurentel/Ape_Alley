using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;

public class MinigameUI : MonoBehaviour
{
    public string minigameSceneName = "BreakoutMinigame";
    //public GameObject winTextUI; 
    //public GameObject loseTextUI;
    public Text gameOverText;

    private bool gameEnd = false;
    private bool gameWin = false;

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

    //private IEnumerator WinGameCoroutine()
    //{
    //    if (winTextUI != null)
    //    {
    //        winTextUI.SetActive(true); // Show the win UI
    //    }
        
    //    yield return new WaitForSeconds(5f); // Wait for 5 seconds
        
    //    QuitMinigame();
    //    PlayerUIManager.Instance.UpdateBananaCounter(1);
    //    PlayerUIManager.Instance.ShowMessage("You've collected your first banana! Press [E] to open your inventory!");
    //}

    //private IEnumerator LoseGameCoroutine()
    //{
    //    if (loseTextUI != null)
    //    {
    //        loseTextUI.SetActive(true); // Show the win UI
    //    }
        
    //    yield return new WaitForSeconds(5f); // Wait for 5 seconds
        
    //    QuitMinigame();
    //}

    private IEnumerator EndGameCoroutine()
    {
        if (gameWin)
        {
            gameOverText.text = "YOU    WIN!";
        } else
        {
            gameOverText.text = "YOU    LOSE!";
        }

        //gameOverText.SetActive(true);

        yield return new WaitForSeconds(5f);

        QuitMinigame();

        if (gameWin)
        {
            PlayerUIManager.Instance.UpdateBananaCounter(1);
            PlayerUIManager.Instance.ShowMessage("You've collected your first banana! Press [E] to open your inventory!");
        }

    }

    void Start() {
        //if (winTextUI != null) {
        //    winTextUI.SetActive(false);
        //}
        //if (loseTextUI != null) {
        //    loseTextUI.SetActive(false);
        //}

        //if (gameOverText != null)
        //{
        //    gameOverText.SetActive(false);
        //}
    }

    void Update()
    {
        if (!gameEnd && GameObject.FindGameObjectWithTag("Brick") == null) 
        {
            gameEnd = true;
            gameWin = true;
            //StartCoroutine(WinGameCoroutine());
        }
        if (!gameEnd && GameObject.FindGameObjectWithTag("Ball") == null && GameObject.FindGameObjectWithTag("Brick") != null) 
        {
            gameEnd = true;
            //StartCoroutine(LoseGameCoroutine());
        }
        else if (!gameEnd && GameObject.FindGameObjectWithTag("Ball") == null){
            gameEnd = true;
            gameWin = true;
            //StartCoroutine(WinGameCoroutine());
        }

        if (gameEnd)
        {
            StartCoroutine(EndGameCoroutine());
        }
    } 
}
