using Unity.Netcode;

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