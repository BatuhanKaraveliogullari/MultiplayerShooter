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
        if(IsOwner) RequestColorChangeServerRpc(ColorUtils.GetColorWithEnum(playerData.currentBulletColor));
        _transform.localScale = VectorUtils.GetScaleWithEnum(playerData.currentBulletSize);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            RequestColorChangeServerRpc(Color.white);    
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
    public void RequestColorChangeServerRpc(Color color)
    {
        NetColor.Value = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & targetLayer) == 0) return;
        other.GetComponent<TargetController>().HitTarget(currentPlayerData);
        Destroy(gameObject);
    }
}
