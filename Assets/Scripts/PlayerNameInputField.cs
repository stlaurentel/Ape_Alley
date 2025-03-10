using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

using Photon.Realtime;

using System.Collections;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    const string playerNamePrefKey = "MyName";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player name null/empty");
            return;
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
