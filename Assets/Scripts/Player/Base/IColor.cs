using UnityEngine;

namespace Player.Base
{
    public interface IColor
    {
        void RequestColorChangeServerRpc(Color color);
    }
}
