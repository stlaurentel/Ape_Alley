using UnityEngine;
using Photon.Pun;

public class PlayerCamera : MonoBehaviourPun
{
    public Camera playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        
        if (photonView.IsMine) { // Check if this PhotonView is owned by the local player
            playerCamera = Camera.main;
            AttachCamera();
        }
        else {
            if (playerCamera != null) {
                playerCamera.enabled = false;
            }
        }
    }

    void AttachCamera() {
        if (playerCamera != null) {
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0, 0.5f, -3);
            playerCamera.transform.localRotation = Quaternion.identity;
        }
        else {
            Debug.LogError("PlayerCamera: No camera assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
