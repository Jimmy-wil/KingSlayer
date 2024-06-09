using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeathMenuScript : MonoBehaviour
{
    public GameObject DeathMenu;
    public GameObject Players;
    
    [SerializeField]
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Transform target;

    private int playerIndex = 0;
    public List<GameObject> PlayerList{
        get
        {
            List<GameObject> res = new List<GameObject>();
            foreach (GameObject gameObject in Players.transform)
            {
                res.Add(gameObject);
            }
            return res;
        }
    }

    void Start()
    {
        DeathMenu.SetActive(false);
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
        DeathMenu.SetActive(true);

    }

    public void SpectateNextPlayer()
    {   
        playerIndex = (playerIndex + 1) % PlayerList.Count;
        target = PlayerList[playerIndex].transform;
    }

    public void SpectatePrevPlayer()
    {   
        playerIndex = (playerIndex - 1) % PlayerList.Count;
        target = PlayerList[playerIndex].transform;
    
    }

    public void LeaveGame()
    {
        target = null;
        NetworkManager.Singleton.Shutdown();
        // SceneManager.UnloadSceneAsync("Game");

        DeathMenu.SetActive(false);
        
    }
}