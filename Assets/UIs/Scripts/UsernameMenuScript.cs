using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class UsernameMenuScript : MonoBehaviour
{
    [SerializeField]
    private UserDataScript UserData;

    public TMP_InputField _usernameInput;
    public GameObject CurrentMenu;
    public GameObject ErrorPanel;
    public TMP_Text ErrorMessage;
    public GameObject NextMenu;

    public TMP_Text DisplayUsername;

    public void Start()
    {
        _usernameInput.text = "Player" + Random.Range(1, 99999999).ToString();
        UserData.Username = _usernameInput.text;
    }
    public void TryConfirm()
    {
        string tag = _usernameInput.text.Trim(' ');
        if (tag.IsNullOrEmpty())
        {
            ErrorMessage.text = "Invalid Username!";
            ErrorPanel.SetActive(true);
        }
        else
        {
            DisplayUsername.text = _usernameInput.text;

            UserData.Username = _usernameInput.text;
            
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
            DisplayUsername.gameObject.SetActive(true);
        }


    }
}
