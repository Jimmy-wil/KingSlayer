using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemDropHandlerScript : NetworkBehaviour
{
    private Item ItemToDrop;

    public void DropItem(Item itemToDrop, Vector2 position)
    {
        ItemToDrop = itemToDrop;
        DropItemServerRpc(position);

    }

    [ServerRpc]
    private void DropItemServerRpc(Vector2 position)
    {
        var clone = Instantiate(ItemToDrop.gameObject, position, Quaternion.identity);
        clone.GetComponent<NetworkObject>().Spawn();
    }

}
