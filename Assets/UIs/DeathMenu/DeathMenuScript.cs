using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class DeathMenuScript : MonoBehaviour
{
    public GameObject Group;
    public GameObject Players;

    public TMP_Text SpectateeText;

    [SerializeField]
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Transform target;

    private int playerIndex = 0;

    public List<GameObject> PlayerList
    {
        get
        {
            List<GameObject> res = new List<GameObject>();
            for (int i = 0; i < Players.transform.childCount; i++)
            {
                res.Add(Players.transform.GetChild(i).gameObject);
            }
            return res;
        }

    }

    void Start()
    {
        Group.SetActive(false);
        mainCamera = Camera.main;
    }
    
    void FixedUpdate()
    {   
        if (!target) return;

        Vector3 targetPosition = target.position + offset;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);

    }

    public void ShowDeathMenu()
    {
        Debug.Log("Showing Death Menu");
        Group.SetActive(true);

    }

    public void SpectateNextPlayer()
    {   
        try
        {
            playerIndex = (playerIndex + 1) % PlayerList.Count;
            target = PlayerList[playerIndex].transform;

            SpectateeText.text = PlayerList[playerIndex].name;

        }
        catch
        {
            Debug.LogWarning("Failed to spectate next player");
        }
    }

    public void SpectatePrevPlayer()
    {
        try
        {
            playerIndex = (playerIndex - 1) % PlayerList.Count;
            target = PlayerList[playerIndex].transform;

            SpectateeText.text = PlayerList[playerIndex].name;
        }
        catch 
        {
            Debug.LogWarning("Failed to spectate prev player");
        }
    }

    public void LeaveGame()
    {
        target = null;
        NetworkManager.Singleton.Shutdown();
        // SceneManager.UnloadSceneAsync("Game");
        Group.SetActive(false);
        
    }

}
