using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDegat : MonoBehaviour
{
    public event EventHandler OnPlayerEnterTrigger;
    public int damageAmount = 10; // Définir la quantité de dégâts à infliger
    public float damageInterval = 2f; // Intervalle de temps entre chaque dégât en secondes
    public AudioSource damageAudioSource; // Référence à l'AudioSource

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Health playerHealth = collider.GetComponent<Health>();
            if (playerHealth != null && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerHealth));
            }
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(Health playerHealth)
    {
        while (true)
        {
            playerHealth.GetHit2(damageAmount);
            damageAudioSource.Play();
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
