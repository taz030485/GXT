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

    public Echo echoPrefab;

    static Dictionary<Vector3Int, Hex.HexType> mapData = new();
    static Dictionary<Vector3Int, Enemy> enemies = new();
    static Dictionary<Vector3Int, Echo> echos = new();

    private void Awake()
    {
        manager = this;
        mapData = new();
        enemies = new();
        echos = new();
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

    public static Vector3Int AddEnemy(Enemy enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Vector3Int enemyGridPosition = manager.grid.WorldToCell(enemyPosition);
        enemies.Add(enemyGridPosition, enemy);
        return enemyGridPosition;
    }

    public static void RemoveEnemy(Vector3Int tilePosition)
    {
        if (enemies.ContainsKey(tilePosition))
        {
            enemies.Remove(tilePosition);
        }
    }

    public static bool TileHasEnemy(Vector3Int tilePosition)
    {
        if (enemies.TryGetValue(tilePosition, out Enemy enemy))
        {
            return true;
        }
        return false;
    }

    public static Enemy GetEnemy(Vector3Int position)
    {
        if (enemies.TryGetValue(position, out Enemy enemy))
        {
            return enemy;
        }
        return null;
    }

    public static bool TileFree(Vector3Int tilePosition)
    {
        if (echos.TryGetValue(tilePosition, out Echo echo))
        {
            return false;
        }

        if (mapData.TryGetValue(tilePosition, out Hex.HexType type))
        {
            if (type != Hex.HexType.Mountain)
            {
                return true;
            }
        }

        return false;
    }

    public static void AddEcho(List<ActionsManager.Action> actions)
    {
        Echo echo = Instantiate<Echo>(manager.echoPrefab, manager.transform);
        echo.SetGrid(manager.grid);
        //Debug.Log(actions[0].position + " " + actions[0].type);
        Vector3Int position = echo.SetActions(actions);
        echos.Add(position, echo);
    }
}
