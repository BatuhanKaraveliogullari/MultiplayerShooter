using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IPlayerInitialize
{
    protected PlayerData currentPlayerData;
    protected bool isMenuActive;
    protected Transform cachedTransform;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        GlobalEventManager.OnIsSelectionMenuActive += OnIsMenuActive;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        GlobalEventManager.OnIsSelectionMenuActive -= OnIsMenuActive;
    }
    
    private void OnIsMenuActive(bool isActive)
    {
        isMenuActive = isActive;
    }

    public virtual void Init(PlayerData playerData)
    {
        currentPlayerData = playerData;
        cachedTransform = transform;
    }
}
