using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : NetworkBehaviour, IColor
{
    private NetworkVariable<Color> NetColor = new();
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private MeshRenderer bulletMeshRenderer;
    [SerializeField] private LayerMask targetLayer;
    private Vector3 bulletDirection;
    private Transform _transform;
    private NetworkPlayerData currentPlayerData;

    public void InitBullet(NetworkPlayerData playerData, Vector3 direction)
    {
        Debug.Log("Client " + playerData.clientID);
        currentPlayerData = playerData;
        bulletDirection = direction;
        _transform = transform;
        if (IsOwner)
        {
            CommitColorServerRpc(ColorUtils.GetColorWithEnum(currentPlayerData.currentBulletColor));
        }
        bulletMeshRenderer.material.color = ColorUtils.GetColorWithEnum(currentPlayerData.currentBulletColor);
        _transform.localScale = VectorUtils.GetScaleWithEnum(currentPlayerData.currentBulletSize);
        InvokeRepeating(nameof(MoveBullet), 0, Time.deltaTime);
    }

    public void MoveBullet()
    {
        _transform.position += (bulletDirection.x * _transform.right + bulletDirection.y * _transform.up + bulletDirection.z * _transform.forward) * Time.deltaTime * bulletSpeed;
    }
    
    [ServerRpc]
    public void CommitColorServerRpc(Color color)
    {
        NetColor.Value = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer & targetLayer) != 0)
        {
            other.GetComponent<TargetController>().HitTarget(currentPlayerData);
            Destroy(gameObject);
        }    
    }
}
