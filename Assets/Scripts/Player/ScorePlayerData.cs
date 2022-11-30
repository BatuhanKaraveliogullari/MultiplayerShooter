using System;
using Unity.Netcode;

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