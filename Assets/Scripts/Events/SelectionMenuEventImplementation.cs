using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMenuEventImplementation : MonoBehaviour
{
    public void SetSelectionmenuActivity(bool isActive)
    {
        GlobalEventManager.OnIsSelectionMenuActive.Invoke(isActive);
    }
}
