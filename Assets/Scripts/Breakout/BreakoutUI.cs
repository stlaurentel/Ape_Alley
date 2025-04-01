using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BreakoutUI : MonoBehaviour
{
    
    public string minigameSceneName = "BreakoutMinigame";

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
}
