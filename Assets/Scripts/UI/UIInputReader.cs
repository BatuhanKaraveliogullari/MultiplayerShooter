using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputReader : MonoBehaviour
{
    [SerializeField] private GameObject bulletSelectionPanel;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bulletSelectionPanel.SetActive(!bulletSelectionPanel.activeInHierarchy);
            SetCursorState(bulletSelectionPanel.activeInHierarchy);
            SetSelectionmenuActivity(bulletSelectionPanel.activeInHierarchy);
        }
    }

    private void SetCursorState(bool isActive)
    {
        Cursor.visible = isActive;
        Cursor.lockState = (isActive) ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
    public void SetSelectionmenuActivity(bool isActive)
    {
        GlobalEventManager.OnIsSelectionMenuActive.Invoke(isActive);
    }
}
