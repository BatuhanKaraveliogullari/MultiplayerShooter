using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NetworkSpawnController : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject targetManager;
    
    public override void OnNetworkSpawn()
    {
        Debug.Log(" Network Spawner Spawned. ");

        if (!IsOwner) return;
        RequestPlayerSpawnServerRpc(OwnerClientId);
        if (IsHost)
        {
            RequestTargetsServerRpc();
        }
    }

    [ServerRpc]
    private void RequestPlayerSpawnServerRpc(ulong clientID)
    {
        var gameObject = Instantiate(playerPrefab, new Vector3(Random.Range(-10,10), 1, Random.Range(-10,10)), Quaternion.identity);
        gameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }
    
    [ServerRpc]
    public void RequestTargetsServerRpc()
    {
        var go = Instantiate(targetManager);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
