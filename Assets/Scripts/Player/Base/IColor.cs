using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IColor
{
    [ServerRpc] void CommitColorServerRpc(Color color);
}