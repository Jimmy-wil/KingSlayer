using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerControllerScript : NetworkBehaviour
{
    public float moveSpeed = 3f;

    public Rigidbody2D rb;
    Vector2 moveDirection;
    
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Move();
    }

    void ProcessInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(x, y).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

}