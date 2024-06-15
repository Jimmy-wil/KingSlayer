using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class RangedParent : NetworkBehaviour, WeaponInterface
{
    public GameObject Projectile;
    private GameObject tempProjectile;

    public int dmg = 20;
    public Vector2 Pointerposition { get; set; }

    public SpriteRenderer weaponRenderer;
    public float delay = 0.3f;
    private bool attackBlocked;

    public bool IsAttacking{ get; set; }

    public Transform circleOrigin;
    public float radius;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    public Animator animator;
    private void Update()
    {
        if (!IsOwner) return;
        if (IsAttacking) return;

        Vector2 direction = (Pointerposition - (Vector2)transform.position).normalized;

        transform.right = direction;

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale=scale;
   
        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = LayerMask.NameToLayer("Player") - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = LayerMask.NameToLayer("Player") + 1;
        }
   
    }

    public void Attack()
    {
        if (attackBlocked) return;

        animator = GetComponentInChildren<Animator>();
        
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

    public void ShootProjectile()
    {
        if (!IsOwner) return;

        Vector2 position = circleOrigin.position;
        Vector2 direction = (Pointerposition - (Vector2)transform.position).normalized;

        tempProjectile = Instantiate(Projectile, position, Quaternion.FromToRotation(Vector3.right, direction));
        
        ShootProjectileServerRpc(IsServer);

        Destroy(tempProjectile, 3);

        tempProjectile = null;
    }

    [ServerRpc(RequireOwnership=false)]
    private void ShootProjectileServerRpc(bool isServer, ServerRpcParams serverRpcParams = default)
    {
        if (tempProjectile == null) return;

        tempProjectile.GetComponent<NetworkObject>().Spawn();

        if (!isServer)
        {
            Debug.Log("New owner!");
            tempProjectile.GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);

        }

    }

}
