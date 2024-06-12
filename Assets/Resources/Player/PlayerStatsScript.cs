using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatsScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject PlayersGameObject;
    [SerializeField]
    private UserDataScript UserData;

    private readonly NetworkVariable<FixedString32Bytes> _playerName = new();

    [SerializeField]
    private Health Health;

    [SerializeField]
    private TMP_Text text;
    private int currentHealth => Health.currentHealth;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _playerName.OnValueChanged += OnUsernameChanged;

    }
    private void OnUsernameChanged(FixedString32Bytes _, FixedString32Bytes newUsername)
    {
        try
        {
            text.text = newUsername.ToString();
            this.gameObject.name = newUsername.ToString();
        }
        catch { }
    }

    private void Start()
    {
        PlayersGameObject = GameObject.Find("Players");    
        this.transform.SetParent(PlayersGameObject.transform);

        if (IsOwner)
        {
            UserData = GameObject.Find("UserData").GetComponent<UserDataScript>();
            UpdateUsernameServerRpc(UserData.Username);

        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            _playerName.Value = _playerName.Value + " ";
            _playerName.Value = _playerName.Value;
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void UpdateUsernameServerRpc(string newUsername)
    {
        Debug.Log(newUsername);
        _playerName.Value = newUsername;

    }


}
