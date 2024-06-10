using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCountUIScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Players;

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
        else
        {
            text.text = $"Players left: {Players.transform.childCount}";
        }
    }
}
