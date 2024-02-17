using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image image;
    public Gradient gradient;
    public Canvas canvas;

    public void Hide()
    {
        canvas.enabled = false;
    }

    public void Show()
    {
        canvas.enabled = true;
    }

    public void UpdateHealthBar(float percent)
    {
        image.fillAmount = percent;
        image.color = gradient.Evaluate(percent);
    }
}
