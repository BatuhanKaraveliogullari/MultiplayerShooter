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
        base.OnNetworkSpawn();
        
        if (IsOwner) 
        {
            RequestColorChangeServerRpc(ColorUtils.GetColorForClient(OwnerClientId));
        }
        
        playerColor.material.color = ColorUtils.GetColorForClient(OwnerClientId);
        gunColor.material.color = ColorUtils.GetColorForClient(OwnerClientId);  
    }
    
    [ServerRpc]
    public void RequestColorChangeServerRpc(Color color)
    {
        netPlayerColor.Value = color;
    }
}