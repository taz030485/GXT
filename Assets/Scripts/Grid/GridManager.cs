using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridManager : MonoBehaviour
{
    static GridManager manager = null;

    public Grid grid;
    public Tilemap map;

    private void Awake()
    {
        manager = this;
    }

    void Update()
    {
        
    }
}
