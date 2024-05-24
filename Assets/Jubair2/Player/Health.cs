using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private DeathMenuScript deathMenu;
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead =false;

    public event EventHandler OnHealthChanged;

    public void InitializeHealth(int healthValue)
    {
        currentHealth=healthValue;
        maxHealth=healthValue;
        isDead=false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) return;
        if (sender == this.gameObject)
        {
            Debug.Log("hit yourself");
            return;
        }
        if (sender.layer == gameObject.layer && sender.layer == LayerMask.NameToLayer("Enemy")) return;
        
        if(OnHealthChanged != null) 
            OnHealthChanged(this, EventArgs.Empty);

        DealDamageServerRpc(amount);

        if (currentHealth>0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead=true;
            deathMenu.ShowDeathMenu();
            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void DealDamageServerRpc(int amount)
    {
        currentHealth -= amount;
    }

    public float GetHealthPercent(){
        return (float)currentHealth/(float)maxHealth;
    }

}
