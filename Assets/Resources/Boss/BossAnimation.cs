using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossAnimation: MonoBehaviour
{
    [SerializeField] private Health health;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        if (health == null)
        {
            Debug.LogError("Health component is not assigned in the inspector on " + gameObject.name);
        }
    }

    private void Update()
    {
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (health != null && animator != null)
        {
            if (health.GetHealthPercent() < 0.5f)
            {
                animator.SetBool("Enraged", true);
            }
        }
    }
}