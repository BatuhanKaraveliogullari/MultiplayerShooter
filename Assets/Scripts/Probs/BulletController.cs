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
    private Vector3 bulletDirection;
    private BulletColor bulletColor;
    private BulletSize bulletSize;
    private Color playerColor;
    private Transform _transform;

    public void InitBullet(NetworkPlayerData playerData, Vector3 direction)
    {
        bulletDirection = direction;
        bulletColor = playerData.currentBulletColor;
        bulletSize = playerData.currentBulletSize;
        playerColor = playerData.playerColor;
        _transform = transform;
        //Set Size and Color here
        if (IsOwner)
        {
            CommitColorServerRpc(ColorUtils.GetColorWithEnum(bulletColor));
        }
        bulletMeshRenderer.material.color = ColorUtils.GetColorWithEnum(bulletColor);
        _transform.localScale = VectorUtils.GetScaleWithEnum(bulletSize);
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
}
