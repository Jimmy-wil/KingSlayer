using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerCountUIScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject Players;
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private TMP_Text WinnerText;

    public bool winner = false;

    [SerializeField]
    private TMP_Text text;

    private void Start()
    {
        NetworkManager.OnClientStopped += OnClientDisconnected;
    }

    private void OnClientDisconnected(bool obj)
    {
        WinPanel.SetActive(false);
        winner = false;
    }

    public void Update()
    {
        UpdatePlayerCount();

    }

    private void UpdatePlayerCount()
    {
        if(Players.transform.childCount == 0)
        {
            text.text = $"No players left!";
        }
        else if(Players.transform.childCount == 1)
        {
            text.text = $"Players left: {Players.transform.childCount}";

            if (winner) return;
            Debug.Log("We got a winner");
            winner = true;

            DoSomethingServerRpc();

            
        }
        else
        {
            text.text = $"Players left: {Players.transform.childCount}";
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void DoSomethingServerRpc()
    {
        DoSomethingClientRpc();
    }
    [ClientRpc]
    private void DoSomethingClientRpc() 
    {
        Debug.Log("Do we have a winner?");
        winner = true;

        WinPanel.SetActive(true);
        
        WinnerText.text = $"{Players.transform.GetChild(0).name}";


    }

}
