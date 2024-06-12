using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private Coroutine transparencyCoroutine;

    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(WeaponParent weapon, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer) // Don't hit the Player Layer
            return;

        int amount = weapon.degat;

        if (spriteRenderer != null)
        {
            if (transparencyCoroutine != null)
            {
                StopCoroutine(transparencyCoroutine);
            }
            transparencyCoroutine = StartCoroutine(HandleTransparency());
        }

        currentHealth -= amount;
        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator HandleTransparency()
    {
        Color color = spriteRenderer.color;
        color.a = 0.7f; // Make sprite 50% transparent
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.3f); // Adjust this value for the delay duration

        color.a = 1.0f; // Make sprite fully opaque again
        spriteRenderer.color = color;
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / (float)maxHealth;
    }
}