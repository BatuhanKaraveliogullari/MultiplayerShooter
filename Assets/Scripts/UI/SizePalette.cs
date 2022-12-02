using Enums;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SizePalette : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private BulletSize bulletSize;

        public void OnPointerClick(PointerEventData eventData)
        {
            GlobalEventManager.OnClientBulletSizeChanged.Invoke(bulletSize);
        }
    }
}