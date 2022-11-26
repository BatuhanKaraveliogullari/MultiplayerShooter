using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerColorController : PlayerController, IColor
{
    private readonly NetworkVariable<Color> netPlayerColor = new();
    [SerializeField] private MeshRenderer playerColor;
    [SerializeField] private MeshRenderer gunColor;

    public override void OnNetworkSpawn() 
    {
        if (IsOwner) 
        {
            CommitColorServerRpc(ColorUtils.GetColorForClient((int)OwnerClientId));
        }
        playerColor.material.color = ColorUtils.GetColorForClient((int)OwnerClientId);
        gunColor.material.color = ColorUtils.GetColorForClient((int)OwnerClientId);  
    }
    
    [ServerRpc]
    public void CommitColorServerRpc(Color color)
    {
        netPlayerColor.Value = color;
    }
}