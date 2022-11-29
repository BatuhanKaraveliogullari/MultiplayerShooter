using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StunBombController : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> networkExplosionPosition = new NetworkVariable<Vector3>();
    
    [SerializeField] private LayerMask playerLayer;
    
    [SerializeField] private float stunDuration = 15f;
    [SerializeField] private float stunRadius = 5f;
    private NetworkPlayerData playerData;
    

    public void InitExplosive(NetworkPlayerData networkPlayerData)
    {
        playerData = networkPlayerData;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        networkExplosionPosition.Value = transform.position;
    }

    public void Explode()
    {
        if(!IsOwner) return;
        RequestExplosionServerRpc(networkExplosionPosition.Value, playerData);
    }

    [ServerRpc]
    private void RequestExplosionServerRpc(Vector3 position, NetworkPlayerData networkPlayerData)
    {
        ExecuteExplosionClientRpc(position, networkPlayerData);
    }

    [ClientRpc]
    private void ExecuteExplosionClientRpc(Vector3 position, NetworkPlayerData networkPlayerData)
    {
        StunPlayers(networkPlayerData, position, stunRadius);
        Destroy(gameObject);
    }

    private void StunPlayers(NetworkPlayerData networkPlayerData, Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, playerLayer);
        foreach (var playerCollider in colliders)
        {
            if (playerCollider.GetComponentInParent<PlayerController>().OwnerClientId == networkPlayerData.clientID) continue;
            playerCollider.GetComponentInParent<PlayerMovementController>().StunPlayer(stunDuration);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(networkExplosionPosition.Value, 5);
    }
}
