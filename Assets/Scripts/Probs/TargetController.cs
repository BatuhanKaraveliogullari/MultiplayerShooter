using Events;
using Player;
using Unity.Netcode;

namespace Probs
{
    public class TargetController : NetworkBehaviour
    {
        public void HitTarget(NetworkPlayerData networkPlayerData)
        {
            if(!IsOwner) return;
        
            GlobalEventManager.OnTargetDestroyed.Invoke(networkPlayerData);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}