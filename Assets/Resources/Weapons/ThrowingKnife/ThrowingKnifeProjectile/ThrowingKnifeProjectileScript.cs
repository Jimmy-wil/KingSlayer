using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class ThrowingKnifeProjectileScript : NetworkBehaviour
{
    public float speed;
    public int dmg = 6;
    public float radius = 1.06f;

    [SerializeField]
    private float despawnTime;
    
    
    void Update()
    {
        Advance();

        DetectColliders();
        
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0f)
        {
            Destroy(this.gameObject);
        }



    }

    private void Advance()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = this.transform.forward * speed * Time.deltaTime;
    }

    public void DetectColliders()
    {
        if (!IsOwner) return;
        
        try
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(this.transform.position, radius))
            {
                Health health = collider.GetComponent<Health>();

                if (health && IsOwner)
                {
                    health.GetHit(dmg, transform.parent.gameObject);

                }

                Debug.Log($"Hit {collider.gameObject.name}, beginning to destroy projectile");

                Destroy(this.gameObject);

            }

        }
        catch
        {
            Debug.LogWarning($"Failed to detect colliders as a {this.gameObject.name}!");
        }
        
    }
}
