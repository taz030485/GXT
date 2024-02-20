using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    static CameraManager manager = null;

    public Grid grid;
    public Transform cameraTransform;

    static bool CameraMoving = false;
    Vector2 lastPointerPos = Vector2.zero;

    public float xMin = 3;
    public float xMax = 10;
    public float zMin = -3;
    public float zMax = 3;

    private void Awake()
    {
        manager = this;
        if (Application.isMobilePlatform)
        {
            Vector3 position = cameraTransform.localPosition;
            position.y = 4;
            cameraTransform.localPosition = position;
        }
    }

    private void OnEnable()
    {
        PointerManager.OnStart += PointerStart;
        PointerManager.OnMove += PointerMove;
        PointerManager.OnStop += PointerStop;
    }

    private void OnDisable()
    {
        PointerManager.OnStart -= PointerStart;
        PointerManager.OnMove -= PointerMove;
        PointerManager.OnStop -= PointerStop;
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
        transform.localPosition = transform.localPosition + worldDiff* 0.005f;
        KeepWithBounds();
        lastPointerPos = screenPosition;
    }

    private void PointerStop(Vector3 position, Vector2 screenPosition)
    {
        if (!CameraMoving) return;
        
        CameraMoving = false;
    }

    void KeepWithBounds()
    {
        Vector3 pos = transform.localPosition;
        if (pos.x < xMin) pos.x = xMin;
        if (pos.x > xMax) pos.x = xMax;
        if (pos.z < zMin) pos.z = zMin;
        if (pos.z > zMax) pos.z = zMax;
        transform.localPosition = pos;
    }
}
