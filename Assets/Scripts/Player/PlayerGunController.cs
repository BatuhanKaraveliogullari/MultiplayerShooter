using Probs;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerGunController : PlayerController
    {
        private readonly NetworkVariable<NetworkPlayerData> netPlayerData = new NetworkVariable<NetworkPlayerData>(writePerm: NetworkVariableWritePermission.Owner);
    
        [Header("Bullet")]
        [SerializeField] private Transform muzzle;
        [SerializeField] private NetworkObject baseBulletPrefabs;
        [SerializeField] private float shootFrequency = 1;
    
        [Header("Bomb")]
        [SerializeField] private StunBombController stunBombPrefab;
        [SerializeField] private float coolDownForStunBomb = 5f;
    
        private float nextTimeToFire = 0f;
        private float nextTimeForStunBomb = 0f;
    
        private StunBombController placedBomb;
        private ulong placedBombClientID;
        private RaycastHit cachedHit;
        private Transform cachedCameraTransform;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            nextTimeToFire = 1 / shootFrequency;
            nextTimeForStunBomb = coolDownForStunBomb;
            placedBombClientID = 100;
            cachedCameraTransform = GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            if(!IsOwner || isMenuActive) return;
        
            if (Input.GetMouseButton(0) && nextTimeToFire >= 1f / shootFrequency)
            {
                netPlayerData.Value = new NetworkPlayerData(currentPlayerData);
                RequestBulletServerRpc(netPlayerData.Value);
                nextTimeToFire = 0f;
            }

            if (Input.GetKeyDown(KeyCode.C) && nextTimeForStunBomb >= coolDownForStunBomb && placedBomb == null)
            {
                netPlayerData.Value = new NetworkPlayerData(currentPlayerData);
                RequestStunBombServerRpc(netPlayerData.Value);
                nextTimeForStunBomb = 0f;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                RequestExplodeBombServerRpc();
            }

            nextTimeToFire += Time.deltaTime;
            nextTimeForStunBomb += Time.deltaTime;
        }

        [ServerRpc]
        private void RequestBulletServerRpc(NetworkPlayerData playerData)
        {
            if (Physics.Raycast(cachedCameraTransform.position, cachedCameraTransform.forward, out cachedHit))
            {
                if (Vector3.Distance(cachedCameraTransform.position, cachedHit.point) > 1f)
                {
                    muzzle.LookAt(cachedHit.point);
                }
            }
            else
            {
                muzzle.LookAt(cachedCameraTransform.position + (cachedCameraTransform.forward * 30f));
            }
        
            NetworkObject createdBullet = Instantiate(baseBulletPrefabs, muzzle.position, Quaternion.identity);
            createdBullet.Spawn();
            createdBullet.GetComponent<BulletController>().InitBullet(playerData, muzzle.forward);
        }

        [ServerRpc]
        private void RequestStunBombServerRpc(NetworkPlayerData playerData)
        {
            placedBomb = Instantiate(stunBombPrefab, new Vector3(cachedTransform.position.x, 0, cachedTransform.position.z), Quaternion.identity);
            placedBomb.GetComponent<NetworkObject>().Spawn();
            placedBomb.InitExplosive(playerData);
            placedBombClientID = playerData.clientID;
        }

        [ServerRpc]
        private void RequestExplodeBombServerRpc()
        {
            if (placedBomb != null && OwnerClientId == placedBombClientID)
            {
                placedBomb.Explode();
                placedBomb = null;
                placedBombClientID = 100;   
            }
        }
    }
}