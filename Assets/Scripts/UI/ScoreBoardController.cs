using System.Collections.Generic;
using Events;
using Player;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace UI
{
    public class ScoreBoardController : NetworkBehaviour
    {
        private NetworkList<ScorePlayerData> clients;
    
        [SerializeField] private GameObject scoreSlot;
    
        private Dictionary<ulong, ClientScoreSlotController> usedSlots = new Dictionary<ulong, ClientScoreSlotController>();

        private void Awake()
        {
            clients = new NetworkList<ScorePlayerData>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            GlobalEventManager.OnTargetDestroyed += OnTargetDestroyed;
            clients.OnListChanged += OnClientsAddOrChanged;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            GlobalEventManager.OnTargetDestroyed -= OnTargetDestroyed;
            clients.OnListChanged -= OnClientsAddOrChanged;
        }

        private void OnClientsAddOrChanged(NetworkListEvent<ScorePlayerData> changeEvent)
        {
            if (!IsOwner) return;
            for (int i = 0; i < clients.Count; i++)
            {
                RequestScoreboardUpdateServerRpc(clients[i]);
            }
        }

        private void OnTargetDestroyed(NetworkPlayerData networkPlayerData)
        {
            if (IsOwner)
            {
                RequestScoreDataUpdateServerRpc(networkPlayerData);   
            }
        }

        [ServerRpc]
        private void RequestScoreDataUpdateServerRpc(NetworkPlayerData networkPlayerData)
        {
            if (IsNetworkListContainID(networkPlayerData.clientID, out int index))
            {
                int tempCurrentScore = clients[index].currentScore + GetScoreOfShot(networkPlayerData);
                clients[index] = new ScorePlayerData(networkPlayerData.clientID, tempCurrentScore);
            }
            else
            {
                clients.Add(new ScorePlayerData(networkPlayerData.clientID, GetScoreOfShot(networkPlayerData)));
            }
        }
    
        [ServerRpc]
        private void RequestScoreboardUpdateServerRpc(ScorePlayerData scoreData)
        {
            UpdateScoreboardClientRpc(scoreData);
        }

        [ClientRpc]
        private void UpdateScoreboardClientRpc(ScorePlayerData scoreData)
        {
            if (usedSlots.ContainsKey(scoreData.currentClientID))
            {
                UpdateSlot(scoreData);
            }
            else
            {
                CreateNewSlot(scoreData);
            }
        }
    
        private void UpdateSlot(ScorePlayerData scoreData)
        {
            usedSlots[scoreData.currentClientID].UpdateSlot(scoreData.currentScore);
        }

        private void CreateNewSlot(ScorePlayerData scoreData)
        {
            GameObject tempSlot = Instantiate(scoreSlot, transform);
            tempSlot.GetComponent<ClientScoreSlotController>().InitSlot(scoreData.currentClientID, scoreData.currentScore);
            usedSlots.Add(scoreData.currentClientID, tempSlot.GetComponent<ClientScoreSlotController>());
        }

        private int GetScoreOfShot(NetworkPlayerData networkPlayerData)
        {
            return (EnumUtils.IsEqualColor(networkPlayerData.currentBulletColor, GameModeController.Instance.ActiveColorMode) && 
                    EnumUtils.IsEqualColor(networkPlayerData.currentBulletSize, GameModeController.Instance.ActiveSizeMode)) ? 1 : -1;
        }

        private bool IsNetworkListContainID(ulong id, out int index)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].currentClientID == id)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}