using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetController : NetworkBehaviour
{
    public void HitTarget(NetworkPlayerData networkPlayerData)
    {
        if(!IsOwner) return;
        
        GlobalEventManager.OnTargetDestroyed.Invoke(networkPlayerData);
        GetComponent<NetworkObject>().Despawn();
    }
}