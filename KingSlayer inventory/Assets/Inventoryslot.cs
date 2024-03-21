using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventoryslot : MonoBehaviour, IDropHandler
{
    

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Dragscript draggableitem = dropped.GetComponent<Dragscript>();
        draggableitem.parentAfterDrag = transform;
    }
}
