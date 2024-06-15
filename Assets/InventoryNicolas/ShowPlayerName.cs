using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;

    [SerializeField] public UserDataScript userData;
    // Start is called before the first frame update
    void Start()
    {
        this.playerName.text = userData.Username;
    }

   
}
