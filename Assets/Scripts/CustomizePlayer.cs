using Photon.Pun;
using UnityEngine;

public class CustomizePlayer : MonoBehaviour
{
    private Animator eyePatchAnimator;
    private PhotonView photonView;

    private bool eyePatchActive = false;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        eyePatchAnimator = transform.Find("EyewearSlot").GetComponent<Animator>();
    }

    public void ToggleEyepatch()
    {
        // Only allow the local player to toggle
        if (photonView.IsMine)
        {
            eyePatchActive = !eyePatchActive;
            photonView.RPC("RPC_ToggleEyepatch", RpcTarget.All, eyePatchActive);
        }
    }

    [PunRPC]
    void RPC_ToggleEyepatch(bool isActive)
    {
        eyePatchAnimator.gameObject.SetActive(isActive);
    }
}