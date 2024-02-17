using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBorder : MonoBehaviour
{
    public Image image;

    private void Awake()
    {
        image.enabled = true;
    }
}
