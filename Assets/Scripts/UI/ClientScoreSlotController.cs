using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ClientScoreSlotController : MonoBehaviour
{
    [SerializeField] private Image currentPlayerColor;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;

    public void InitSlot(ulong id, int score)
    {
        currentPlayerColor.color = ColorUtils.GetColorForClient(id);
        playerName.text = "Player" + id;
        UpdateSlot(score);
    }
    
    public void UpdateSlot(int score)
    {
        playerScore.text = score.ToString();
    }
}