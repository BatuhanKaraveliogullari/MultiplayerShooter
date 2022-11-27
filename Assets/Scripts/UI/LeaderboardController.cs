using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class LeaderboardController : NetworkBehaviour
{
    [SerializeField] private List<ClientScoreSlotController> clientScoreSlots = new List<ClientScoreSlotController>();
    private Dictionary<ulong, ClientScoreSlotController> usedClientSlots = new Dictionary<ulong, ClientScoreSlotController>();
    private void OnEnable()
    {
        GlobalEventManager.OnTargetDestroyed += OnTargetDestroyed;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnTargetDestroyed -= OnTargetDestroyed;
    }

    private void OnTargetDestroyed(ulong id, NetworkPlayerData networkPlayerData)
    {
        Debug.Log("Client"+(int)id + "'s Leaderboard is Updating with score to Client" + (int)networkPlayerData.clientID);
        
        if (usedClientSlots.ContainsKey(networkPlayerData.clientID))
        {
            usedClientSlots[networkPlayerData.clientID].UpdateSlot(CalculateScore(networkPlayerData));
        }
        else
        {
            ClientScoreSlotController tempClientSlot = clientScoreSlots[usedClientSlots.Count];
            tempClientSlot.gameObject.SetActive(true);
            tempClientSlot.InitSlot(this, networkPlayerData, CalculateScore(networkPlayerData));
            usedClientSlots.Add(networkPlayerData.clientID, tempClientSlot);
        }
    }
    
    public void OnLastScoreUpdated()
    {
        List<ClientScoreSlotController> sortedSlotList = usedClientSlots.Values.ToList().OrderByDescending(x => x.cachedScore).ToList();
        
        for (int i = 0; i < sortedSlotList.Count; i++)
        {
            sortedSlotList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -50f);
        }
    }

    private int CalculateScore(NetworkPlayerData networkPlayerData)
    {
        return 1;
    }
}
