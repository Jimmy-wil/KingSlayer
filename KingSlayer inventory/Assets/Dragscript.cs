using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragscript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   
[Header("UI")]
public Image image;

public Text countText;

   [HideInInspector]  public Itemscript item;
   [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    private void Start()
    {
        InitialiseItem(item);
    }
    
    


    public void InitialiseItem(Itemscript newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textactiv = count > 1;
        countText.gameObject.SetActive(textactiv);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("is dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}

