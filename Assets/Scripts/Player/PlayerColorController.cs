using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerColorController : NetworkBehaviour 
{
    private readonly NetworkVariable<Color> _netColor = new();
    [SerializeField] private MeshRenderer playerColor;
    [SerializeField] private MeshRenderer gunColor;

    public override void OnNetworkSpawn() 
    {
        if (IsOwner) 
        {
            CommitNetworkColorServerRpc(ColorUtils.GetColorForClient((int)OwnerClientId));
        }
        playerColor.material.color = ColorUtils.GetColorForClient((int)OwnerClientId);
        gunColor.material.color = ColorUtils.GetColorForClient((int)OwnerClientId);  
    }
    

    [ServerRpc]
    private void CommitNetworkColorServerRpc(Color color)
    {
        _netColor.Value = color;
    }
}