using Unity.Netcode;
using UnityEngine;

public class StunBombController : NetworkBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float stunDuration;
    private NetworkPlayerData playerData;
    private NetworkVariable<Vector3> networkExplosionPosition = new NetworkVariable<Vector3>();
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
        Debug.Log(" Explosion happened at " + position + " and affect " + ExplosionDamage(position, 5) + " players");
        Destroy(gameObject);
    }
    
    private int ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius, playerLayer);
        foreach (var playerCollider in colliders)
        {
            playerCollider.GetComponentInParent<PlayerMovementController>().StunPlayer(stunDuration);
        }

        return colliders.Length;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(networkExplosionPosition.Value, 5);
    }
}
