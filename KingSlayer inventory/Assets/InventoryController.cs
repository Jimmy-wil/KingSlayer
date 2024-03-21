using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public bool IsInventoryClosed;
    public GameObject Inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        IsInventoryClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {

            if (IsInventoryClosed)
            {
                Inventory.SetActive(true);
                IsInventoryClosed = false;
            }
            else
            {
                Inventory.SetActive(false);
                IsInventoryClosed = true;
            }
        }
    }
}
