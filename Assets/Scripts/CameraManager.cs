using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager manager = null;

    public Grid grid;

    static bool CameraMoving = false;
    static Vector3 CameraStartPosition = Vector3.zero;
    static Vector3 PointerStartPosition = Vector3.zero;

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

    private void PointerStart(Vector3 position)
    {
        Vector3Int gridPosition = grid.WorldToCell(position);
        if (!Player.PlayerUnderPointer(gridPosition))
        {
            PointerStartPosition = position;
            CameraStartPosition = transform.position;
            CameraMoving = true;
        }
    }

    private void PointerMove(Vector3 position)
    {
        if (!CameraMoving) return;
        
        Vector3 moveDifference = PointerStartPosition - position;
        //Debug.Log(moveDifference);
        transform.position = CameraStartPosition + moveDifference;
    }

    private void PointerStop(Vector3 position)
    {
        if (!CameraMoving) return;
        
        CameraMoving = false;
    }
}
