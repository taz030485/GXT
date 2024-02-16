using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hex", menuName = "Hex/HexObject", order = 1)]
public class Hex : ScriptableObject
{
    public enum HexType
    {
        Dirt,
        Grass,
        Mountain
    }

    public HexType hexType;
    public GameObject prefab;

    static Transform parent = null;

    public void Spawn(Vector3 position)
    {
        if (parent == null)
        {
            GameObject gameObject = new GameObject("Tile Parnet");
            parent = gameObject.transform;
        }

        Instantiate<GameObject>(prefab, position, Quaternion.identity, parent);
    }

    static Vector3Int
    LEFT = new(-1, 0, 0),
    RIGHT = new(1, 0, 0),
    DOWN = new(0, -1, 0),
    DOWNLEFT = new(-1, -1, 0),
    DOWNRIGHT = new(1, -1, 0),
    UP = new(0, 1, 0),
    UPLEFT = new(-1, 1, 0),
    UPRIGHT = new(1, 1, 0);

    static Vector3Int[] directions_when_y_is_even = { LEFT, RIGHT, DOWN, DOWNLEFT, UP, UPLEFT };
    static Vector3Int[] directions_when_y_is_odd = { LEFT, RIGHT, DOWN, DOWNRIGHT, UP, UPRIGHT };

    public static IEnumerable<Vector3Int> Neighbors(Vector3Int node) 
    {
        Vector3Int[] directions = (node.y % 2) == 0? 
            directions_when_y_is_even: 
            directions_when_y_is_odd;
        foreach (var direction in directions) 
        {
        Vector3Int neighborPos = node + direction;
        yield return neighborPos;
        }
    }
}
