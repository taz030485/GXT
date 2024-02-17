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

    public Arrow blueArrorPrefab;
    public Arrow redArrorPrefab;
    List<Arrow> arrows = new();

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

    private void PointerStart(Vector3 position, Vector2 screenPosition)
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

    bool foundEnemy = false;
    private void PointerMove(Vector3 position, Vector2 screenPosition)
    {
        if (!movingPlayer) return;

        Vector3Int gridPosition = grid.WorldToCell(position);

        if (gridPosition != lastPointerPosition)
        {
            if (path.Contains(gridPosition))
            {
                ClearNeighbors(lastPointerPosition);
                ClearPath();
                int index = path.IndexOf(gridPosition);
                path.RemoveRange(index, path.Count - index);
                ShowPath();
                foundEnemy = false;
                if (path.Count > 0)
                {
                    lastPointerPosition = path[path.Count-1];
                }
            }

            if (!foundEnemy)
            {
                if (GridManager.TileHasEnemy(gridPosition))
                {
                    ClearNeighbors(lastPointerPosition);
                    AttackPosition(gridPosition);
                    AddAttack(lastPointerPosition, gridPosition);

                    // Stop expanding
                    foundEnemy = true;
                }else{
                    if (GridManager.TileFree(gridPosition))
                    {
                        ClearNeighbors(lastPointerPosition);
                        AddToPath(gridPosition);
                        if (path.Count > 1)
                        {
                            AddArrow(lastPointerPosition, gridPosition);
                        }
                        HighlightNeighbors(gridPosition);
                    }
                }
            }
            lastPointerPosition = gridPosition;
        }
    }

    private void PointerStop(Vector3 position, Vector2 screenPosition)
    {
        if (!movingPlayer) return;

        Vector3Int gridPosition = grid.WorldToCell(position);
        ClearNeighbors(gridPosition);
        ClearPath();
        path.Clear();
        indicatorTileMap.SetTile(gridPosition, null);
        movingPlayer = false;
        foundEnemy = false;

        // Do Actions
    }

    void HighlightNeighbors(Vector3Int centrePosition)
    {
        foreach (Vector3Int neighbour in Hex.Neighbors(centrePosition))
        {
            if (!path.Contains(neighbour))
            {
                if (!GridManager.TileFree(neighbour))
                {
                    indicatorTileMap.SetTile(neighbour, greyIndicator);
                }else if (GridManager.TileHasEnemy(neighbour))
                {
                    indicatorTileMap.SetTile(neighbour, redIndicator);
                }else
                {
                    indicatorTileMap.SetTile(neighbour, greenIndicator);
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

    void AttackPosition(Vector3Int position)
    {
        indicatorTileMap.SetTile(position, redIndicator);
    }

    void AddToPath(Vector3Int position)
    {
        indicatorTileMap.SetTile(position, blueIndicator);
        path.Add(position);

    }

    void AddArrow(Vector3Int position, Vector3Int target)
    {
        Arrow arrow = Instantiate<Arrow>(blueArrorPrefab, transform);
        arrow.PointAt(grid.CellToWorld(position), grid.CellToWorld(target));
        arrows.Add(arrow);
    }

    void AddAttack(Vector3Int position, Vector3Int target)
    {
        Arrow arrow = Instantiate<Arrow>(redArrorPrefab, transform);
        arrow.PointAt(grid.CellToWorld(position), grid.CellToWorld(target));
        arrows.Add(arrow);
    }

    void ShowPath()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector3Int node = path[i];
            indicatorTileMap.SetTile(node, blueIndicator);
            if (i != 0)
            {
                AddArrow(path[i-1], node);
            }
        }
    }

    void ClearPath()
    {
        foreach (Vector3Int node in path)
        {
            indicatorTileMap.SetTile(node, null);
        }

        foreach (Arrow arrow in arrows)
        {
            Destroy(arrow.gameObject);
        }
        arrows.Clear();
    }
}
