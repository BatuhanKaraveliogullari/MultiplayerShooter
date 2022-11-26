using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private Image indicatorImage;
    [SerializeField] private TMP_Text playerNameText;

    private void OnEnable()
    {
        GlobalEventManager.OnOwnerSetSpecification += OnPlayerSetup;
    }
    
    private void OnDisable()
    {
        GlobalEventManager.OnOwnerSetSpecification -= OnPlayerSetup;
    }
    
    private void OnPlayerSetup(NetworkObject networkObject, PlayerData playerData)
    {
        if (!networkObject.IsOwner) return;
        indicatorImage.color = playerData.playerColor;
        playerNameText.text = playerData.playerName;
    }
}
