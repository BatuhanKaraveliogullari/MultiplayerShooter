using System;
using Unity.Netcode;

public static class GlobalEventManager
{
    public static Action<NetworkObject, PlayerData> OnOwnerSetSpecification;
}
