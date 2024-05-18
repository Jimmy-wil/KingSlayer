using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInvetoryPage inventoryUI;
   // [SerializeField] private MouseFollower _mouseFollower;
    private bool inventoryIsClosed;

    public int inventorySize = 10;
    void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
        inventoryIsClosed = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryIsClosed)
            {
                inventoryUI.Show();
                inventoryIsClosed = false;
            }
            else
            {
                inventoryUI.Hide();
                inventoryIsClosed = true;
            }
            
           // toggle = !toggle;
           // inventoryUI.gameObject.SetActive(toggle);
           // _mouseFollower.gameObject.SetActive(toggle);
        }
    }
}
