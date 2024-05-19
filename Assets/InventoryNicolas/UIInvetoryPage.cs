using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInvetoryPage : MonoBehaviour
{
   [SerializeField] private UIInventoryItem itemPrefab;

   [SerializeField] private RectTransform contentPanel;

   [SerializeField] private MouseFollower mousefollower;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    public Sprite image, image2;
    public int quantity;
    public string title;

    private int currentlyDraggedItemIndex = -1;

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBegindrag += HandleBeginDrag;
            uiItem.OnItemdroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBoutonClick += HandleShowItemActions;
        }
    }

    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
       
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        mousefollower.Toggle(false);
        //mousefollower.SetData(image, quantity);
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            mousefollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
            return;
        }
        listOfUIItems[currentlyDraggedItemIndex].SetData(index == 0 ? image : image2, quantity);
        listOfUIItems[index].SetData(currentlyDraggedItemIndex == 0 ? image : image2, quantity); 
        
        mousefollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if(index == -1)
            return;

        currentlyDraggedItemIndex = index;
        
        mousefollower.Toggle(true);
        mousefollower.SetData(index == 0 ? image : image2, quantity);
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
       listOfUIItems[0].Select();
    }

    private void Awake()
    {
        Hide();
        mousefollower.Toggle(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        listOfUIItems[0].SetData(image,quantity);
        listOfUIItems[1].SetData(image2,quantity);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
}
