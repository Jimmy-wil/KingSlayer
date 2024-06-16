using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnWeaponHandlerScript : NetworkBehaviour
{
    [SerializeField] private GameObjectDictionary GameObjectDictionary;


    public void SpawnWeapon(int key, NetworkObjectReference playerReference)
    { 
        if(IsServer)
        {
            if (!playerReference.TryGet(out NetworkObject networkObject))
            {
                Debug.LogWarning("Unable to get player!");
                return;
            }

            if (GameObjectDictionary.ItemDictionary.TryGetValue(key, out GameObject weaponToEquip))
            {
                GameObject clone = Instantiate(weaponToEquip);

                clone.GetComponent<NetworkObject>().Spawn();

                clone.transform.SetParent(networkObject.transform);

                clone.transform.position = networkObject.transform.position;

            }
            else
            {
                Debug.LogWarning("Unable to get weapon from the dictionary!");
            }
        }
        else
        {
            SpawnWeaponServerRpc(key, playerReference);

        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void SpawnWeaponServerRpc(int key, NetworkObjectReference playerReference, ServerRpcParams serverRpcParams = default)
    {
        if (!playerReference.TryGet(out NetworkObject networkObject))
        {
            Debug.LogWarning("Unable to get played!");
            return;
        }

        if (GameObjectDictionary.ItemDictionary.TryGetValue(key, out GameObject weaponToEquip))
        {
            GameObject clone = Instantiate(weaponToEquip);

            
            clone.GetComponent<NetworkObject>().Spawn();

            clone.transform.SetParent(networkObject.transform);

            clone.transform.position = networkObject.transform.position;

            clone.GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);
        }
        else
        {
            Debug.LogWarning("Unable to get weapon from the dictionary!");
        }
    }


    public void DestroyCurrentWeapon(NetworkObjectReference networkObjectReference)
    {
        if (IsServer)
        {
            if (networkObjectReference.TryGet(out NetworkObject networkObject))
            {
                if (networkObject == null)
                {
                    Debug.LogWarning("Player Not found");
                    return;
                }

                if (networkObject.transform.childCount > 3)
                {
                    GameObject ToDestroy = networkObject.transform.GetChild(3).gameObject;
                    if (ToDestroy != null)
                    {
                        Debug.Log($"Destroying {ToDestroy.name}...");
                        Destroy(ToDestroy);
                    }
                }
            }

            else
            {
                Debug.Log("No 4th gameobject in player, nothing to destroy");
            }
        }
        else
        {
            DestroyCurrentWeaponServerRpc(networkObjectReference);

        }
        
    }

    [ServerRpc(RequireOwnership=false)]
    private void DestroyCurrentWeaponServerRpc(NetworkObjectReference networkObjectReference)
    {

        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            if (networkObject == null)
            {
                Debug.LogWarning("Player Not found");
                return;
            }

            if (networkObject.transform.childCount > 3)
            {
                GameObject ToDestroy = networkObject.transform.GetChild(3).gameObject;
                if (ToDestroy != null)
                {
                    Debug.Log($"Destroying {ToDestroy.name}...");
                    Destroy(ToDestroy);
                }
            }
        }

        else
        {
            Debug.Log("No 4th gameobject in player, nothing to destroy");
        }


    }

    public void DropObject()
    {
        DropObjectServerRpc();
    }
    [ServerRpc(RequireOwnership=false)]
    private void DropObjectServerRpc()
    {

    }

}
