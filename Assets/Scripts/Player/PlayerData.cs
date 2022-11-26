using UnityEngine;

public class PlayerData
{
    public Color playerColor;
    public string playerName;

    public PlayerData(int clientID)
    {
        playerColor = ColorUtils.GetColorForClient(clientID);
        playerName = "Player " + clientID.ToString();
    }
}
