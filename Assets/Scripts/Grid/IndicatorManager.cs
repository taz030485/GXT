using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IndicatorManager : MonoBehaviour
{
    static IndicatorManager manager = null;

    public Grid grid;
    public Tilemap indicators;

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
        
    }

    private void PointerMove(Vector3 position)
    {
        
    }

    private void PointerStop(Vector3 position)
    {
        
    }
}
