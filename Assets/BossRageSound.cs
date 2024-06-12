using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRageSound : StateMachineBehaviour
{
    public AudioClip rageSound; // Le son à jouer
    public bool loopWhileRunning; // Booléen pour définir si le son doit être en boucle

    private AudioSource audioSource;

    // OnStateEnter est appelé lorsque la transition commence et que l'état commence à être évalué par le state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Vérifier si AudioSource est déjà trouvé
        if (audioSource == null)
        {
            // Trouver l'AudioSource sur l'Animator (ou un parent) ou créer un nouveau AudioSource
            audioSource = animator.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = animator.gameObject.AddComponent<AudioSource>();
            }
        }

        // Définir le clip audio
        audioSource.clip = rageSound;

        // Définir le mode boucle selon la condition
        if (loopWhileRunning)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }

        // Jouer le son
        audioSource.Play();
    }

    // OnStateExit est appelé lorsque la transition se termine et que l'état cesse d'être évalué par le state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Arrêter la lecture en boucle lorsque l'état se termine
        if (loopWhileRunning)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }
}