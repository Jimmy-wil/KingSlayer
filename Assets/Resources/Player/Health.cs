using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private DeathMenuScript deathMenu;
    [SerializeField]
    public int currentHealth, maxHealth;

    [SerializeField]
    private Color hitColor = Color.red;

    private Coroutine changeSpriteColorRoutine;

    [SerializeField]
    private SpriteRenderer Sprite;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public event EventHandler OnHealthChanged;

    public void InitializeHealth(int healthValue)
    {
        currentHealth=healthValue;
        maxHealth=healthValue;
        isDead=false;
    }


    private IEnumerator ChangeSpriteColorRoutine()
    {
        Sprite.color = hitColor;

        yield return new WaitForSeconds(0.15f);

        Sprite.color = Color.white;

        changeSpriteColorRoutine = null;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) return;
        // if self to self
        if (sender == this.gameObject)
        {
            return;
        }

        // if enemy to enemy
        if (sender.layer == gameObject.layer && sender.layer == LayerMask.NameToLayer("Enemy")) return;
        
        if(OnHealthChanged != null) 
            OnHealthChanged(this, EventArgs.Empty);


        DealDamageServerRpc(amount);

        if (changeSpriteColorRoutine != null)
        {
            StopCoroutine(changeSpriteColorRoutine);
        }
        changeSpriteColorRoutine = StartCoroutine(ChangeSpriteColorRoutine());


        if (currentHealth>0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead=true;

            if(!(this.gameObject.layer == LayerMask.NameToLayer("Enemy")))
            {
                ShowDeathMenuServerRpc(AuthenticationService.Instance.PlayerId);
                deathMenu.ShowDeathMenu();

            }

            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership=false)]
    private void ShowDeathMenuServerRpc(string playedId)
    {
        ShowDeathMenuClientRpc(playedId);
    }
    [ClientRpc]
    private void ShowDeathMenuClientRpc(string playedId)
    {
        if (AuthenticationService.Instance.PlayerId == playedId)
            deathMenu.ShowDeathMenu();
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
