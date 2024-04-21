using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemen : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

   public void pause()
   {
      PauseMenu.SetActive(true);
   }

   public void home()
   {
    SceneManager.LoadScene("MainMenu");
   }

   public void back()
   {
    PauseMenu.SetActive(false);
   }






}
