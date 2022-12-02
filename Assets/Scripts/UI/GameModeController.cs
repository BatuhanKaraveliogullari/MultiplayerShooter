using Enums;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace UI
{
    public class GameModeController : NetworkBehaviour
    {
        public static GameModeController Instance;
    
        private NetworkVariable<GameMode> networkGameModeVariable = new NetworkVariable<GameMode>();

        public BulletColor ActiveColorMode { get => networkGameModeVariable.Value.currentBulletColor; }
        public BulletSize ActiveSizeMode { get => networkGameModeVariable.Value.currentBulletSize; }
    
        [SerializeField] private TMP_Text gameModeText;
        [SerializeField] private float roundSession = 1f;
    
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        
            networkGameModeVariable.OnValueChanged += OnGameModeChanged;
        
            if (IsHost)
            {
                InvokeRepeating(nameof(SetGameMode), 0, roundSession); 
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        
            networkGameModeVariable.OnValueChanged -= OnGameModeChanged;
        }

        private void OnGameModeChanged(GameMode previousValue, GameMode newValue)
        {
            if(IsOwner) ChangedGameModeServerRpc(newValue);
        }

        [ServerRpc]
        private void ChangedGameModeServerRpc(GameMode gameMode)
        {
            UpdateGameModeClientRpc(gameMode);
        }

        [ClientRpc]
        private void UpdateGameModeClientRpc(GameMode gameMode)
        {
            gameModeText.text = gameMode.currentBulletColor.ToString() + " - " +
                                gameMode.currentBulletSize.ToString();
            gameModeText.color = ColorUtils.GetColorWithEnum(gameMode.currentBulletColor);
        }
    

        private void SetGameMode()
        {
            networkGameModeVariable.Value = new GameMode()
            {
                currentBulletColor = GetRandomColor(),
                currentBulletSize = GetRandomSize()
            };
        }

        private BulletColor GetRandomColor()
        {
            int randomValue = Random.Range(0, 100);
     
            if (randomValue < 33)
            {
                return BulletColor.Red;
            }
            else if(randomValue < 66)
            {
                return BulletColor.Blue;
            }
            else
            {
                return BulletColor.Green;
            }
        }
    
        private BulletSize GetRandomSize()
        {
            int randomValue = Random.Range(0, 100);
     
            if (randomValue < 33)
            {
                return BulletSize.Small;
            }
            else if(randomValue < 66)
            {
                return BulletSize.Standard;
            }
            else
            {
                return BulletSize.Large;
            }
        }
    }
}