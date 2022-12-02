using Enums;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ColorPalette : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private BulletColor bulletColor;

        public void OnPointerClick(PointerEventData eventData)
        {
            GlobalEventManager.OnClientBulletColorChanged.Invoke(bulletColor);
        }
    }
}