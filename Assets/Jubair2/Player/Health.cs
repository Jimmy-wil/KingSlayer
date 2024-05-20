using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if(isDead) return;
        if(sender.layer==gameObject.layer)//Dont Hit the Player Layer
        return;

        currentHealth-=amount;
        if (currentHealth>0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead=true;
            DestroyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        DestroyClientRpc();
        Destroy(gameObject);
    }
    [ClientRpc]
    public void DestroyClientRpc() 
    {
        Destroy(gameObject);
    }

}
