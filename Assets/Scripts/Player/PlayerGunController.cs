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
        //Debug.Log(" A bullet requested from server. ");
        FireBulletClientRpc(playerData);
    }

    [ClientRpc]
    private void FireBulletClientRpc(NetworkPlayerData playerData)
    {
        //Debug.Log(" Bullet is cresting by clients. ");
        GameObject go = Instantiate(baseBulletPrefabs, muzzle.position, Quaternion.identity);
        go.GetComponent<BulletController>().InitBullet(playerData, muzzle.forward);
    }
}