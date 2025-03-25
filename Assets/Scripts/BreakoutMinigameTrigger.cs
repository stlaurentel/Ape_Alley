using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class BreakoutMinigameTrigger : MonoBehaviour
{
    public string minigameSceneName = "BreakoutMinigame";
    private EventSystem eventSystem;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine) {
            LoadMinigame();
            PausePlayerMovement(other.gameObject);
        }
    }

    private void startgame()
    {

            LoadMinigame();
            //PausePlayerMovement(other.gameObject);
    }

    private void LoadMinigame() {
        SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);
    }

    private void PausePlayerMovement(GameObject player) {
        // Disable player movement and/or other scripts
        player.GetComponent<PlayerMovement>().enabled = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //eventSystem.enabled = false;
            //Debug.Log("space");
            //startgame();
        }
    }
}
