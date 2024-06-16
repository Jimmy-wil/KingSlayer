using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class WinUIManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private TMP_Text WinnerText;

    public void DisplayWinner(string s)
    {
        winPanel.SetActive(true);
        DisplayWinnerServerRpc(s);
    }
    [ServerRpc(RequireOwnership=false)]
    private void DisplayWinnerServerRpc(string s)
    {
        winPanel.SetActive(true);
        WinnerText.text = s;
        DisplayWinnerClientRpc(s);
    }
    [ClientRpc]
    private void DisplayWinnerClientRpc(string s)
    {
        Debug.Log("For the win!");
        winPanel.SetActive(true);
        WinnerText.text = s;
    }
}
