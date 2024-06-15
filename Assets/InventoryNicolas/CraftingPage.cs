using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPage : MonoBehaviour
{
    [SerializeField] private RectTransform recipeContent;
    [SerializeField] private RectTransform resultContent ;
    
    [SerializeField] public UIInventoryItem itemPrefab;
    [SerializeField] private MouseFollower mousefollower;
    [SerializeField] private UIInventoryDescription itemDescription;
    
    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    private int currentlyDraggedItemIndex = -1;
    
    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

    public event Action<int, int> OnSwapItems;

    public void Start()
    {
        this.gameObject.SetActive(true);
    }


    public void InitializeCraftingUI()
    {
        for (int i = 0; i < 2 ; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(recipeContent);
            listOfUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBegindrag += HandleBeginDrag;
            uiItem.OnItemdroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBoutonClick += HandleShowItemActions;
        }

        for (int i = 0; i < 1 ; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(resultContent);
            listOfUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBegindrag += HandleBeginDrag;
            uiItem.OnItemdroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBoutonClick += HandleShowItemActions;
        }
    }

   
    

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage,itemQuantity);
        }
    }

    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        
       OnItemActionRequested?.Invoke(index);
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
       ResetDraggedItem();
    }

    private void ResetDraggedItem()
    {
        mousefollower.Toggle(false);
        currentlyDraggedItemIndex = -1; 
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex,index);
        HandleItemSelection(inventoryItemUI);
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if(index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mousefollower.Toggle(true);
        mousefollower.SetData(sprite,quantity);
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1) 
            return;
        OnDescriptionRequested?.Invoke(index);
    }

    private void Awake()
    {
       
        itemDescription.ResetDescription();
        mousefollower.Toggle(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        ResetSelection();
    }

    public void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listOfUIItems)
        {
            item.Deselect();
        }
    }
    
 

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
      itemDescription.SetDescription(name, description);
      DeselectAllItems();
      listOfUIItems[itemIndex].Select();
    }

    public void ResetAllItems()
    {
        foreach (var  item  in listOfUIItems)
        {
            item.ResetData();
            item.Deselect();
        }
    }
}
