using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : NetworkBehaviour, IColor
{
    private readonly NetworkVariable<Color> NetColor = new();
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private MeshRenderer bulletMeshRenderer;
    [SerializeField] private LayerMask targetLayer;
    private Vector3 bulletDirection;
    private Transform _transform;
    private NetworkPlayerData currentPlayerData;

    private void Awake()
    {
        _transform = transform;
        NetColor.OnValueChanged += OnColorChanged;
    }

    private void OnDisable()
    {
        NetColor.OnValueChanged -= OnColorChanged;
    }

    private void OnColorChanged(Color previousvalue, Color newvalue)
    {
        bulletMeshRenderer.material.color = newvalue;
    }

    public void InitBullet(NetworkPlayerData playerData, Vector3 direction)
    {
        currentPlayerData = playerData;
        bulletDirection = direction;
        InvokeRepeating(nameof(MoveBullet), 0, Time.deltaTime);
        if(IsOwner) CommitColorServerRpc(ColorUtils.GetColorWithEnum(playerData.currentBulletColor));
        _transform.localScale = VectorUtils.GetScaleWithEnum(playerData.currentBulletSize);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            CommitColorServerRpc(Color.white);    
        }
        else
        {
            bulletMeshRenderer.material.color = NetColor.Value;
            _transform.localScale = Vector3.one;
        }
    }

    public void MoveBullet()
    {
        _transform.position += (bulletDirection.x * _transform.right + bulletDirection.y * _transform.up + bulletDirection.z * _transform.forward) * Time.deltaTime * bulletSpeed;
    }
    
    [ServerRpc]
    public void CommitColorServerRpc(Color color)
    {
        Debug.Log(color);
        NetColor.Value = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Client" + (int)currentPlayerData.clientID + "'s bullet hit " + other.gameObject.name);
        if((1 << other.gameObject.layer & targetLayer) != 0)
        {
            Debug.Log("Client" + (int)currentPlayerData.clientID + "'s bullet hit the target.");
            other.GetComponent<TargetController>().HitTarget(currentPlayerData);
            Destroy(gameObject);
        }    
    }
}
