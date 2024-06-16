using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBar1 : MonoBehaviour
{
    [SerializeField] 
    private UnityEngine.UI.Image _healthBarForegroundImage;

    [SerializeField]
    private InventoryController inventoryController;

    private GameObject player;

    private Health health;

    void Start()
    {
        player = GameObject.Find(inventoryController.UserData.Username);
        if (player == null)
        {
            Debug.LogWarning("Player Not found");
            return;
        }
        
        health =player.GetComponent<Health>();
        player=null;
    }
    public void UpdateHealthBar(Health health)
    {
       // _healthBarForegroundImage.fillAmount=health.GetHealthPercent();
    }
     private void Update()
    {
        player = GameObject.Find(inventoryController.UserData.Username);
        if (player == null)
        {
            Debug.LogWarning("Player Not found");
            return;
        }
        health =player.GetComponent<Health>();
        _healthBarForegroundImage.fillAmount=health.GetHealthPercent();
        player=null;

    }
}
