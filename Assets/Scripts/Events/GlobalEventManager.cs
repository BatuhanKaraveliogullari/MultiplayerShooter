using System;
using Enums;
using Player;
using Unity.Netcode;

namespace Events
{
    public static class GlobalEventManager
    {
        public static Action<NetworkObject, PlayerData> OnOwnerSetSpecification;
        public static Action<BulletColor> OnClientBulletColorChanged;
        public static Action<BulletSize> OnClientBulletSizeChanged;
        public static Action<bool> OnIsSelectionMenuActive;
        public static Action<NetworkPlayerData> OnTargetDestroyed;
    }
}
