using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootChestScript : MonoBehaviour
{

    public GameObject PromptUI;
    public GameObject LootContainerUI;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("Collided with a player");
        PromptUI.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            LootContainerUI.SetActive(true);
        }

    }
}
