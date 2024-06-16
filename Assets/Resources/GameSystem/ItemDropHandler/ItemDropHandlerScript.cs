using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemDropHandlerScript : NetworkBehaviour
{
    public List<Item> droplist;

    private Item ItemToDrop;
    public void DropItem(Item itemToDrop, Vector2 position)
    {
        droplist.Contains(itemToDrop);

        ItemToDrop = itemToDrop;
        DropItemServerRpc(position);

    }

    [ServerRpc(RequireOwnership=false)]
    private void DropItemServerRpc(Vector2 position)
    {
        var clone = Instantiate(ItemToDrop.gameObject, position, Quaternion.identity);
        clone.GetComponent<NetworkObject>().Spawn();

        ItemToDrop.GetComponent<Item>().InventoryItem = ItemToDrop.InventoryItem;

    }

}
