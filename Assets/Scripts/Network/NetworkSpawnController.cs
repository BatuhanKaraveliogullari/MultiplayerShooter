using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NetworkSpawnController : NetworkBehaviour
{
    public GameObject playerPrefab;
    public GameObject targetManager;
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

    private void OnTargetDestroyed(NetworkPlayerData networkPlayerData)
    {
        Debug.Log(" Target destroyed by " + networkPlayerData.clientID);
        destroyedTargetCount++;
        if (destroyedTargetCount >= targetCount)
        {
            RequestTargetOnlyServerRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(" Network Spawner Spawned. ");

        if (!IsOwner) return;
        RequestPlayerSpawnServerRpc(OwnerClientId);
        if (IsHost)
        {
            //RequestTargetsServerRpc();
            RequestTargetOnlyServerRpc();
        }
    }

    [ServerRpc]
    private void RequestPlayerSpawnServerRpc(ulong clientID)
    {
        Debug.Log(" Requested Player Spawn from server. ");
        var go = Instantiate(playerPrefab, new Vector3(Random.Range(-10,10), 1, Random.Range(-10,10)), Quaternion.identity);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }
    
    [ServerRpc]
    public void RequestTargetsServerRpc()
    {
        var go = Instantiate(targetManager);
        go.GetComponent<NetworkObject>().Spawn();
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
