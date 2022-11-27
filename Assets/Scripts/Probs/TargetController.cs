using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetController : NetworkBehaviour
{
    public void HitTarget(NetworkPlayerData networkPlayerData)
    {
        Debug.Log(gameObject.name + " hit by Client" + networkPlayerData.clientID + "'s bullet on Client" + OwnerClientId + " game.");
        if(!IsOwner) return;
        GlobalEventManager.OnTargetDestroyed.Invoke(OwnerClientId, networkPlayerData);
        GetComponent<NetworkObject>().Despawn();
    }
}
