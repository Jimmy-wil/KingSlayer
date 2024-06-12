using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimation : MonoBehaviour
{
     // Référence à l'Animator
    public Animator animator;

    // Référence au composant AudioSource
    public AudioSource audioSource;

    void Update()
    {
        // Vérifier l'état du booléen "Running" dans l'Animator
        bool isRunning = animator.GetBool("Running");

        // Si "Running" est vrai, jouer le son
        if (isRunning)
        {
            PlaySound();
        }
        else
        {
            // Sinon, arrêter le son
            StopSound();
        }
    }

    void PlaySound()
    {
        // Si le son n'est pas déjà en train de jouer, le jouer
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void StopSound()
    {
        // Arrêter le son si celui-ci est en train de jouer
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

