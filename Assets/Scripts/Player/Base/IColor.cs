using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IColor
{
    void RequestColorChangeServerRpc(Color color);
}
