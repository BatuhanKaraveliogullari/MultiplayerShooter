using UnityEngine;

namespace GUI
{
    public class KeyFeatureDisplayer : MonoBehaviour
    {
        private void OnGUI()
        {
            UnityEngine.GUI.Label(new Rect(10, 100, 300, 20), "Press 'B' to select Bullet");
            UnityEngine.GUI.Label(new Rect(10, 120, 300, 20), "Press 'C' to Plant Stun Bomb");
            UnityEngine.GUI.Label(new Rect(10, 140, 300, 20), "Press 'V' to Explode Placed Stun Bomb");
        }
    }
}