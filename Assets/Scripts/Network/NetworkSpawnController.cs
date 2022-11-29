using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkSpawnController : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;
    [SerializeField] private NetworkObject targetManager;
    [SerializeField] private NetworkObject scoreboard;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        RequestPlayerSpawnServerRpc(OwnerClientId);
        if (!IsHost) return;
        RequestScoreboardServerRpc();
        RequestTargetManagerServerRpc();
    }

    [ServerRpc]
    private void RequestPlayerSpawnServerRpc(ulong clientID)
    {
        Instantiate(playerPrefab, new Vector3(Random.Range(-10,10), 1, Random.Range(-10,10)), Quaternion.identity).SpawnAsPlayerObject(clientID);
    }
    
    [ServerRpc]
    private void RequestTargetManagerServerRpc()
    {
        Instantiate(targetManager).Spawn();
    }
    
    [ServerRpc]
    private void RequestScoreboardServerRpc()
    {
        Instantiate(scoreboard).Spawn();
    }
}
