using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int[,] floorMap;

    public abstract void ClearMap();

    public abstract void GenerateMap(string _seed);

    public abstract int[] WorldPointToCoord(Vector3 point);

    public abstract Vector3 CoordToWorldPoint(int x, int y);
}
