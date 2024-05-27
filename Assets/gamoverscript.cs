using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverPanel;
    private Health playerHealth;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Ensure the panel is initially hidden
        playerHealth = FindObjectOfType<Health>(); // Find the player's Health component
    }

    public void ShowGameOverMenu()
    {
        gameOverPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        // Assuming your main menu is the first scene (index 0)
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StayInGame()
    {
        gameOverPanel.SetActive(false);
        playerHealth.StayAsPhantom(); // Activate phantom mode
    }
}