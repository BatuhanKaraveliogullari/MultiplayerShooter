using System;
using Unity.Netcode;

namespace Player
{
    /// <summary> This class is creating for transmiting some data on network.
    /// We are updating scoreboard for each client. This class is holding updated data for clients. </summary>
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
}