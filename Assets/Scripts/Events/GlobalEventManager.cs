using System;
using Unity.Netcode;

public static class GlobalEventManager
{
    public static Action<NetworkObject, PlayerData> OnOwnerSetSpecification;
    public static Action<BulletColor> OnClientBulletColorChanged;
    public static Action<BulletSize> OnClientBulletSizeChanged;
    public static Action<bool> OnIsSelectionMenuActive;
    public static Action<NetworkPlayerData> OnTargetDestroyed;
}
