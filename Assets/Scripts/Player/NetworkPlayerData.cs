using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct NetworkPlayerData : INetworkSerializable, IEquatable<NetworkPlayerData>
{
    public Color playerColor;
    public ulong clientID;
    public BulletColor currentBulletColor;
    public BulletSize currentBulletSize;

    public NetworkPlayerData(PlayerData playerData)
    {
        playerColor = playerData.playerColor;
        clientID = playerData.playerID;
        currentBulletColor = playerData.currentBulletColor;
        currentBulletSize = playerData.currentBulletSize;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerColor);
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref currentBulletColor);
        serializer.SerializeValue(ref currentBulletSize);
    }

    public bool Equals(NetworkPlayerData other)
    {
        return playerColor.Equals(other.playerColor) && clientID == other.clientID && currentBulletColor == other.currentBulletColor && currentBulletSize == other.currentBulletSize;
    }

    public override bool Equals(object obj)
    {
        return obj is NetworkPlayerData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(playerColor, clientID, (int)currentBulletColor, (int)currentBulletSize);
    }
}
