using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnZoneScript : NetworkBehaviour
{
    public GameObject player;
    public float minX = -5;
    public float maxX = 5;
    public float minY = -2;
    public float maxY = 2;

    // Start is called before the first frame update
    public void RandomSpawnPlayer()
    {
        RandomSpawnPlayerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RandomSpawnPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject clone = Instantiate(player, new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)), Quaternion.identity);
        clone.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }
    
}
