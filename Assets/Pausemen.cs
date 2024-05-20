using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemen : MonoBehaviour
{
   [SerializeField] GameObject pausemenu;

   public void Pause()
   {
    pausemenu.SetActive(true);
   }

   public void home()
   {
    SceneManager.LoadScene("MainMenu");
   }

   public void resume()
   {
    pausemenu.SetActive(false);
   }
}
