using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParentScript : NetworkBehaviour, WeaponInterface
{
    public int dmg = 20;
    public Vector2 Pointerposition { get; set; }

    public SpriteRenderer characterRenderer, weaponRenderer;
    public float delay = 0.3f;
    private bool attackBlocked;

    public bool IsAttacking { get; set; }

    public Transform circleOrigin;
    public float radius;

    private void Start()
    {
        characterRenderer = this.transform.parent.GetComponentInChildren<SpriteRenderer>();
    }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    public Animator animator;
    private void Update()
    {
        if(!IsOwner) return;
        if(IsAttacking) return;

        Vector2 direction = (Pointerposition - (Vector2)transform.position).normalized;

        transform.right = direction;

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            // scale.y = -1;
            weaponRenderer.flipY = true;
        }
        else if (direction.x > 0)
        {
            // scale.y = 1;
            weaponRenderer.flipY = false;
        }
        transform.localScale=scale;
   
        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder-1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder+1;
        }
   
    }

    public void Attack()
    {
        if (attackBlocked) return;

        animator.SetTrigger("Attack");
        
        IsAttacking=true;
        attackBlocked=true;

        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked=false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Vector3 position =circleOrigin==null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position,radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position,radius))
        {
            Health health = collider.GetComponent<Health>();
            if(health)
            {
                health.GetHit(dmg, transform.parent.gameObject);
            }
        }
    }
}
