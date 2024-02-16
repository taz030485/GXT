using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    static GridManager manager = null;

    public Grid grid;
    public Tilemap map;
    public Tile dirt;
    public Hex dirtSO;
    public Tile grass;
    public Hex grassSO;
    public Tile mountain;
    public Hex mountainSO;

    static Dictionary<Vector3Int, Hex.HexType> mapData = new();

    private void Awake()
    {
        manager = this;
    }

    private void Start()
    {
        foreach (var position in map.cellBounds.allPositionsWithin)
        {
            TileBase tile = map.GetTile(position);
            if (tile == null) continue;
            if (tile == dirt) Spawn(position, Hex.HexType.Dirt);
            if (tile == grass) Spawn(position, Hex.HexType.Grass);
            if (tile == mountain) Spawn(position, Hex.HexType.Mountain);
            map.SetTile(position, null);
        }    
    }

    void Spawn(Vector3Int position, Hex.HexType hexType)
    {
        mapData.Add(position, hexType);
        Debug.Log(position + " " + hexType);
        Vector3 spawnPosition = grid.CellToWorld(position);

        switch (hexType)
        {
            case Hex.HexType.Dirt:
                dirtSO.Spawn(spawnPosition);
                break;
            case Hex.HexType.Grass:
                grassSO.Spawn(spawnPosition);
                break;
            case Hex.HexType.Mountain:
                mountainSO.Spawn(spawnPosition);
                break;
            default:
                break;
        }
    }

    public static bool TileFree(Vector3Int tilePosition)
    {
        if (mapData.TryGetValue(tilePosition, out Hex.HexType type))
        {
            if (type != Hex.HexType.Mountain)
            {
                return true;
            }
        }
        return false;
    }
}
