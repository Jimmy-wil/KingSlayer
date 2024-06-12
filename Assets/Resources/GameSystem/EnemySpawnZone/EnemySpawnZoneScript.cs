using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class EnemySpawnZoneScript : NetworkBehaviour
{
    public GameObject Enemy;

    [SerializeField]
    private float spawnRange = 3f;

    [SerializeField]
    private float detectPlayerRadius = 5f;

    [SerializeField]
    private float maxTimer = 3;

    [SerializeField]
    private uint maxInstances = 3;

    private NetworkVariable<float> timer = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        timer.Value = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        SpawnTimer();
    }

    private void SpawnTimer()
    {
        timer.Value -= Time.deltaTime;
        if (DetectPlayer() || this.transform.childCount >= maxInstances)
        {
            timer.Value = maxTimer;
            return;
        }

        if (timer.Value < 0f)
        {
            SpawnEnemyServerRpc();
            timer.Value += maxTimer;
        }

    }

    private bool DetectPlayer()
    {
        try
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(this.transform.position, detectPlayerRadius))
            {
                GameObject gameObject = collider.gameObject;
                if (gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void SpawnEnemyServerRpc()
    {
        GameObject clone = Instantiate(Enemy, new Vector2(
            this.transform.position.x + Random.Range(-spawnRange, spawnRange),
            this.transform.position.y + Random.Range(-spawnRange, spawnRange)), 
            Quaternion.identity);
        
        var cloneNetworkObject = clone.GetComponent<NetworkObject>();
        cloneNetworkObject.Spawn();
        
        clone.transform.parent = this.transform;
    }

}
