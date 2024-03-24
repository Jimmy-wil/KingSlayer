using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    float speedX, speedY;

    [SerializeField]
    private float HP;
    public Rigidbody2D rb;

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            rb.gameObject.SetActive(false);
        }
}
   void Start()
   {
        rb = GetComponent<Rigidbody2D>();
   }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal")*moveSpeed;
        speedY = Input.GetAxisRaw("Vertical")*moveSpeed;
        rb.velocity = new Vector2(speedX,speedY);
    }
}
