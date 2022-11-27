using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameModeController : NetworkBehaviour
{
    public static GameModeController Instance;

    public BulletColor ActiveColorMode { get => networkGameModeVariable.Value.currentBulletColor; }
    public BulletSize ActiveSizeMode { get => networkGameModeVariable.Value.currentBulletSize; }
    [SerializeField] private TMP_Text gameModeText;
    [SerializeField] private float roundSession = 1f;
    private NetworkVariable<GameMode> networkGameModeVariable = new NetworkVariable<GameMode>();
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public override void OnNetworkSpawn()
    {
        networkGameModeVariable.OnValueChanged += OnGameModeChanged;
        if (IsHost)
        {
            InvokeRepeating(nameof(SetGameMode), 0, roundSession); 
        }
    }

    private void OnGameModeChanged(GameMode previousvalue, GameMode newvalue)
    {
        if(IsOwner) ChangedGameModeServerRpc(newvalue);
    }

    [ServerRpc]
    private void ChangedGameModeServerRpc(GameMode gameMode)
    {
        UpdateGameModeCLientRpc(gameMode);
    }

    [ClientRpc]
    private void UpdateGameModeCLientRpc(GameMode gameMode)
    {
        gameModeText.text = gameMode.currentBulletColor.ToString() + " - " +
                            gameMode.currentBulletSize.ToString();
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

public struct GameMode : INetworkSerializable
{
    public BulletColor currentBulletColor;
    public BulletSize currentBulletSize;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref currentBulletColor);
        serializer.SerializeValue(ref currentBulletSize);
    }
}