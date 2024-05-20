using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;

public class PlayerTestController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public Rigidbody2D rb;
    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        direction = new Vector2(x, y).normalized;
    }
    // Called after Update
    private void FixedUpdate()
    {
        rb.velocity = direction * walkSpeed;
    }
}
