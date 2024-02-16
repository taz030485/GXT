using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IndicatorManager : MonoBehaviour
{
    static IndicatorManager manager = null;

    public Grid grid;
    public Tilemap indicatorTileMap;
    public Tile greenIndicator;
    public Tile redIndicator;
    public Tile blueIndicator;
    public Tile greyIndicator;

    Vector3Int playerPosition = Vector3Int.zero;
    Vector3Int lastPointerPosition = Vector3Int.zero;
    List<Vector3Int> path = new List<Vector3Int>();

    bool movingPlayer = false;

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
        if (Player.PlayerUnderPointer(gridPosition))
        {
            playerPosition = gridPosition;
            AddToPath(gridPosition);
            HighlightNeighbors(gridPosition);
            lastPointerPosition = gridPosition;
            movingPlayer = true;
        }
    }

    private void PointerMove(Vector3 position)
    {
        if (!movingPlayer) return;

        Vector3Int gridPosition = grid.WorldToCell(position);

        if (gridPosition != lastPointerPosition)
        {
            if (GridManager.TileFree(gridPosition))
            {
                ClearNeighbors(lastPointerPosition);
                AddToPath(gridPosition);
                HighlightNeighbors(gridPosition);
            }
            lastPointerPosition = gridPosition;
        }
    }

    private void PointerStop(Vector3 position)
    {
        if (!movingPlayer) return;

        Vector3Int gridPosition = grid.WorldToCell(position);
        ClearNeighbors(gridPosition);
        ClearPath();
        path.Clear();

        movingPlayer = false;
    }

    void HighlightNeighbors(Vector3Int centrePosition)
    {
        foreach (Vector3Int neighbour in Hex.Neighbors(centrePosition))
        {
            if (!path.Contains(neighbour))
            {
                if (GridManager.TileFree(neighbour))
                {
                    indicatorTileMap.SetTile(neighbour, greenIndicator);
                }else{
                    indicatorTileMap.SetTile(neighbour, redIndicator);
                }
            }
        }
    }

    void ClearNeighbors(Vector3Int centrePosition)
    {
        foreach (Vector3Int neighbour in Hex.Neighbors(centrePosition))
        {
            if (!path.Contains(neighbour))
            {
                indicatorTileMap.SetTile(neighbour, null);
            }
        }
    }

    void AddToPath(Vector3Int position)
    {
        indicatorTileMap.SetTile(position, blueIndicator);
        path.Add(position);
    }

    void ClearPath()
    {
        foreach (Vector3Int node in path)
        {
            indicatorTileMap.SetTile(node, null);
        }
    }
}
