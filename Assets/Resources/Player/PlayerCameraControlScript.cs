using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCameraControl : NetworkBehaviour
{
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private Transform target;


    void Start()
    {
        mainCamera = Camera.main;
        target = GetComponent<Transform>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {   
        if (!IsOwner) return;
        Vector3 targetPosition = target.position + offset;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
    }
}
