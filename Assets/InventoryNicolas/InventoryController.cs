using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInvetoryPage inventoryUI;
    private bool toggle;

    public int inventorySize = 10;
    void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) == true)
        {
            toggle = !toggle;
            inventoryUI.gameObject.SetActive(toggle);
        }
    }
}
