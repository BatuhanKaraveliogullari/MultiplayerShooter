using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SizePalette : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BulletSize bulletSize;

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEventManager.OnClientBulletSizeChanged.Invoke(bulletSize);
    }
}
