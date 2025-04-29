//using Photon.Pun;
//using UnityEngine;

//public class CustomizePlayer : MonoBehaviour
//{
//    public PlayerInventory inventory;

//    private Animator eyePatchAnimator;
//    private Animator clownHatAnimator;
//    private PhotonView photonView;

//    private bool eyePatchActive = false;
//    private bool clownHatActive = false;

//    private const string PROP_EYEPATCH = "EyepatchOn";
//    private const string PROP_CLOWNHAT = "ClownHatOn";


//    void Start()
//    {
//        photonView = GetComponent<PhotonView>();

//        eyePatchAnimator = transform.Find("EyewearSlot").GetComponent<Animator>();
//        clownHatAnimator = transform.Find("ClownHatSlot").GetComponent<Animator>();

//    }

//    public void ToggleEyepatch()
//    {
//        // Only allow the local player to toggle
//        if (photonView.IsMine)
//        {
//            eyePatchActive = !eyePatchActive;
//            photonView.RPC("RPC_ToggleEyepatch", RpcTarget.AllBuffered, eyePatchActive);
//        }
//    }

//    [PunRPC]
//    void RPC_ToggleEyepatch(bool isActive)
//    {
//        eyePatchAnimator.gameObject.SetActive(isActive);
//    }

//    public void ToggleClownHat()
//    {
//        // Only allow the local player to toggle
//        if (photonView.IsMine)
//        {
//            clownHatActive = !clownHatActive;
//            photonView.RPC("RPC_ToggleClownHat", RpcTarget.AllBuffered, clownHatActive);
//        }
//    }

//    [PunRPC]
//    void RPC_ToggleClownHat(bool isActive)
//    {
//        clownHatAnimator.gameObject.SetActive(isActive);
//    }
//}

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;

public class CustomizePlayer : MonoBehaviour, IInRoomCallbacks
{
    public PlayerInventory inventory;

    private Animator eyePatchAnimator;
    private Animator clownHatAnimator;
    private PhotonView photonView;

    private const string PROP_EYEPATCH = "EyepatchOn";
    private const string PROP_CLOWNHAT = "ClownHatOn";

    public GameObject inventoryClownHat;
    public GameObject inventoryEyepatch;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        eyePatchAnimator = transform.Find("EyewearSlot").GetComponent<Animator>();
        clownHatAnimator = transform.Find("ClownHatSlot").GetComponent<Animator>();

        // Apply the current appearance from the owner's custom properties
        ApplyCurrentAppearance();
    }

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void ToggleEyepatch()
    {
        if (!photonView.IsMine) return;

        inventoryEyepatch.SetActive(!inventoryEyepatch.activeInHierarchy);

        bool newState = !GetPropertyState(PROP_EYEPATCH);
        Hashtable props = new Hashtable { { PROP_EYEPATCH, newState } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void ToggleClownHat()
    {
        if (!photonView.IsMine) return;

        inventoryClownHat.SetActive(!inventoryClownHat.activeInHierarchy);

        bool newState = !GetPropertyState(PROP_CLOWNHAT);
        Hashtable props = new Hashtable { { PROP_CLOWNHAT, newState } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    private bool GetPropertyState(string key)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(key, out object value))
            return (bool)value;
        return false;
    }

    private void ApplyCurrentAppearance()
    {
        if (photonView.Owner.CustomProperties.TryGetValue(PROP_EYEPATCH, out object ep))
        {
            SetEyepatchVisual((bool)ep);
        }
        else
        {
            SetEyepatchVisual(false);
        }

        if (photonView.Owner.CustomProperties.TryGetValue(PROP_CLOWNHAT, out object ch))
        {
            SetClownHatVisual((bool)ch);
        }
        else
        {
            SetClownHatVisual(false);
        }
    }

    private void SetEyepatchVisual(bool isActive)
    {
        eyePatchAnimator.gameObject.SetActive(isActive);
    }

    private void SetClownHatVisual(bool isActive)
    {
        clownHatAnimator.gameObject.SetActive(isActive);
    }

    // Called when any player's properties change
    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer != photonView.Owner) return;

        if (changedProps.ContainsKey(PROP_EYEPATCH))
        {
            SetEyepatchVisual((bool)changedProps[PROP_EYEPATCH]);
        }

        if (changedProps.ContainsKey(PROP_CLOWNHAT))
        {
            SetClownHatVisual((bool)changedProps[PROP_CLOWNHAT]);
        }
    }

    // Unused IInRoomCallbacks (required to implement but can remain empty)
    public void OnPlayerEnteredRoom(Player newPlayer) { }
    public void OnPlayerLeftRoom(Player otherPlayer) { }
    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }
    public void OnMasterClientSwitched(Player newMasterClient) { }
}
