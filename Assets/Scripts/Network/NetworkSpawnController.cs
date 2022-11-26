using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkSpawnController : NetworkBehaviour
{
    public GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        Debug.Log(" Network Spawner Spawned. ");
        if (!IsOwner) return;
        RequestPlayerSpawnServerRpc(OwnerClientId);
    }

    [ServerRpc]
    private void RequestPlayerSpawnServerRpc(ulong clientID)
    {
        Debug.Log(" Requested Player Spawn from server. ");
        var go = Instantiate(playerPrefab, new Vector3(Random.Range(-10,10), 1, Random.Range(-10,10)), Quaternion.identity);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }
}
