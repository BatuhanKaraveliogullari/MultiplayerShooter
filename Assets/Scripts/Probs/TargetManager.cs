using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetManager : NetworkBehaviour
{
    public GameObject targetController;
    public int targetCount;
    private int destroyedTargetCount = 0;
    
    private void OnEnable()
    {
        GlobalEventManager.OnTargetDestroyed += OnTargetDestroyed;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnTargetDestroyed -= OnTargetDestroyed;
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            RequestTargetOnlyServerRpc();
        }
    }

    private void OnTargetDestroyed(NetworkPlayerData networkPlayerData)
    {
        Debug.Log(" Target destroyed by " + networkPlayerData.clientID);
        destroyedTargetCount++;
        if (destroyedTargetCount >= targetCount)
        {
            RequestTargetOnlyServerRpc();
        }
    }
    
        
    [ServerRpc]
    public void RequestTargetOnlyServerRpc()
    {
        for (int i = 0; i < targetCount; i++)
        {
            var var = Instantiate(targetController, new Vector3(Random.Range(-30,30), 0, Random.Range(-30,30)), Quaternion.identity);
            var.GetComponent<NetworkObject>().Spawn();
        }
    }
}