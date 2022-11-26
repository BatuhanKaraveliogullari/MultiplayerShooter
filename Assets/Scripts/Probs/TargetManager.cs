using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetManager : NetworkBehaviour
{
    [SerializeField] private List<TargetController> targets = new List<TargetController>();
    private int hitCount = 0;

    [ServerRpc]
    public void OnTargetHitServerRpc(int index, NetworkPlayerData networkPlayerData)
    {
        GameObject tempGo = targets[index].gameObject;
        targets.RemoveAt(index);
        Destroy(tempGo);
        //hitCount++;
        // //Update Leaderboard.
        // if (hitCount >= targets.Count)
        // {
        //     //Restart Level
        //     ActiveAllTargetsServerRpc();
        //     hitCount = 0;   
        // }
        Debug.Log(networkPlayerData.clientID + " hit target " + (targets.Count - hitCount) + " remaining");
    }

    // [ServerRpc]
    // private void ActiveAllTargetsServerRpc()
    // {
    //     for (var i = 0; i < targets.Count; i++)
    //     {
    //         targets[i].gameObject.SetActive(true);
    //     }
    // }
}
