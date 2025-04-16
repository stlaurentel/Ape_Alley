using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using System; 
using System.Collections.Generic;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [Header("Prefab References")]
    public GameObject chatCanvasPrefab;
    public GameObject messageBubblePrefab;
    
    [Header("UI Settings")]
    public float messageBubbleDuration = 3f;
    
    // UI References
    private TMP_InputField chatInput;
    private TMP_Text chatLogText;
    private GameObject chatPrompt;
    private GameObject chatLogPanel;
    
    // Runtime variables
    private GameObject chatInstance;
    private bool isChatOpen = false;

    private void Awake() 
    {
        // Verify we have a PhotonView
        if (photonView == null)
        {
            Debug.LogError("ChatManager requires a PhotonView on the same GameObject!", this);
            return;
        }

        // Instantiate the chat UI
        chatInstance = Instantiate(chatCanvasPrefab, transform);
        InitializeChatReferences();

        Debug.Log($"Chat canvas created for {(photonView.IsMine ? "LOCAL" : "REMOTE")} player");

        // Only enable interaction for local player
        if (!photonView.IsMine) 
        {
            if (chatInput != null) chatInput.interactable = false;
            if (chatPrompt != null) chatPrompt.SetActive(false);
        }
    }

    private void InitializeChatReferences()
    {
        // Ensure this runs for ALL players
        chatInput = chatInstance.transform.Find("InputPromptPanel/ChatInput")?.GetComponent<TMP_InputField>();
        chatLogPanel = chatInstance.transform.Find("ChatLogPanel")?.gameObject;
        chatLogText = chatLogPanel?.transform.Find("ChatLogText")?.GetComponent<TMP_Text>();
        chatPrompt = chatInstance.transform.Find("InputPromptPanel/ChatPrompt")?.gameObject;

        if (photonView.IsMine) {
        if (chatLogText != null) {
            chatLogText.text += "\n";
        }
        // Initialize UI state
        if (chatInput != null) 
            chatInput.gameObject.SetActive(false);
    
        if (chatPrompt != null)
            chatPrompt.SetActive(photonView.IsMine); // Only show prompt for local player
    
        // ACTIVATE CHAT LOG FOR EVERYONE
        if (chatLogPanel != null)
            chatLogPanel.SetActive(true);

        if (photonView.IsMine && chatInput != null)
        {
            chatInput.onValueChanged.AddListener(UpdateTypingPrompt);
        }
        }
        else {
            if (chatInstance != null) {
                chatInstance.SetActive(false);
            }
        }
            
    }

    private void UpdateTypingPrompt(string currentText)
    {
        if (!isChatOpen || chatPrompt == null) return;
        TMP_Text promptText = chatPrompt.GetComponent<TMP_Text>();
    
        if (currentText.StartsWith("/"))
        {
            promptText.text = currentText.Substring(1); // Show text after slash
        }
        else
        {
            promptText.text = currentText; // Show full text if no slash
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        // Open chat with / key
        if ((Input.GetKeyDown(KeyCode.Slash) || 
             Input.GetKeyDown(KeyCode.Backslash)) && !isChatOpen)
        {
            OpenChat();
        }
        
        // Send message on Enter
        if (isChatOpen && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput?.text))
        {
            SendChatMessage();
        }
        
        // Close chat on Escape
        if (isChatOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChat();
        }
    }

    private void OpenChat()
    {
        if (!photonView.IsMine || chatInput == null || chatPrompt == null) return;
    
        isChatOpen = true;
        chatInput.gameObject.SetActive(true);
        chatPrompt.SetActive(true);
        StartCoroutine(ForceInputFieldFocus());
        TMP_Text promptText = chatPrompt.GetComponent<TMP_Text>();
        promptText.text = "";
    
        GameObject player = GetLocalPlayer();
        if (player != null) 
        {
            // Only disable movement on the local player
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = false;
                movement.playerTyping = true;
                player.GetComponent<PhotonView>().RPC("SetTypingState", RpcTarget.Others, true);
            }
            else {
                Debug.LogError("PlayerMovement component not found on player!");
            }
        }
    }
    
    private IEnumerator ForceInputFieldFocus()
    {
        yield return null; // Wait one frame
        chatInput?.Select();
        chatInput?.ActivateInputField();
    }

    private void CloseChat()
    {
        if (!photonView.IsMine || chatInput == null || chatPrompt == null) return;
    
        isChatOpen = false;
        chatInput.gameObject.SetActive(false);
        chatPrompt.SetActive(true);
        TMP_Text promptText = chatPrompt.GetComponent<TMP_Text>();
        promptText.text = "Press / to chat...";
        chatInput.text = "";
    
        GameObject player = GetLocalPlayer();
        if (player != null) 
        {
            // Only enable movement on the local player
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = true;
                movement.playerTyping = false;
                player.GetComponent<PhotonView>().RPC("SetTypingState", RpcTarget.Others, false);
            }
        }
    }

    private GameObject GetLocalPlayer()
    {   
        Debug.Log($"Searching for local player: {PhotonNetwork.LocalPlayer.NickName}");
        if (photonView != null && photonView.gameObject.CompareTag("Player") && photonView.IsMine)
        {
            return photonView.gameObject;
        } 
    
        PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);
        foreach (PhotonView view in photonViews)
        {
            Debug.Log($"Checking view: Owner={view.Owner?.NickName}, IsMine={view.IsMine}, Tag={view.tag}");
            if (view.IsMine && view.CompareTag("Player") && 
            view.Owner != null && 
            view.Owner.NickName == PhotonNetwork.LocalPlayer.NickName &&
            view.GetComponent<PlayerMovement>() != null)
            {
                return view.gameObject;
            }
        }
        
        Debug.LogWarning("Local player not found");
        return null;
    }

    private void SendChatMessage()
    {
        if (chatInput == null || string.IsNullOrEmpty(chatInput.text)) return;

        string message = chatInput.text;
        string sender = PhotonNetwork.LocalPlayer.NickName;
        
        // Send to all players
        photonView.RPC(nameof(ReceiveMessage), RpcTarget.All, sender, message);
        AddMessageToLog(sender, message); // Show locally immediately
        
        CloseChat();
    }

    [PunRPC]
    public void ReceiveMessage(string sender, string message)
    {
        // Debug log to verify reception
        Debug.Log($"Received message from {sender}: {message}");

        // Add if message is not our own duplicate
        if (!(sender == PhotonNetwork.LocalPlayer.NickName)) AddMessageToLog(sender, message);
    
        // Show bubble only for sender's local view
        if (sender == PhotonNetwork.LocalPlayer.NickName)
        {
            ShowMessageBubble(sender, message);
        }
    }


    private void AddMessageToLog(string sender, string message)
    {
        string formattedMessage = $"{sender}: {message}\n";
    
        // Updated to use non-deprecated FindObjectsByType
        ChatManager[] allChatManagers = FindObjectsByType<ChatManager>(FindObjectsSortMode.None);
    
        foreach (ChatManager chatManager in allChatManagers)
        {
            if (chatManager.chatLogText != null)
            {
                // Use StartCoroutine directly since Photon RPCs are already on main thread
                chatManager.StartCoroutine(chatManager.UpdateChatLog(formattedMessage));
            }
        }
    }

    private IEnumerator UpdateChatLog(string formattedMessage)
{
    // Split the existing text into lines
    string[] lines = chatLogText.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    
    // Keep max 14 lines (excluding empty lines)
    const int maxLines = 14;
    if (lines.Length >= maxLines)
    {
        // Remove the oldest line (index 0) while preserving the empty line if it exists
        List<string> lineList = new List<string>(lines);
        lineList.RemoveAt(1);
        chatLogText.text = string.Join("\n", lineList) + "\n" + formattedMessage;
    }
    else
    {
        // Just append normally if under limit
        chatLogText.text += formattedMessage;
    }
    
    Debug.Log($"Message added to {photonView.Owner?.NickName}'s chat (Lines: {lines.Length})");
    
    // Scroll to bottom
    yield return new WaitForEndOfFrame();
    if (chatLogPanel != null)
    {
        var scrollRect = chatLogPanel.GetComponentInParent<ScrollRect>();
        if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0;
    }
}

    // Modified to work without instance references
    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        if (this.chatLogPanel != null)
        {
            var scrollRect = this.chatLogPanel.GetComponentInParent<ScrollRect>();
            if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0;
        }
    }

    private void ShowMessageBubble(string sender, string message)
    {
        if (messageBubblePrefab == null) return;

        GameObject targetPlayer = FindPlayerByName(sender);
        if (targetPlayer == null) return;

        // Find existing bubble
        Transform bubbleTransform = targetPlayer.transform.Find("MessageBubble(Clone)");
        GameObject bubble;
    
        if (bubbleTransform != null)
        {
            bubble = bubbleTransform.gameObject;
        }
        else
        {
            bubble = Instantiate(messageBubblePrefab, targetPlayer.transform);
            bubble.name = "MessageBubble(Clone)";
        }
    
        // Set message text
        TMP_Text bubbleText = bubble.GetComponentInChildren<TMP_Text>();
        if (bubbleText != null) bubbleText.text = message;
    
        bubble.SetActive(true);
        StartCoroutine(HideBubbleAfterDelay(bubble, messageBubbleDuration));
    }

    private IEnumerator HideBubbleAfterDelay(GameObject bubble, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bubble != null) bubble.SetActive(false);
    }

    private GameObject FindPlayerByName(string name)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == name && player.TagObject is GameObject playerObj)
            {
                return playerObj;
            }
        }
        return null;
    }
}