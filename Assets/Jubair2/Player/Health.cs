using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;


public class Health : NetworkBehaviour
{
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;

    public UnityEvent<GameObject> OnHitWithReference;
    public UnityEvent<GameObject> OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public GameOverMenu gameOverMenu; // Add reference to the GameOverMenu
    private Playcontrol playerController;

    private void Start()
    {
        playerController = GetComponent<Playcontrol>();
    }

    public void InitializeHealth(int healthValue)
    {
        maxHealth = healthValue;
        currentHealth = maxHealth;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) return;
        if (sender.layer == gameObject.layer) return; // Prevent friendly fire

        currentHealth -= amount;
        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            isDead = true;
            currentHealth = 0; // Ensure health doesn't drop below zero
            OnDeathWithReference?.Invoke(sender);
            gameOverMenu.ShowGameOverMenu(); // Show the game over menu
            DestroyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyServerRpc()
    {
        DestroyClientRpc();
    }

    [ClientRpc]
    private void DestroyClientRpc()
    {
        Destroy(gameObject);
    }

    public void StayAsPhantom()
    {
        if (IsServer)
        {
            playerController.isPhantom.Value = true;
        }
        else
        {
            ActivatePhantomModeServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ActivatePhantomModeServerRpc()
    {
        playerController.isPhantom.Value = true;
    }
}