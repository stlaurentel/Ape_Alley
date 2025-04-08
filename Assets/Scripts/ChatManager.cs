using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public GameObject chatCanvasPrefab;
    public GameObject messageBubblePrefab; 
    
    private TMP_InputField chatInput;
    private TMP_Text chatLogText;
    private GameObject chatPrompt;
    private GameObject chatLogPanel;
    
    private GameObject chatInstance;
    private PhotonView photonView;
    private bool isChatOpen = false;

    private void Awake() 
    {
        photonView = GetComponent<PhotonView>();
        chatInstance = Instantiate(chatCanvasPrefab, transform);
        InitializeChatReferences();

        if (!photonView.IsMine) {
            chatInput.interactable = false;
            chatPrompt.SetActive(false);
        }
    }
    
    

    private void InitializeChatReferences()
    {
        // Get references from the instantiated canvas
        chatInput = chatInstance.transform.Find("InputPromptPanel/ChatInput").GetComponent<TMP_InputField>();
        chatLogPanel = chatInstance.transform.Find("ChatLogPanel").gameObject;
        chatLogText = chatLogPanel.transform.Find("ChatLogText").GetComponent<TMP_Text>();
        chatPrompt = chatInstance.transform.Find("InputPromptPanel/ChatPrompt").gameObject;
        
        chatInput.gameObject.SetActive(false);
        chatPrompt.SetActive(photonView.IsMine);
        //chatLogPanel.SetActive(true);
    }


    private void Update()
    {
        if (!photonView.IsMine) return;
        if ((Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Question)) && !isChatOpen)
        {
            OpenChat();
        }
        
        if (isChatOpen && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput.text))
        {
            SendChatMessage();
        }
        
        if (isChatOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChat();
        }
    }

    private void OpenChat()
    {
        isChatOpen = true;
        chatInput.gameObject.SetActive(true);
        chatPrompt.SetActive(false);
        StartCoroutine(ForceInputFieldFocus());
    }
    
    private IEnumerator ForceInputFieldFocus()
    {
        yield return null; // Wait one frame
        chatInput.Select();
        chatInput.ActivateInputField();
    }

    private void CloseChat()
    {
        isChatOpen = false;
        chatInput.gameObject.SetActive(false);
        chatPrompt.SetActive(true);
        chatInput.text = "";
    }

    private void SendChatMessage()
    {
        string message = chatInput.text;
        string sender = PhotonNetwork.LocalPlayer.NickName;
        
        if (photonView != null)
        {
            photonView.RPC("ReceiveMessage", RpcTarget.All, sender, message);
            AddMessageToLog(sender, message); // Show locally immediately
        }
        
        CloseChat();
    }

    [PunRPC]
    public void ReceiveMessage(string sender, string message)
    {
        // Only update UI for local player
        AddMessageToLog(sender, message);
        
        if (sender == PhotonNetwork.LocalPlayer.NickName) {
            ShowMessageBubble(sender, message);
        }
    }

    private void AddMessageToLog(string sender, string message)
    {
        string formattedMessage = $"{sender}: {message}\n";
        chatLogText.text += formattedMessage;
        
        Canvas.ForceUpdateCanvases();
        var scrollRect = chatLogPanel.GetComponentInParent<ScrollRect>();
        if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0f;
    }

    [PunRPC]
    private void SyncChatLog(string sender, string message)
    {
        string formattedMessage = $"{sender}: {message}\n";
        chatLogText.text += formattedMessage;
        
        Canvas.ForceUpdateCanvases();
        var scrollRect = chatLogPanel.GetComponentInParent<ScrollRect>();
        if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0f;
    }



    private void ShowMessageBubble(string sender, string message)
    {
        // Find the player who sent this message
        GameObject targetPlayer = FindPlayerByName(sender);
        if (targetPlayer == null || messageBubblePrefab == null) return;

        // Create or find existing bubble
        Transform bubble = targetPlayer.transform.Find("MessageBubble");
        if (bubble == null)
        {
            bubble = Instantiate(messageBubblePrefab, targetPlayer.transform).transform;
            bubble.name = "MessageBubble";
        }
        
        // Set message text
        TMP_Text bubbleText = bubble.GetComponentInChildren<TMP_Text>();
        if (bubbleText != null) bubbleText.text = message;
        
        bubble.gameObject.SetActive(true);
        StartCoroutine(HideBubbleAfterDelay(bubble.gameObject, 3f));
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
            if (player.NickName == name)
            {
                if (player.TagObject is GameObject playerObj)
                {
                    return playerObj;
                }
            }
        }
        return null;
    }
}