using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyArrow : MonoBehaviour
{
    public List<Enemy> enemies;
    public Camera mainCamera;
    bool allDead = false;
    public Image image;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (allDead) return;

        Enemy nextEnemy = null;
        foreach (var enemy in enemies)
        {
            if (!enemy.DiedOnce())
            {
                nextEnemy = enemy;
                break;
            }
        }

        if (nextEnemy == null)
        {
            allDead = true;
            return;
        }
        
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(nextEnemy.transform.position);
        if ((screenPosition.x < 0) || (screenPosition.y < 0) || (screenPosition.x > Screen.width) || (screenPosition.y > Screen.height))
        {
            image.enabled = true;
            Vector2 viewportPosition = mainCamera.WorldToViewportPoint(nextEnemy.transform.position);
            viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
            viewportPosition.y = Mathf.Clamp01(viewportPosition.y);
            Vector2 edgePosition = mainCamera.ViewportToScreenPoint(viewportPosition);
            rectTransform.localPosition = edgePosition - new Vector2(Screen.width, Screen.height)/2;
            Vector2 vector = screenPosition - edgePosition;
            float angle = Vector2.SignedAngle(Vector2.up, vector);
            rectTransform.localRotation = Quaternion.Euler(0,0,angle);
        }else{
            image.enabled = false;
        }
    }
}
