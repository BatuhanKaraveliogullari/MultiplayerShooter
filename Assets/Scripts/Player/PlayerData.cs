using UnityEngine;

public class PlayerData
{
    public ulong playerID;
    public Color playerColor;
    public BulletColor currentBulletColor;
    public BulletSize currentBulletSize;

    public PlayerData(ulong clientID)
    {
        playerID = clientID;
        playerColor = ColorUtils.GetColorForClient(clientID);
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
