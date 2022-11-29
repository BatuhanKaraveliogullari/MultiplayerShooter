using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StunBombController : NetworkBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float stunDuration;
    [SerializeField] private float stunRadius = 5f;
    private NetworkPlayerData playerData;
    private NetworkVariable<Vector3> networkExplosionPosition = new NetworkVariable<Vector3>();
    private List<PlayerMovementController> cachedAffectedPlayers = new List<PlayerMovementController>();
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
        Debug.Log(" Bomb has bean destroyed on " + (int)OwnerClientId + "'s game. ");
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
            Debug.Log("Client" + (int)OwnerClientId + " affected stun bumb placed by Client" + (int)networkPlayerData.clientID);
            playerCollider.GetComponentInParent<PlayerMovementController>().StunPlayer(stunDuration);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(networkExplosionPosition.Value, 5);
    }
}
