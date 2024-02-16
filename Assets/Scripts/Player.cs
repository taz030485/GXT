using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Player manager = null;

    public Grid grid;

    static Vector3Int playerPosition = Vector3Int.zero;

    private void Awake()
    {
        manager = this;
    }

    public static bool PlayerUnderPointer(Vector3Int position)
    {
        if (position == playerPosition)
        {
            return true;
        }else{
            return false;
        }
    }
}
