using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    audioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
    }


    public void PlayGame()
    {
        audioManager.PlaySFX(audioManager.clip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }

    public void QuitGame()
    {
        audioManager.PlaySFX(audioManager.clip);
        Debug.Log("QUIT");
        Application.Quit();
    }

}
