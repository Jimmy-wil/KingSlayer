using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemDropHandlerScript : NetworkBehaviour
{
    public List<Item> droplist;

    private int index;
    public void DropItem(Item itemToDrop, Vector2 position)
    {
        for (int i = 0; i < droplist.Count; i++)
        {
            if(droplist[i].InventoryItem == itemToDrop.InventoryItem)
            {
                index = i;
            }
        }
        Debug.Log(droplist[index].ToString());
        DropItemServerRpc(position, index);

    }

    [ServerRpc(RequireOwnership=false)]
    private void DropItemServerRpc(Vector2 position, int i)
    {
        var clone = Instantiate(droplist[i].gameObject, position, Quaternion.identity);
        Debug.Log(droplist[i].InventoryItem);

        clone.GetComponent<NetworkObject>().Spawn();


    }

}
