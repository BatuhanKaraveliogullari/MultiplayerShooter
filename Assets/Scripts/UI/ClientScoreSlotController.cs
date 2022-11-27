using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ClientScoreSlotController : MonoBehaviour
{
    [SerializeField] private Image playerColor;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;
    private LeaderboardController cachedLeaderboardController;
    [HideInInspector] public int cachedScore = 0;

    public void InitSlot(LeaderboardController leaderboardController, NetworkPlayerData playerData, int score)
    {
        cachedLeaderboardController = leaderboardController;
        playerColor.color = playerData.playerColor;
        playerName.text = "Player" + (int)playerData.clientID;
        UpdateSlot(score);
        playerScore.text = cachedScore.ToString();
        cachedLeaderboardController.OnLastScoreUpdated();
    }
    
    public void UpdateSlot(int score)
    {
        cachedScore += score;
        playerScore.text = cachedScore.ToString();
        cachedLeaderboardController.OnLastScoreUpdated();
    }
}
