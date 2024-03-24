using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sliderload : MonoBehaviour
{
  [Header("Menu Scenes")]
  [SerializeField] private GameObject loadingscreen;
  [SerializeField] private GameObject mainMenu;

  [Header("slider")]
  [SerializeField] private Slider loadingslider;

  public void LoadLevelBtn(string leveltoload)
  {
    mainMenu.SetActive(false);
    loadingscreen.SetActive(true);
    StartCoroutine(Loadlevelasync(leveltoload));

  }

  IEnumerator Loadlevelasync(string leveltoload)
  {
    AsyncOperation loadoperation = SceneManager.LoadSceneAsync(leveltoload);

    while(!loadoperation.isDone)
    {
        float progressivevalue = Mathf.Clamp01(loadoperation.progress / 0.9f);
        loadingslider.value = progressivevalue;
        yield return null;
    }
  }
}
