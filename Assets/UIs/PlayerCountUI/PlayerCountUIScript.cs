using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerCountUIScript : MonoBehaviour
{
    [SerializeField]
    private WinUIManagerScript winUIManagerScript;
    [SerializeField]
    private GameObject Players;
    [SerializeField]
    public GameObject WinPanel;
    [SerializeField]
    private TMP_Text WinnerText;

    public bool winner = false;

    [SerializeField]
    private TMP_Text text;

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
            winner = true;

            Debug.Log("We got a winner");

            // winUIManagerScript.DisplayWinner(Players.transform.GetChild(0).gameObject.name);



        }
        else
        {
            text.text = $"Players left: {Players.transform.childCount}";
        }
    }

}
