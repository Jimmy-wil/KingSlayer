using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private RandomItemScript randomDrop;

    [SerializeField]
    private GameObject deathMenu;
    [SerializeField]
    public int currentHealth, maxHealth;

    [SerializeField]
    private Color hitColor = Color.red;

    private Coroutine changeSpriteColorRoutine;

    [SerializeField]
    private SpriteRenderer Sprite;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    public bool isDead = false;

    [ServerRpc(RequireOwnership=false)]
    public void AddHpServerRpc(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
        AddHpClientRpc(amount);
    }
    [ClientRpc]
    public void AddHpClientRpc(int amount)
    {
        if (IsHost) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public event EventHandler OnHealthChanged;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;

    }


    private IEnumerator ChangeSpriteColorRoutine()
    {
        Sprite.color = hitColor;

        yield return new WaitForSeconds(0.15f);

        Sprite.color = Color.white;

        changeSpriteColorRoutine = null;
    }


    private GameObject Sender;


    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) return;

        // if self to self
        if (sender == this.gameObject)
        {
            Debug.Log("Hit yourself");
            return;
        }
        // if enemy to enemy
        if (sender.layer == gameObject.layer && sender.layer == LayerMask.NameToLayer("Enemy")) return;

        if (changeSpriteColorRoutine != null)
        {
            StopCoroutine(changeSpriteColorRoutine);
        }
        changeSpriteColorRoutine = StartCoroutine(ChangeSpriteColorRoutine());

        Sender = sender;

        // if client to client
        DealDamageServerRpc(amount, this.gameObject);
    }

    public void GetHit2(int amount)
    {
        if (OnHealthChanged != null)
            OnHealthChanged(this, EventArgs.Empty);


        if (changeSpriteColorRoutine != null)
        {
            StopCoroutine(changeSpriteColorRoutine);
        }
        changeSpriteColorRoutine = StartCoroutine(ChangeSpriteColorRoutine());

        if(Sender != null)
            OnHitWithReference?.Invoke(Sender);
        
        Sender = null;

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            isDead = true;

            Debug.Log($"IsOwner: {IsOwner}, {this.gameObject.name}");

            if (this.gameObject.layer != LayerMask.NameToLayer("Enemy") && IsOwner)
            {
                deathMenu = GameObject.Find("DeathMenu");
                deathMenu.GetComponent<DeathMenuScript>().ShowDeathMenu();
                if(randomDrop != null)
                {
                    var clone = Instantiate(randomDrop.GetItem(), this.gameObject.transform.position, Quaternion.identity);
                    clone.GetComponent<NetworkObject>().Spawn();

                }

            }

            if (IsOwner)
            {
                DestroyObjectServerRpc();

            }

        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void DestroyObjectServerRpc()
    {
        Debug.Log("Getting destroyed");
        if (randomDrop != null)
        {
            Debug.Log("Dropping something");

            var clone = Instantiate(randomDrop.GetItem(), this.gameObject.transform.position, Quaternion.identity);
            clone.GetComponent<NetworkObject>().Spawn();

        }

        Destroy(gameObject); // Die
    }


    [ServerRpc(RequireOwnership=false)]
    public void DealDamageServerRpc(int amount, NetworkObjectReference gameObject)
    {
        if (gameObject.TryGet(out NetworkObject networkObject))
        {
            networkObject.GetComponent<Health>();

            if (networkObject.IsOwnedByServer)
            {
                GetHit2(amount);
                DealDamageClientRpc(amount, gameObject);
            }
            else
            {
                currentHealth -= amount;
                DealDamageClientRpc(amount, gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Couldn't get network object from NetworkObjectReference!");
        }

    }

    [ClientRpc]
    private void DealDamageClientRpc(int amount, NetworkObjectReference gameObject)
    {
        if (IsHost) return;
        if (gameObject.TryGet(out NetworkObject networkObject))
        {
            networkObject.GetComponent<Health>().GetHit2(amount);
        }

    }

    public float GetHealthPercent(){
        return (float)currentHealth/(float)maxHealth;
    }

}
