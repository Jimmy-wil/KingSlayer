using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject panel;
    public GameObject mainMenuGui;
    public GameObject mainMenu;

    private bool open = false;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (!mainMenuGui.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("état présidence");
            open = !open;
            panel.SetActive(open);
        }
    }

    public void LeaveGame()
    {
        NetworkManager.Singleton.Shutdown();
        // SceneManager.UnloadSceneAsync("Game");

        mainMenuGui.SetActive(true);
        
        open = !open;
        panel.SetActive(open);
    }
}
