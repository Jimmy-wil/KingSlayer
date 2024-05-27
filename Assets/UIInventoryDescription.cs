using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    [SerializeField] private TMP_Text description;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.title.text = "";
        this.description.text = "";
    }

    public void SetDescription(string itemName, string itemDescription)
    {
        this.title.text = itemName;
        this.description.text = itemDescription;
    }
}
