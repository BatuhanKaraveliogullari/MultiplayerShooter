using System;
using UnityEngine;

public class KeyFeatureDisplayer : MonoBehaviour
{
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 100, 300, 20), "Press 'B' to select Bullet");
        GUI.Label(new Rect(10, 120, 300, 20), "Press 'C' to Plant Stun Bomb");
        GUI.Label(new Rect(10, 140, 300, 20), "Press 'V' to Explode Placed Stun Bomb");
    }
}