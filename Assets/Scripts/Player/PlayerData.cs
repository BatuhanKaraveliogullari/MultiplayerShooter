using Unity.Netcode;
using UnityEngine;

public class PlayerData
{
    public Color playerColor;
    public ulong playerID;
    public BulletColor currentBulletColor;
    public BulletSize currentBulletSize;

    public PlayerData(ulong clientID)
    {
        playerColor = ColorUtils.GetColorForClient(clientID);
        playerID = clientID;
        currentBulletColor = BulletColor.Blue;
        currentBulletSize = BulletSize.Large;
        GlobalEventManager.OnClientBulletColorChanged += ClientColorChanged;
        GlobalEventManager.OnClientBulletSizeChanged += ClientSizeChanged;
    }

    private void ClientSizeChanged(BulletSize newSize)
    {
        Debug.Log(" Client is changed size (" + newSize + ") ");
        currentBulletSize = newSize;
    }

    private void ClientColorChanged(BulletColor newColor)
    {
        Debug.Log(" Client is changed color (" + newColor + ") ");
        currentBulletColor = newColor;
    }
}
