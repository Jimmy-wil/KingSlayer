using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Loadingscene : MonoBehaviour
{
   public GameObject loadingscreen;
   public Slider loadingbar;
 public void loadscene(int levelindex)
 {
    StartCoroutine(loadsceneasynccro(levelindex));
 }

 IEnumerator loadsceneasynccro(int levelindex)
 {
    AsyncOperation operation = SceneManager.LoadSceneAsync(levelindex);
    loadingscreen.SetActive(true);
    while (!operation.isDone)
    {
        loadingbar.value = operation.progress;
        yield return null;
    }
 }

}