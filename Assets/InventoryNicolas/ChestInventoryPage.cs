using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventoryPage : MonoBehaviour
{
    [SerializeField] public UIInventoryItem itemPrefab;

    [SerializeField] private RectTransform contentPanel;

   //changed form private to public
    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnItemActionRequested;


    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize ; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            
            listOfUIItems.Add(uiItem);
            
            // uiItem.OnItemClicked += HandleItemSelection;
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
    

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1) 
            return;

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


    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listOfUIItems)
        {
            item.Deselect();
        }
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
