using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    public TMP_Text bananaText;
    public GameObject messagePanel;
    public TMP_Text messageText;
     
    public Vector2 uiOffset = new Vector2(0, 0);
    public float messageDuration = 3f;
    public Vector3 counterOffset = new Vector3(0,0,0);

    private Canvas _canvas;

    void Awake() {
        Instance = this;
        Debug.Log($"Before disabling - Panel active: {messagePanel.activeSelf}");
        if (messagePanel != null)
        {
            // This ensures we're working with the instantiated copy
            messagePanel = transform.Find("PlayerUICanvas/MessagePanel")?.gameObject ?? messagePanel;
            messagePanel.SetActive(false);
        }
        Debug.Log($"After disabling - Panel active: {messagePanel.activeSelf}");
    }

    public void Initialize(Transform player)
    {
        transform.SetParent(player);
        transform.localPosition = uiOffset;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        _canvas = GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.worldCamera = Camera.main;
    
        Canvas.ForceUpdateCanvases(); 
        
        messagePanel.SetActive(false);
        UpdateBananaCounter(0); 

    }

    public void UpdateBananaCounter(int count) 
    {
        bananaText.text = count.ToString();
    }

    public void ShowMessage(string message) 
    {
        messageText.text = message;
        Debug.Log($"Before enabling- Panel active: {messagePanel.activeSelf}");
        messagePanel.SetActive(true);
        Debug.Log($"After enabling- Panel active: {messagePanel.activeSelf}");
        Invoke(nameof(HideMessage), messageDuration);
    }

    private void HideMessage() => messagePanel.SetActive(false);
}
