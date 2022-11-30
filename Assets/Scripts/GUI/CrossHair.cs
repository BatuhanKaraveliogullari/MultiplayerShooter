using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private Texture2D _crossHair;
    [SerializeField] private float _crossHairWidth;
    [SerializeField] private float _crossHairHeight;
 
 
    private void OnGUI() {
 
        GUI.DrawTexture(new Rect((Screen.width * 0.5f) - (_crossHairWidth * 0.5f)
            ,(Screen.height * 0.5f) - (_crossHairHeight  * 0.5f)
            , _crossHairWidth, _crossHairHeight), _crossHair);
         
    }
}
