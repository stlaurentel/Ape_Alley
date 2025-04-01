using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    // Keep track of the local player instance
    private GameObject localPlayerInstance;

    // Custom property keys
    private const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";

    void Awake()
    {
        // Make sure this GameObject persists across scene loads
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Missing player prefab");
            return;
        }

        // Check if in a room
        if (PhotonNetwork.InRoom)
        {
            // If the local player instance hasn't been cached yet
            if (localPlayerInstance == null)
            {
                CreatePlayerInstance();
            }
        }
    }

    private void CreatePlayerInstance()
    {
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f);

        Debug.LogFormat("Creating player {0} at position {1}", PhotonNetwork.NickName, spawnPosition);

        // Instantiate the player
        localPlayerInstance = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPosition, Quaternion.identity, 0);

        // Set a custom property to indicate player is loaded and ready
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            {PLAYER_LOADED_LEVEL, true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnLeftRoom()
    {
        // Reset local player instance when leaving room
        localPlayerInstance = null;
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // Called when joining a room
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);

        // If local player instance has not yet been created, create it
        if (localPlayerInstance == null)
        {
            CreatePlayerInstance();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // If the player loaded level property changed for any player
        if (changedProps.ContainsKey(PLAYER_LOADED_LEVEL))
        {
            // Log that a player is ready
            Debug.Log($"Player {targetPlayer.NickName} is ready!");

            // If all players are now ready, we can trigger any "all players ready" logic here
            if (CheckAllPlayersReady())
            {
                Debug.Log("All players are ready!");
            }
        }
    }

    // Check if all players are ready
    private bool CheckAllPlayersReady()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerReady;
            if (p.CustomProperties.TryGetValue(PLAYER_LOADED_LEVEL, out playerReady))
            {
                if (!(bool)playerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    // Called when someone else joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

        // Let Photon clean up automatically
    }
}