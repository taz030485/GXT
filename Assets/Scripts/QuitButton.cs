using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
