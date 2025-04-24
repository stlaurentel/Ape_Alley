using UnityEngine;
using Photon.Pun;

public class BreakoutInteractionText : MonoBehaviour
{
    public GameObject interactionUI; 
    private bool touchingSpace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        touchingSpace = false;
        if (!GetComponentInParent<PhotonView>().IsMine)
        {
            if (interactionUI != null) interactionUI.SetActive(false);
            enabled = false; // Disable this script entirely for non-local players
            return;
        }

        if (interactionUI != null) {
            interactionUI.SetActive(false);
        }
    }

    public void SetTouchingSpace(bool isTouching)
    {
        touchingSpace = isTouching;
        if (interactionUI != null) {
            interactionUI.SetActive(isTouching);
        }
    }
}
