using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class ThrowingKnifeProjectileScript : NetworkBehaviour
{
    public float speed = 8;
    public int dmg = 6;
    public float radius = 1.06f;


    void Update()
    {
        Advance();
        DetectColliders();

    }

    private void Advance()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = this.transform.right * speed;
    }

    public void DetectColliders()
    {
        if (!IsOwner) return;

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(this.transform.position, radius))
        {
        if (this.GetComponent<NetworkObject>().IsOwner && collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                return;
            }

            Health health = collider.GetComponent<Health>();

            if (health && IsOwner)
            {
                health.GetHit(dmg, gameObject);

            }

            Debug.Log($"Hit {collider.gameObject.name}, beginning to destroy projectile");

            Destroy(this.gameObject);

        }

        
    }
}
