using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetController : NetworkBehaviour
{
    public void HitTarget(NetworkPlayerData networkPlayerData)
    {
        Debug.Log(" Target destroyed by " + networkPlayerData.clientID);
        if(!IsOwner) return;
        GetComponent<NetworkObject>().Despawn();
        GlobalEventManager.OnTargetDestroyed.Invoke(networkPlayerData);
    }
}
