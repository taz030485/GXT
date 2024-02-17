using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitCheck : MonoBehaviour
{
    public GameObject panel;

    void Update()
    {
        if (Screen.height > Screen.width)
        {
            if (!panel.activeSelf) panel.SetActive(true);
        }else{
            if (panel.activeSelf) panel.SetActive(false);
        }
    }
}
