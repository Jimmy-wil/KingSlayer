using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnGameObject : NetworkBehaviour
{
    public GameObject skeleton1;

    public void SpawnEnemy()
    {
        SpawnEnemyServerRpc();
    }

    [ServerRpc(RequireOwnership=false)]
    private void SpawnEnemyServerRpc()
    {
        GameObject clone = Instantiate(skeleton1, new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)), Quaternion.identity);
        var cloneNetworkObject = clone.GetComponent<NetworkObject>();
        cloneNetworkObject.Spawn();
    }

}
