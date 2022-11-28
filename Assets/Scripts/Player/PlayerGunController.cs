using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using UnityEngine;

public class PlayerGunController : PlayerController
{
    private readonly NetworkVariable<NetworkPlayerData> netPlayerData = new NetworkVariable<NetworkPlayerData>(writePerm: NetworkVariableWritePermission.Owner);
    [Header("Bullet")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject baseBulletPrefabs;
    [SerializeField] private float shootFrequency = 1;
    [Header("Bomb")]
    [SerializeField] private GameObject stunBombPrefab;
    [SerializeField] private float coolDownForStunBomb = 5f;
    private float nextTimeToFire = 0f;
    private float nextTimeForStunBomb = 0f;
    private StunBombController placedStunBomb;
    private ulong placedID;
    

    private void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        nextTimeToFire = 1 / shootFrequency;
        nextTimeForStunBomb = coolDownForStunBomb;

        placedID = (ulong)100;
    }

    private void Update()
    {
        if(!IsOwner || isMenuActive) return;

        if (Input.GetMouseButton(0) && nextTimeToFire >= 1f / shootFrequency)
        {
            //we will shoot
            netPlayerData.Value = new NetworkPlayerData(currentPlayerData);
            RequestForBulletServerRpc(netPlayerData.Value);
            nextTimeToFire = 0f;
        }

        if (Input.GetKeyDown(KeyCode.C) && nextTimeForStunBomb >= coolDownForStunBomb && placedStunBomb == null)
        {
            netPlayerData.Value = new NetworkPlayerData(currentPlayerData);
            RequestForStunBombServerRpc(netPlayerData.Value);
            nextTimeForStunBomb = 0f;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Cached Bomb: " + placedStunBomb + " Created by Cleint" + (int)placedID);
            netPlayerData.Value = new NetworkPlayerData(currentPlayerData);
            RequestExplodeBombServerRpc(netPlayerData.Value);
        }

        nextTimeToFire += Time.deltaTime;
        nextTimeForStunBomb += Time.deltaTime;
    }

    [ServerRpc]
    private void RequestForBulletServerRpc(NetworkPlayerData playerData)
    {
        Debug.Log("Client" + (int)OwnerClientId + " Create new bullet belong to Client" + (int)playerData.clientID);
        GameObject go = Instantiate(baseBulletPrefabs, muzzle.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
        go.GetComponent<BulletController>().InitBullet(playerData, muzzle.forward);
    }

    [ServerRpc]
    private void RequestForStunBombServerRpc(NetworkPlayerData playerData)
    {
        Debug.Log("Client" + (int)OwnerClientId + " Create new Stun Bomb belong to Client" + (int)playerData.clientID);
        placedStunBomb = Instantiate(stunBombPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity).GetComponent<StunBombController>();
        placedStunBomb.GetComponent<NetworkObject>().Spawn();
        placedStunBomb.InitExplosive(playerData);
        placedID = playerData.clientID;
    }

    [ServerRpc]
    private void RequestExplodeBombServerRpc(NetworkPlayerData networkPlayerData)
    {
        if (placedStunBomb != null && OwnerClientId == placedID)
        {
            placedStunBomb.Explode();
            placedStunBomb = null;
            placedID = (ulong)100;   
        }
    }
}