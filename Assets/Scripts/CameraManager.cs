using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    static CameraManager manager = null;

    public Grid grid;

    static bool CameraMoving = false;
    Vector2 lastPointerPos = Vector2.zero;

    private void Awake()
    {
        manager = this;
    }

    private void OnEnable()
    {
        PointerManager.OnStart += PointerStart;
        PointerManager.OnMove += PointerMove;
        PointerManager.OnStop += PointerStop;
    }

    private void PointerStart(Vector3 position, Vector2 screenPosition)
    {
        Vector3Int gridPosition = grid.WorldToCell(position);
        if (!Player.PlayerUnderPointer(gridPosition))
        {
            lastPointerPos = screenPosition;
            CameraMoving = true;
        }
    }

    private void PointerMove(Vector3 position, Vector2 screenPosition)
    {
        if (!CameraMoving) return;
        
        Vector2 moveDifference = lastPointerPos - screenPosition;
        Vector3 worldDiff = new(moveDifference.x, 0, moveDifference.y);
        transform.position = transform.position + worldDiff* 0.005f;
        lastPointerPos = screenPosition;
    }

    private void PointerStop(Vector3 position, Vector2 screenPosition)
    {
        if (!CameraMoving) return;
        
        CameraMoving = false;
    }
}
