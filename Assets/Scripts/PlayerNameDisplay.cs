using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI nameText;

    void Awake()
    {
        // Find TextMeshPro if not assigned
        if (nameText == null)
        {
            nameText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        // Ensure we have a valid PhotonView
        if (photonView != null && nameText != null)
        {
            // Set the name based on the specific player's PhotonView
            string playerName = photonView.Owner != null
                ? photonView.Owner.NickName
                : "Unknown Player";

            // Set the name text
            nameText.text = playerName;
        }
    }

    void Update()
    {
        // Optional: Rotate name to face forward direction of the player
        if (nameText != null)
        {
            nameText.transform.rotation = transform.rotation;
        }
    }
}