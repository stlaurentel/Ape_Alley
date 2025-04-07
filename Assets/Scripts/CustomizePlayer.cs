using Photon.Pun;
using UnityEngine;

public class CustomizePlayer : MonoBehaviour
{
    public PlayerInventory inventory;

    private Animator eyePatchAnimator;
    private Animator clownHatAnimator;
    private PhotonView photonView;

    private bool eyePatchActive = false;
    private bool clownHatActive = false;


    void Start()
    {
        photonView = GetComponent<PhotonView>();

        eyePatchAnimator = transform.Find("EyewearSlot").GetComponent<Animator>();
        clownHatAnimator = transform.Find("ClownHatSlot").GetComponent<Animator>();

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

    public void ToggleClownHat()
    {
        // Only allow the local player to toggle
        if (photonView.IsMine)
        {
            clownHatActive = !clownHatActive;
            photonView.RPC("RPC_ToggleClownHat", RpcTarget.All, clownHatActive);
        }
    }

    [PunRPC]
    void RPC_ToggleClownHat(bool isActive)
    {
        clownHatAnimator.gameObject.SetActive(isActive);
    }
}