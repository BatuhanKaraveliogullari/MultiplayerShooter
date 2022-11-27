using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using UnityEngine;

public class PlayerGunController : PlayerController
{
    private readonly NetworkVariable<NetworkPlayerData> netPlayerData = new NetworkVariable<NetworkPlayerData>(writePerm: NetworkVariableWritePermission.Owner);
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject baseBulletPrefabs;
    [SerializeField] private float shootFrequency = 1;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        nextTimeToFire = 1 / shootFrequency;
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

        nextTimeToFire += Time.deltaTime;
    }

    [ServerRpc]
    private void RequestForBulletServerRpc(NetworkPlayerData playerData)
    {
        Debug.Log("Client" + (int)OwnerClientId + " Create new bullet belong to Client" + (int)playerData.clientID);
        GameObject go = Instantiate(baseBulletPrefabs, muzzle.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
        go.GetComponent<BulletController>().InitBullet(playerData, muzzle.forward);
    }
}