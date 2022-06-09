using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Tiles {
    outerFloor = -1,
    floor = 0,
    outerWall = 1,
    leftOuterWall = 2,
    rightOuterWall = 3,
    innerWall = 4,
    leftInnerWall = 5,
    rightInnerWall = 6,
    leftWall = 7,
    rightWall = 8,

}

public class MapGenerator : MonoBehaviour {

    [Header("Dimensions")]
	public int width = 50;
	public int height = 50;

	[Header("Map Customisation")]
	[Range(0,100)]
	public int fillPercent = 49;
    public int smoothing = 3;
	public int minOuterRegionSize = 50;
	public int minInnerRegionSize = 50;

    [Header("Seed")]
	public string seed;
	public bool useRandomSeed;

	

    [Header("Tiles")]
    public GameObject[] outerFloor;
    public GameObject[] floor;
    public GameObject[] outerWall;
    public GameObject[] leftOuterWall;
    public GameObject[] rightOuterWall;
    public GameObject[] innerWall;
    public GameObject[] leftInnerWall;
    public GameObject[] rightInnerWall;
    public GameObject[] leftWall;
    public GameObject[] rightWall;

	int[,] map;
	int[,] visited;

	void Start() {
		GenerateMap();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
            ClearMap();
			GenerateMap();
		}
	}

	void GenerateMap() {
		map = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < smoothing; i ++) {
			SmoothMap();
		}

		burstSmallRegions();

        DrawMap();
	}

	void burstSmallRegions() {
		List<List<Tile>> outerRegions = GetRegions(-1);
		List<List<Tile>> innerRegions = GetRegions (0);

		foreach (List<Tile> outerRegion in outerRegions) {
			if (outerRegion.Count < minOuterRegionSize) {
				foreach (Tile tile in outerRegion) {
					map[tile.x,tile.y] = (int) Tiles.floor;
				}
			}
		}
		
		foreach (List<Tile> innerRegion in innerRegions) {
			if (innerRegion.Count < minInnerRegionSize) {
				foreach (Tile tile in innerRegion) {
					map[tile.x,tile.y] = (int) Tiles.outerFloor;
				}
			}
		}
	}

	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random rng = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					map[x,y] = (int) Tiles.outerFloor;
				}
				else {
					map[x,y] = (rng.Next(0,100) < fillPercent)? (int) Tiles.outerFloor: (int) Tiles.floor;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingOuterCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = (int) Tiles.outerFloor;
				else if (neighbourWallTiles < 4)
					map[x,y] = (int) Tiles.floor;

			}
		}
	}

	int GetSurroundingOuterCount(int tileX, int tileY) {
		int wallCount = 0;
		for (int neighbourX = tileX - 1; neighbourX <= tileX + 1; neighbourX ++) {
			for (int neighbourY = tileY - 1; neighbourY <= tileY + 1; neighbourY ++) {
                //if within the map
				if (IsWithinMap(neighbourX, neighbourY)) {
                    //if not the tile itself
					if (neighbourX != tileX || neighbourY != tileY) {
                        if (map[neighbourX, neighbourY] == -1) {
                            wallCount += 1;
                        }
					}
				}
				else {
					wallCount ++;
				}
			}
		}
		return wallCount;
	}

    List<List<Tile>> GetRegions(int tileType) {
		List<List<Tile>> regions = new List<List<Tile>>();
		visited = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (visited[x,y] == 0 && map[x,y] == tileType) {
					List<Tile> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);
				}
			}
		}

		return regions;
	}

	List<Tile> GetRegionTiles(int originX, int originY) {
		List<Tile> region = new List<Tile>();
		int tileType = map[originX, originY];

		Queue<Tile> queue = new Queue<Tile>();
		queue.Enqueue (new Tile(originX, originY));
		visited[originX, originY] = 1;

		while (queue.Count > 0) {
			Tile tile = queue.Dequeue();
			region.Add(tile);

			for (int x = tile.x - 1; x <= tile.x + 1; x++) {
				for (int y = tile.y - 1; y <= tile.y + 1; y++) {
					if (IsWithinMap(x,y) && (y == tile.y || x == tile.x)) {
						if (visited[x,y] == 0 && map[x,y] == tileType) {
							visited[x,y] = 1;
							queue.Enqueue(new Tile(x,y));
						}
					}
				}
			}
		}

		return region;
	}

    bool IsWithinMap(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}


	void DrawMap() {
		if (map != null) {
			for (int x = 0; x < width; x ++) {
				for (int y = 0; y < height; y ++) {
                    DrawTile(x, y);
				}
			}
		}
	}

    void DrawTile(int x, int y) {
        Tiles tileType = (Tiles) map[x, y];
        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
        GameObject tileToDraw;
        Debug.Log(tileType);

        switch(tileType) {
            case Tiles.outerFloor:
                tileToDraw = outerFloor[0];
                break;
            case Tiles.floor:
                tileToDraw = floor[0];
                break;
            case Tiles.outerWall:
                tileToDraw = outerWall[0];
                break;
            case Tiles.leftOuterWall:
                tileToDraw = leftOuterWall[0];
                break;
            case Tiles.rightOuterWall:
                tileToDraw = rightOuterWall[0];
                break;
            case Tiles.innerWall:
                tileToDraw = innerWall[0];
                break;
            case Tiles.leftInnerWall:
                tileToDraw = leftInnerWall[0];
                break;
            case Tiles.rightInnerWall:
                tileToDraw = rightInnerWall[0];
                break;
            case Tiles.leftWall:
                tileToDraw = leftWall[0];
                break;
            case Tiles.rightWall:
                tileToDraw = rightWall[0];
                break;
            default:
                tileToDraw = floor[0];
                break;
        }
        //Debug.Log("(" + x + "," + y + "): " + tileToDraw);
        Instantiate(tileToDraw, pos, Quaternion.identity);
    }

    void ClearMap() {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject floor in floors) {
            Destroy(floor);
        }
        foreach (GameObject wall in walls) {
            Destroy(wall);
        }
    }

    struct Tile {
		public int x;
		public int y;

		public Tile(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}
}