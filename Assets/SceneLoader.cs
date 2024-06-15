using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class SceneLoader : MonoBehaviour
{
    public GameObject loadingscreen;
    public Slider loadingbar;
    public void LoadScene(int levelindex)
    {
        StartCoroutine(LoadScene2(levelindex));
    }

    IEnumerator LoadScene2(int levelindex)
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
