using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoardController : NetworkBehaviour
{
    [SerializeField] private GameObject scoreSlot;
    private Dictionary<ulong, ClientScoreSlotController> usedSlots = new Dictionary<ulong, ClientScoreSlotController>();
    private NetworkList<ScorePlayerData> clients;
    
    private void Awake()
    {
        clients = new NetworkList<ScorePlayerData>();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(" Client" + (int)OwnerClientId + "'s scoreboard is spawned.");
        GlobalEventManager.OnTargetDestroyed += OnTargetDestroyed;
        clients.OnListChanged += OnClientsAddOrChanged;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnTargetDestroyed -= OnTargetDestroyed;
        clients.OnListChanged -= OnClientsAddOrChanged;
    }

    private void OnClientsAddOrChanged(NetworkListEvent<ScorePlayerData> changeevent)
    {
        Debug.Log("Client"+(int)OwnerClientId + "'s network list changed.");
        if (IsOwner)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                UpdateScoreboardServerRpc(clients[i]);
            }   
        }
    }

    private void OnTargetDestroyed(NetworkPlayerData networkPlayerData)
    {
        Debug.Log("Client"+(int)OwnerClientId + "'s Scoreboard is Updating with score to Client" + (int)networkPlayerData.clientID);
        if (IsOwner)
        {
            UpdateScoreDatasServerRpc(networkPlayerData);   
        }
    }

    [ServerRpc]
    private void UpdateScoreDatasServerRpc(NetworkPlayerData networkPlayerData)
    {
        if (IsNetworkListContainID(networkPlayerData.clientID))
        {
            int index = GetNetworkObjectIndexByID(networkPlayerData.clientID);
            int tempCurrentScore = clients[index].currentScore + CalculateScore(networkPlayerData);
            clients[index] = new ScorePlayerData()
            {
                currentClientID = networkPlayerData.clientID,
                currentScore = tempCurrentScore
            };
        }
        else
        {
            clients.Add(new ScorePlayerData()
            {
                currentClientID = networkPlayerData.clientID,
                currentScore = CalculateScore(networkPlayerData)
            });
        }
    }
    
    [ServerRpc]
    private void UpdateScoreboardServerRpc(ScorePlayerData scoreData)
    {
        UpdateScoreBoardClientRpc(scoreData);
    }

    [ClientRpc]
    private void UpdateScoreBoardClientRpc(ScorePlayerData scoreData)
    {
        if (usedSlots.ContainsKey(scoreData.currentClientID))
        {
            UpdateSlot(scoreData);
        }
        else
        {
            CreateNewSlot(scoreData);
            // GameObject tempSlot = Instantiate(scoreSlot);
            // tempSlot.GetComponent<NetworkObject>().Spawn();
            // tempSlot.transform.SetParent(transform);
            // Debug.Log(scoreData.currentClientID + " id " + scoreData.currentScore + " SCORE");
            // tempSlot.GetComponent<ClientScoreSlotController>().InitSlot(scoreData.currentClientID, scoreData.currentScore);
            // usedSlots.Add(scoreData.currentClientID, tempSlot.GetComponent<ClientScoreSlotController>());
        }
    }
    
    private void UpdateSlot(ScorePlayerData scoreData)
    {
        usedSlots[scoreData.currentClientID].UpdateSlot(scoreData.currentScore);
    }

    private void CreateNewSlot(ScorePlayerData scoreData)
    {
        GameObject tempSlot = Instantiate(scoreSlot, transform);
        Debug.Log(scoreData.currentClientID + " id " + scoreData.currentScore + " SCORE");
        tempSlot.GetComponent<ClientScoreSlotController>().InitSlot(scoreData.currentClientID, scoreData.currentScore);
        usedSlots.Add(scoreData.currentClientID, tempSlot.GetComponent<ClientScoreSlotController>());
    }

    private int CalculateScore(NetworkPlayerData networkPlayerData)
    {
        return (EnumUtils.IsEqualColor(networkPlayerData.currentBulletColor, GameModeController.Instance.ActiveColorMode) && 
                EnumUtils.IsEqualColor(networkPlayerData.currentBulletSize, GameModeController.Instance.ActiveSizeMode)) ? 1 : -1;
    }

    private bool IsNetworkListContainID(ulong id)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].currentClientID == id)
            {
                return true;
            }
        }

        return false;
    }

    private int GetNetworkObjectIndexByID(ulong id)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].currentClientID == id)
            {
                return i;
            }
        }

        return -1;
    }

}
public struct ScorePlayerData : INetworkSerializable, IEquatable<ScorePlayerData>
{
    public ulong currentClientID;
    public int currentScore;

    public ScorePlayerData(ulong clientID, int score)
    {
        currentClientID = clientID;
        currentScore = score;
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref currentClientID);
        serializer.SerializeValue(ref currentScore);
    }

    public bool Equals(ScorePlayerData other)
    {
        return currentClientID == other.currentClientID && currentScore == other.currentScore;
    }

    public override bool Equals(object obj)
    {
        return obj is ScorePlayerData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(currentClientID, currentScore);
    }
}
// public class ScoreData : NetworkVariableBase, INetworkSerializable
// {
//     public List<ulong> clientIDs = new List<ulong>();
//     public List<ScorePlayerData> clientScoreSlots = new List<ScorePlayerData>();
//
//     public void UpdateSlotValues(ulong id, int score)
//     {
//         clientScoreSlots[clientIDs.IndexOf(id)].score += score;
//     }
//
//     public override void WriteField(FastBufferWriter writer)
//     {
//         writer.WriteValueSafe(clientIDs.Count);
//         foreach (var dataEntry in clientIDs)
//         {
//             writer.WriteValueSafe(dataEntry);
//         }
//         writer.WriteValueSafe(clientScoreSlots.Count);
//         foreach (var dataEntry in clientScoreSlots)
//         {
//             writer.WriteValueSafe(dataEntry.color);
//             writer.WriteValueSafe(dataEntry.score);
//         }
//     }
//
//     public override void ReadField(FastBufferReader reader)
//     {
//         var itemsToUpdate = (int)0;
//         reader.ReadValueSafe(out itemsToUpdate);
//         clientIDs.Clear();
//         for (int i = 0; i < itemsToUpdate; i++)
//         {
//             reader.ReadValueSafe(out ulong newEntry);
//             clientIDs.Add(newEntry);
//         }
//         clientScoreSlots.Clear();
//     }
//
//     public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
//     {
//         
//     }
//     
//     public override void WriteDelta(FastBufferWriter writer)
//     {
//         
//     }
//
//     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//     {
//         int count = 0;
//         serializer.SerializeValue(ref count);
//         for (int i = 0; i < count; i++)
//         {
//             serializer.SerializeValue(ref clientIDs[i]);
//         }
//     }
// }



// public struct ScoreData : INetworkSerializable , IEquatable<ScoreData>
// {
//     public ScoreData(ScoreData lastScoreData, NetworkPlayerData networkPlayerData)
//     {
//         
//     }
//
//     public ScoreData(NetworkPlayerData networkPlayerData)
//     {
//         
//     }
//
//     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//     {
//         
//     }
//
//     public bool Equals(ScoreData other)
//     {
//         return false;
//     }
// }