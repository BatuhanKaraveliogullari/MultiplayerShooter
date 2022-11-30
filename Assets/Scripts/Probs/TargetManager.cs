using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject targetController;
    [SerializeField] private int targetCount;
    
    private int destroyedTargetCount = 0;

    private void OnDisable()
    {
        GlobalEventManager.OnTargetDestroyed -= OnTargetDestroyed;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        GlobalEventManager.OnTargetDestroyed += OnTargetDestroyed;
        
        if (!IsHost) return;
        RequestTargetsServerRpc();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        GlobalEventManager.OnTargetDestroyed -= OnTargetDestroyed;
    }

    private void OnTargetDestroyed(NetworkPlayerData networkPlayerData)
    {
        if(!IsHost) return;
        destroyedTargetCount++;
        if (destroyedTargetCount >= targetCount)
        {
            RequestTargetsServerRpc();
            destroyedTargetCount = 0;
        }
    }

    [ServerRpc]
    private void RequestTargetsServerRpc()
    {
        for (int i = 0; i < targetCount; i++)
        {
            Instantiate(targetController, new Vector3(Random.Range(-30,30), 0, Random.Range(-30,30)), Quaternion.identity).Spawn();
        }
    }
}