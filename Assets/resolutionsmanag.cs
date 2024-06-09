using System.Collections;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class resolutionsmanag : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutiondropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredresolution;

    private float currentrefreshrate;
    private int currentresolutionindex = 0;

    void Start()
    {
        resolutions = Screen.resolutions;
        filteredresolution = new List<Resolution>();

        resolutiondropdown.ClearOptions();
        // Utilisation de refreshRateRatio pour obtenir la fréquence de rafraîchissement actuelle
        currentrefreshrate = Screen.currentResolution.refreshRateRatio.numerator / (float)Screen.currentResolution.refreshRateRatio.denominator;

        Debug.Log("Current Refresh Rate: " + currentrefreshrate);
        for (int i = 0; i < resolutions.Length; i++)
        {
            float refreshRate = resolutions[i].refreshRateRatio.numerator / (float)resolutions[i].refreshRateRatio.denominator;
            Debug.Log("Resolution: " + resolutions[i].width + "x" + resolutions[i].height + " @ " + refreshRate + "Hz");

            // Utilisation de refreshRateRatio pour filtrer les résolutions
            if (Mathf.Approximately(refreshRate, currentrefreshrate))
            {
                filteredresolution.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredresolution.Count; i++)
        {
            float refreshRate = filteredresolution[i].refreshRateRatio.numerator / (float)filteredresolution[i].refreshRateRatio.denominator;
            string resolutionoption = filteredresolution[i].width + "x" + filteredresolution[i].height + " " + refreshRate + " Hz";
            options.Add(resolutionoption);

            if (filteredresolution[i].width == Screen.width && filteredresolution[i].height == Screen.height)
            {
                currentresolutionindex = i;
            }
        }

        resolutiondropdown.AddOptions(options);
        resolutiondropdown.value = currentresolutionindex;
        resolutiondropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionindex)
    {
        Resolution resolution = filteredresolution[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
} 