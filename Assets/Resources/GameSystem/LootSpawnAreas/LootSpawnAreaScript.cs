using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class LootSpawnZoneScript : NetworkBehaviour
{

    [SerializeField]
    private List<Item> spawnItems = new List<Item>();

    [SerializeField]
    private float spawnRange = 3f;

    [SerializeField]
    private int minAmount = 0;
    [SerializeField]
    private int maxAmount = 3;

    private void Start()
    {
        if(!IsServer) return;

        SpawnEnemyServerRpc();
    }

    [ServerRpc(RequireOwnership=false)]
    private void SpawnEnemyServerRpc()
    {
        int limit = Random.Range(minAmount, maxAmount);

        for (int i = 0; i < limit; i++)
        {
            GameObject clone = Instantiate(spawnItems[Random.Range(0, spawnItems.Count)].gameObject, new Vector2(
                this.transform.position.x + Random.Range(-spawnRange, spawnRange),
                this.transform.position.y + Random.Range(-spawnRange, spawnRange)), 
                Quaternion.identity);
        
            var cloneNetworkObject = clone.GetComponent<NetworkObject>();
            cloneNetworkObject.Spawn();
        
            clone.transform.parent = this.transform;

        }

    
    }

}
