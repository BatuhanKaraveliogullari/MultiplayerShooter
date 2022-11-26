using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovementController : PlayerController
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 5f;

    private float horizontalMovement;
    private float verticalMovement;
    
    private float cachedXRotation;
    private float cachedYRotation;

    private void Start()
    {
        currentPlayerData = new PlayerData(OwnerClientId);
        foreach (var playerController in GetComponents<PlayerController>())
        {
            playerController.Init(currentPlayerData);
        }
        
        if (!IsOwner)
        {
            Destroy(camera);
        }
        else
        {
            GlobalEventManager.OnOwnerSetSpecification.Invoke(NetworkObject, currentPlayerData);
        }
    }
    
    private void Update()
    {
        if(isMenuActive) return;
        Move();
        Rotate();
    }

    private void Move()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        verticalMovement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        transform.position += VectorUtils.CorrectHeight(horizontalMovement * transform.right + verticalMovement * transform.forward);
    }

    private void Rotate()
    {
        cachedXRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
        cachedYRotation = Mathf.Clamp(cachedYRotation - (Input.GetAxis("Mouse Y") * mouseSensitivity), -90, 90);
        //Value we got represent the rotation angle along axises. That's why we use Y for X angle.
        transform.rotation = Quaternion.Euler(cachedYRotation, cachedXRotation, 0);
    }
}
