using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;

public class BreakoutMinigameTrigger : MonoBehaviourPun
{
    public static BreakoutMinigameTrigger Instance;
    public string minigameSceneName = "BreakoutMinigame";
    private EventSystem eventSystem;
    private GameObject localPlayer;
    private bool canLoadScene = true;
    public bool canClickSpace = true;
    public bool touchingSpace = false;

    void Start()
    {   
        Instance = this;
        // get local playerobject
        localPlayer = GameObject.FindGameObjectWithTag("Player");
        if (localPlayer == null) {
            Debug.LogWarning("No player found");
        }
        // get event system safely
        eventSystem = EventSystem.current;
        if (eventSystem == null) {
            Debug.LogWarning("No EventSystem found in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)  && canLoadScene && canClickSpace && touchingSpace)
        {
            canClickSpace = false;
            StartCoroutine(StartGameRoutine());
        }
    }


    private IEnumerator StartGameRoutine()
    {
        // prevent multiple loads
        canLoadScene = false;
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
        }
        GameObject localPlayer = GetLocalPlayer();
        if (localPlayer != null)
        {
            PausePlayerMovement(localPlayer);
            yield return StartCoroutine(LoadMinigameAsync());
        }
        else
        {
            Debug.LogWarning("Local player not found");
        }
        
        canLoadScene = true;
    }

     private GameObject GetLocalPlayer()
    {
        // More reliable way to find local player in Photon
        PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);
        foreach (PhotonView view in photonViews)
        {
            if (view.IsMine && view.CompareTag("Player"))
            {
                return view.gameObject;
            }
        }
        Debug.LogWarning("Local player not found");
        return null;
    }

    private IEnumerator LoadMinigameAsync()
    {
        // wait for end of frame to avoid scene callback conflicts
        yield return new WaitForEndOfFrame();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // set new scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(minigameSceneName));
    }

    private void PausePlayerMovement(GameObject player) {
        // Disable player movement and/or other scripts
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    void onDestroy() {
        if (eventSystem != null) {
            eventSystem.enabled = true;
        }
    }

   

    private void LoadMinigame() {
        SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
}
