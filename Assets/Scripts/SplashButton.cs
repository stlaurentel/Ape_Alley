using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashButton : MonoBehaviour
{
    public string sceneName;

    public void ChangeScene() {
        SceneManager.LoadScene(sceneName);
    } 
}
