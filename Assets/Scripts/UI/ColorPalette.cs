using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPalette : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BulletColor bulletColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEventManager.OnClientBulletColorChanged.Invoke(bulletColor);
    }
}