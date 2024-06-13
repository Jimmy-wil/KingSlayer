using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    // Start is called before the first frame update
    [SerializeField] public Image itemImage;
    [SerializeField] public TMP_Text quantityTxt;
    [SerializeField] private Image borderImage;

    public event Action<UIInventoryItem> OnItemClicked, OnItemdroppedOn, OnItemBegindrag, OnItemEndDrag, OnRightMouseBoutonClick,
        OnLeftMouseBoutonClick;

    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
      this.itemImage.gameObject.SetActive(false);
      empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }
    

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBoutonClick?.Invoke(this);
        }

        if (pointerData.button == PointerEventData.InputButton.Left)
        {
          OnLeftMouseBoutonClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty) return;
        OnItemBegindrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemdroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
