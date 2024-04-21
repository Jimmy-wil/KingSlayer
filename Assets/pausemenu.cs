using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
   [SerializeField] GameObject pauseMenu;

   public void pause()
   {
    pauseMenu.SetActive(true);
   }

   public void home()
   {
    SceneManager.LoadScene("MainMenu");
 }

 public void resume(){
    pauseMenu.SetActive(false);
 }


}
