using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;

public class BreakoutUI : MonoBehaviour
{
    public string minigameSceneName = "BreakoutMinigame";

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

    private IEnumerator EndGameCoroutine()
    {
        if (gameWin)
        {
            gameOverText.text = "YOU    WIN!";
        } else
        {
            gameOverText.text = "YOU    LOSE!";
        }

        yield return new WaitForSeconds(5f);

        QuitMinigame();

        if (gameWin)
        {
            PlayerUIManager.Instance.UpdateBananaCounter(1);
            PlayerUIManager.Instance.ShowMessage("You've collected your first banana! Press [E] to open your inventory!");
        }

    }

    void Update()
    {
        if (!gameEnd && GameObject.FindGameObjectWithTag("Brick") == null) 
        {
            gameEnd = true;
            gameWin = true;
        }
        if (!gameEnd && GameObject.FindGameObjectWithTag("Ball") == null && GameObject.FindGameObjectWithTag("Brick") != null) 
        {
            gameEnd = true;
        }
        else if (!gameEnd && GameObject.FindGameObjectWithTag("Ball") == null){
            gameEnd = true;
            gameWin = true;
        }

        if (gameEnd)
        {
            StartCoroutine(EndGameCoroutine());
        }
    } 
}
