using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum TileTypes {
    outerFloor = -1,
    floor = 0,
    outerWall0 = 1,
	outerWall1 = 2,
    outerWall2 = 3,
	outerWall3 = 4,
	outerWall4 = 5,
    innerWall0 = 6,
	innerWall1 = 7,
	innerWall2 = 8,
	innerWall3 = 9,
	innerWall4 = 10,
    leftWall = 11,
    rightWall = 12,

}

public class MapGenerator : MonoBehaviour {
	public Text textPrefab;

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
    public GameObject[] outerWall0;
	public GameObject[] outerWall1;
	public GameObject[] outerWall2;
	public GameObject[] outerWall3;
	public GameObject[] outerWall4;
    public GameObject[] innerWall0;
	public GameObject[] innerWall1;
	public GameObject[] innerWall2;
	public GameObject[] innerWall3;
	public GameObject[] innerWall4;
    public GameObject[] leftWall;
    public GameObject[] rightWall;

	int[,] floorMap;
	int[,] wallMap;
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
		floorMap = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < smoothing; i ++) {
			SmoothMap();
		}

		burstSmallRegions();

        DrawFloorMap();
		DrawWallMap();
	}

	void burstSmallRegions() {
		List<List<Tile>> outerRegions = GetRegions(-1);
		List<List<Tile>> innerRegions = GetRegions (0);

		foreach (List<Tile> outerRegion in outerRegions) {
			if (outerRegion.Count < minOuterRegionSize) {
				foreach (Tile tile in outerRegion) {
					floorMap[tile.x,tile.y] = (int) TileTypes.floor;
				}
			}
		}
		
		foreach (List<Tile> innerRegion in innerRegions) {
			if (innerRegion.Count < minInnerRegionSize) {
				foreach (Tile tile in innerRegion) {
					floorMap[tile.x,tile.y] = (int) TileTypes.outerFloor;
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
					floorMap[x,y] = (int) TileTypes.outerFloor;
				}
				else {
					floorMap[x,y] = (rng.Next(0,100) < fillPercent)? (int) TileTypes.outerFloor: (int) TileTypes.floor;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int surroundingOuterTiles = GetSurroundingTileCount(x, y, (int) TileTypes.outerFloor);

				if (surroundingOuterTiles > 4)
					floorMap[x,y] = (int) TileTypes.outerFloor;
				else if (surroundingOuterTiles < 4)
					floorMap[x,y] = (int) TileTypes.floor;

			}
		}
	}



	List<Tile> GetWalls() {
		wallMap = new int[width, height];
		List<List<Tile>> outerRegions = GetRegions((int) TileTypes.outerFloor);
		Debug.Log(outerRegions.Count);
		List<Tile> walls = new List<Tile>();

		foreach (List<Tile> outerRegion in outerRegions) {
			foreach (Tile tile in outerRegion) {
				List<bool> neighbours = new List<bool>(8);
				for (int neighbourY = tile.y + 1; neighbourY >= tile.y - 1; neighbourY--) {
					for (int neighbourX = tile.x - 1; neighbourX <= tile.x + 1; neighbourX++)  {
						if (IsWithinMap(neighbourX, neighbourY) ) {
							if ((neighbourX != tile.x || neighbourY != tile.y)) {
								if (floorMap[neighbourX, neighbourY] == (int) TileTypes.floor) {
									neighbours.Add(true);
								} else {
									neighbours.Add(false);
								}
							}
						} else {
							neighbours.Add(false);
						}
					}
				}
				
				int wallType = GetWallType(neighbours);
				if (wallType >= 1 && wallType <= 12) {
					Debug.Log(tile.x + "," + tile.y + ":" + string.Join(",", neighbours) + "\n" + (TileTypes) wallType);
					wallMap[tile.x, tile.y] = wallType;
					walls.Add(tile);
				}
			}
		}

		return walls;
	}

	int GetWallType(List<bool> n) {
		//outer
		if (n[1]) {
			if (n[1] && n[2] && n[4] && n[6] && n[7]) {
				return (int) TileTypes.innerWall1;
			} else if (n[0] && n[1] && n[3] && n[5] && n[6]) {
				return (int) TileTypes.innerWall2;
			} else if (n[1] && n[2] && n[4]) {
				return (int) TileTypes.outerWall4;
			} else if (n[1] && n[0] && n[3]) {
				return (int) TileTypes.outerWall3;
			} else if (n[0] && n[1] && n[2]) {
				return (int) TileTypes.outerWall0;
			} else if (n[1] && n[2]) {
				return (int) TileTypes.outerWall1;
			} else if (n[0] && n[1]) {
				return (int) TileTypes.outerWall2;
			} else if (n[1]) {
				return (int) TileTypes.outerWall0;
			}
		} 
		//inner
		else if (n[6]) {
			if (n[3] && n[5] && n[6]) {
				return (int) TileTypes.innerWall3;
			} else if (n[4] && n[6] && n[7]) {
				return (int) TileTypes.innerWall4;
			} else if (n[5] && n[6] && n[7]) {
				return (int) TileTypes.innerWall0;
			} else if (n[6] && n[7]) {
				return (int) TileTypes.innerWall1;
			} else if (n[5] && n[6]) {
				return (int) TileTypes.innerWall2;
			} else if (n[6]) {
				return (int) TileTypes.innerWall0;
			}
		} 
		//sides
		else {
			if (n[2] && n[4] && n[7]) {
				return (int) TileTypes.leftWall;
			} else if (n[0] && n[3] && n[5]) {
				return (int) TileTypes.rightWall;
			} else if (n[7]) {
				return (int) TileTypes.leftWall;
			} else if (n[5]) {
				return (int) TileTypes.rightWall;
			} else if (n[4]) {
				return (int) TileTypes.leftWall;
			} else if (n[3]) {
				return (int) TileTypes.rightWall;
			}
		}
		return (int) TileTypes.outerFloor;
	}

	int GetSurroundingTileCount(int tileX, int tileY, int tileType) {
		int surroundingTileCount = 0;
		for (int neighbourX = tileX - 1; neighbourX <= tileX + 1; neighbourX ++) {
			for (int neighbourY = tileY - 1; neighbourY <= tileY + 1; neighbourY ++) {
                //if within the map
				if (IsWithinMap(neighbourX, neighbourY)) {
                    //if not the tile itself
					if (neighbourX != tileX || neighbourY != tileY) {
                        if (floorMap[neighbourX, neighbourY] == tileType) {
                            surroundingTileCount += 1;
                        }
					}
				}
				else {
					surroundingTileCount ++;
				}
			}
		}
		return surroundingTileCount;
	}

    List<List<Tile>> GetRegions(int tileType) {
		List<List<Tile>> regions = new List<List<Tile>>();
		visited = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (visited[x,y] == 0 && floorMap[x,y] == tileType) {
					List<Tile> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);
				}
			}
		}

		return regions;
	}

	List<Tile> GetRegionTiles(int originX, int originY) {
		List<Tile> region = new List<Tile>();
		int tileType = floorMap[originX, originY];

		Queue<Tile> queue = new Queue<Tile>();
		queue.Enqueue (new Tile(originX, originY));
		visited[originX, originY] = 1;

		while (queue.Count > 0) {
			Tile tile = queue.Dequeue();
			region.Add(tile);

			for (int x = tile.x - 1; x <= tile.x + 1; x++) {
				for (int y = tile.y - 1; y <= tile.y + 1; y++) {
					if (IsWithinMap(x,y) && (y == tile.y || x == tile.x)) {
						if (visited[x,y] == 0 && floorMap[x,y] == tileType) {
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


	void DrawFloorMap() {
		if (floorMap != null) {
			for (int x = 0; x < width; x ++) {
				for (int y = 0; y < height; y ++) {
                    DrawTile(x, y, floorMap);
				}
			}
		}
	}

	void DrawWallMap() {
		List<Tile> walls = GetWalls();
		Debug.Log(walls.Count);
		foreach (Tile wall in walls) {
			DrawTile(wall.x, wall.y, wallMap);
		}
	}

    void DrawTile(int x, int y, int[,] map) {
        TileTypes tileType = (TileTypes) map[x, y];
        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
        GameObject tileToDraw;
        //Debug.Log(tileType);

        switch(tileType) {
            case TileTypes.outerFloor:
                tileToDraw = outerFloor[0];
                break;
            case TileTypes.floor:
                tileToDraw = floor[0];
                break;
            case TileTypes.outerWall0:
                tileToDraw = outerWall0[0];
                break;
            case TileTypes.outerWall1:
                tileToDraw = outerWall1[0];
                break;
			case TileTypes.outerWall2:
                tileToDraw = outerWall2[0];
                break;
			case TileTypes.outerWall3:
                tileToDraw = outerWall3[0];
                break;
			case TileTypes.outerWall4:
                tileToDraw = outerWall4[0];
                break;
            case TileTypes.innerWall0:
                tileToDraw = innerWall0[0];
                break;
            case TileTypes.innerWall1:
                tileToDraw = innerWall1[0];
                break;
			case TileTypes.innerWall2:
                tileToDraw = innerWall2[0];
                break;
			case TileTypes.innerWall3:
                tileToDraw = innerWall3[0];
                break;
			case TileTypes.innerWall4:
                tileToDraw = innerWall4[0];
                break;
            case TileTypes.leftWall:
                tileToDraw = leftWall[0];
                break;
            case TileTypes.rightWall:
                tileToDraw = rightWall[0];
                break;
            default:
                tileToDraw = floor[0];
                break;
        }
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

    public class Tile {
		public int x;
		public int y;

		public Tile(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}
}