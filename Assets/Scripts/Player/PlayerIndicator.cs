using Events;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
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
            playerNameText.text = playerData.playerID.ToString();
        }
    }
}
