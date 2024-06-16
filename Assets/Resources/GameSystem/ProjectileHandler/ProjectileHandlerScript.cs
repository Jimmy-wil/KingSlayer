using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileHandlerScript : MonoBehaviour
{
    public Dictionary<int, GameObject> dictionary;
    public List<int> projectileList;

    private void Start()
    {
        
    }

    /*
    [ServerRpc(RequireOwnership = false)]
    public void ShootProjectileServerRpc(bool isServer, ServerRpcParams serverRpcParams = default)
    {
        
        .GetComponent<NetworkObject>().Spawn();

        if (!isServer)
        {
            Debug.Log("New owner!");
            tempProjectile.GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);

        }

    }

    */

}
