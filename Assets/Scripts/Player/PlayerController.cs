using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public PlayerData currentPlayerData;
    protected bool isMenuActive;

    private void OnEnable()
    {
        GlobalEventManager.OnIsSelectionMenuActive += OnIsMenuActive;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnIsSelectionMenuActive -= OnIsMenuActive;
    }

    private void OnIsMenuActive(bool isActive)
    {
        isMenuActive = isActive;
    }

    public virtual void Init(PlayerData playerData)
    {
        currentPlayerData = playerData;
    }
}
