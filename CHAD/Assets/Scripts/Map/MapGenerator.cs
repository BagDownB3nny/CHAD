using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

#region TileTypes
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
	water = 13,
	bush = 14,
	tree = 15,


}
#endregion

public class MapGenerator : MonoBehaviour {
	public Text textPrefab;

    [Header("Dimensions")]
	public int width = 50;
	public int height = 50;

	[Header("Floor")]
	[Range(0,100)]
	public int fillPercent = 49;
    public int smoothing = 3;
	public int smoothingRadius = 1;
	public int minOuterRegionSize = 50;
	public int minInnerRegionSize = 50;
	[Header("Branch")]
	public int minBranchRadius = 3;
	public int maxBranchRadius = 5;
	public int branchVariation = 2;
	[Header("River")]
	public int minRiverRadius = 3;
	public int maxRiverRadius = 5;
	public int riverWidthVariation = 2;
	public int riverDirectionVariation = 45;
	[Header("Bush")]
	public float bushThreshold = 0.5f;
	public float bushScale = 1;
	[Header("Tree")]
	public float treeThreshold = 0.2f;
	public float treeScale = 20;

    [Header("Seed")]
	public string seed;
	public bool useRandomSeed;

	[Header("Animation")]
	public bool animationMode = false;
	public float animationInterval = 0.5f;


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
	public GameObject[] water;
	public GameObject[] bush;
	public GameObject[] tree;

	int[,] floorMap;
	int[,] vegetationMap;
	int[,] wallMap;
	int[,] visited;

	void Start() {
		if (animationMode) {
			StartCoroutine(GenerateMapWithAnimation());
		} else {
			GenerateMap();
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
            ClearMap();
			if (animationMode) {
				StartCoroutine(GenerateMapWithAnimation());
			} else {
				GenerateMap();
			}
		}
	}

	#region GenerateMap
	void GenerateMap() {
		floorMap = new int[width,height];
		
		RandomFillMap();

		for (int i = 0; i < smoothing; i ++) {
			SmoothMap();
		}

		burstSmallRegions();

		List<Region> finalRegions = GetRegions((int) TileTypes.floor);
		finalRegions.Sort();
		finalRegions[0].isMainRegion = true;
		finalRegions[0].isConnectedToMainRegion = true;

		ConnectClosestRegions(finalRegions, false);

		SmoothMap();

		FillWater();

		DrawFloorMap();

		DrawVegetation(TileTypes.bush, bushScale, bushThreshold);

		DrawVegetation(TileTypes.tree, treeScale, treeThreshold);
		
		DrawWallMap();
	}

	IEnumerator GenerateMapWithAnimation() {
		floorMap = new int[width,height];
		

		RandomFillMap();

		DrawFloorMap();
		yield return new WaitForSeconds(animationInterval);

		for (int i = 0; i < smoothing; i ++) {
			SmoothMap();

			ClearMap();
			DrawFloorMap();
			yield return new WaitForSeconds(animationInterval);
		}

		burstSmallRegions();

		ClearMap();
		DrawFloorMap();
		yield return new WaitForSeconds(1);

		List<Region> finalRegions = GetRegions((int) TileTypes.floor);
		finalRegions.Sort();
		finalRegions[0].isMainRegion = true;
		finalRegions[0].isConnectedToMainRegion = true;

		ConnectClosestRegions(finalRegions, false);

		ClearMap();
		DrawFloorMap();
		yield return new WaitForSeconds(animationInterval);

		SmoothMap();

		ClearMap();
		DrawFloorMap();
		yield return new WaitForSeconds(animationInterval);

		FillWater();

		ClearMap();
		DrawFloorMap();
		yield return new WaitForSeconds(animationInterval);

		DrawVegetation(TileTypes.bush, bushScale, bushThreshold);

		yield return new WaitForSeconds(animationInterval);

		DrawVegetation(TileTypes.tree, treeScale, treeThreshold);

		yield return new WaitForSeconds(animationInterval);

		DrawWallMap();
	}
	#endregion

	#region BurstSmallRegions
	void burstSmallRegions() {
		List<Region> outerRegions = GetRegions(-1);
		List<Region> innerRegions = GetRegions(0);

		foreach (Region outerRegion in outerRegions) {
			if (outerRegion.tiles.Count < minOuterRegionSize) {
				foreach (Tile tile in outerRegion.tiles) {
					floorMap[tile.x,tile.y] = (int) TileTypes.floor;
				}
			}
		}
		
		foreach (Region innerRegion in innerRegions) {
			if (innerRegion.tiles.Count < minInnerRegionSize) {
				foreach (Tile tile in innerRegion.tiles) {
					floorMap[tile.x,tile.y] = (int) TileTypes.outerFloor;
				}
			}
		}
	}
	#endregion

	#region RandomFillMap
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
	#endregion

	#region SmoothMap
	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int surroundingOuterTiles = GetSurroundingTileCount(x, y, smoothingRadius, (int) TileTypes.outerFloor);

				int diameter = (smoothingRadius * 2) + 1;
				int cutOff = ((diameter * diameter) - 1) / 2;

				if (surroundingOuterTiles > cutOff) {
					floorMap[x,y] = (int) TileTypes.outerFloor;
				} else if (surroundingOuterTiles < cutOff) {
					floorMap[x,y] = (int) TileTypes.floor;
				}
			}
		}
	}

	int GetSurroundingTileCount(int x, int y, int _radius, int _tileType) {
		int surroundingTileCount = 0;
		for (int neighbourX = x - _radius; neighbourX <= x + _radius; neighbourX ++) {
			for (int neighbourY = y - _radius; neighbourY <= y + _radius; neighbourY ++) {
                //if within the map
				if (IsWithinMap(neighbourX, neighbourY)) {
                    //if not the tile itself
					if (neighbourX != x || neighbourY != y) {
                        if (floorMap[neighbourX, neighbourY] == _tileType) {
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
	#endregion

	#region Walls
	void DrawWallMap() {
		List<Tile> walls = GetWalls();
		foreach (Tile wall in walls) {
			DrawTile(wall.x, wall.y, 0, wallMap);
		}
	}

	List<Tile> GetWalls() {
		wallMap = new int[width, height];
		List<Region> outerRegions = GetRegions((int) TileTypes.outerFloor);
		List<Tile> walls = new List<Tile>();
		List<Tile> outerWalls = new List<Tile>();

		foreach (Region outerRegion in outerRegions) {
			foreach (Tile tile in outerRegion.edgeTiles) {
				List<bool> neighbours = new List<bool>(8);
				for (int neighbourY = tile.y + 1; neighbourY >= tile.y - 1; neighbourY--) {
					for (int neighbourX = tile.x - 1; neighbourX <= tile.x + 1; neighbourX++)  {
						if (IsWithinMap(neighbourX, neighbourY) ) {
							if ((neighbourX != tile.x || neighbourY != tile.y)) {
								if (floorMap[neighbourX, neighbourY] != (int) TileTypes.outerFloor) {
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
				if (wallType != (int) TileTypes.outerFloor) {
					//if is an outerwall, add to outerWalls list
					if (wallType == (int) TileTypes.outerWall0 || wallType == (int) TileTypes.outerWall1 ||
							wallType == (int) TileTypes.outerWall2 || wallType == (int) TileTypes.outerWall3 ||
							wallType == (int) TileTypes.outerWall4) {
								outerWalls.Add(new Tile(tile.x, tile.y));
							}
					wallMap[tile.x, tile.y] = wallType;
					walls.Add(tile);
				}
			}
		}

		//add an additional floor below the outerwalls
		foreach(Tile tile in outerWalls) {
			floorMap[tile.x, tile.y] = (int) TileTypes.floor;
			DrawTile(tile.x, tile.y, 0, floorMap);
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
	#endregion

	#region ConnectRegions
	void ConnectClosestRegions(List<Region> regions, bool forceConnectionToMainRegion = false) {
		int minDist = 0;
		Tile bestStartTile = new Tile();
		Tile bestEndTile = new Tile();
		Region bestStartRoom = new Region(TileTypes.floor);
		Region bestEndRoom = new Region(TileTypes.floor);
		bool possibleConnectionFound = false;

		List<Region> notConnectedToMainRegion = new List<Region> ();
		List<Region> connectedToMainRegion = new List<Region> ();
		
		if (forceConnectionToMainRegion) {
			foreach (Region region in regions) {
				if (region.isConnectedToMainRegion) {
					connectedToMainRegion.Add (region);
				} else {
					notConnectedToMainRegion.Add (region);
				}
			}
		} else {
			notConnectedToMainRegion = regions;
			connectedToMainRegion = regions;
		}

		foreach (Region startRegion in notConnectedToMainRegion) {
			if (!forceConnectionToMainRegion) {
				possibleConnectionFound = false;
				if (startRegion.connectedRegions.Count > 0) {
					continue;
				}
			}
			

			foreach (Region endRegion in connectedToMainRegion) {
				if (startRegion == endRegion || startRegion.IsConnected(endRegion)) {
					continue;
				}

				for (int startTileIndex = 0; startTileIndex < startRegion.edgeTiles.Count; startTileIndex ++) {
					for (int endTileIndex = 0; endTileIndex < endRegion.edgeTiles.Count; endTileIndex ++) {
						Tile startTile = startRegion.edgeTiles[startTileIndex];
						Tile endTile = endRegion.edgeTiles[endTileIndex];
						int dist = (int)(Mathf.Pow(startTile.x - endTile.x, 2) + Mathf.Pow(startTile.y - endTile.y, 2));

						if (dist < minDist || !possibleConnectionFound) {
							minDist = dist;
							possibleConnectionFound = true;
							bestStartTile = startTile;
							bestEndTile = endTile;
							bestStartRoom = startRegion;
							bestEndRoom = endRegion;
						}
					}
				}
			}
			if (possibleConnectionFound && !forceConnectionToMainRegion) {
				CreateBranch(bestStartRoom, bestEndRoom, bestStartTile, bestEndTile);
			}
		}
		if (possibleConnectionFound && forceConnectionToMainRegion) {
			CreateBranch(bestStartRoom, bestEndRoom, bestStartTile, bestEndTile);
		}
		
		if (!forceConnectionToMainRegion) {
			ConnectClosestRegions(regions, true);
		}
	}

	void CreateBranch(Region startRoom, Region endRoom, Tile startTile, Tile endTile) {
		Region.ConnectRooms(startRoom, endRoom);

		List<Tile> lineTiles = GetLine(startTile, endTile);
		FillCircle(lineTiles, TileTypes.floor, minBranchRadius, maxBranchRadius, branchVariation);
	}

	List<Tile> GetLine(Tile start, Tile end) {
		List<Tile> line = new List<Tile> ();

		int x = start.x;
		int y = start.y;

		int dx = end.x - start.x;
		int dy = end.y - start.y;

		bool inverted = false;
		int step = Math.Sign (dx);
		int gradientStep = Math.Sign (dy);

		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs(dy);
			shortest = Mathf.Abs(dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0; i < longest; i ++) {
			line.Add(new Tile(x,y));

			if (inverted) {
				y += step;
			}
			else {
				x += step;
			}

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest) {
				if (inverted) {
					x += gradientStep;
				}
				else {
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}

		return line;
	}

	void FillCircle(List<Tile> tiles, TileTypes tileType, int minRadius, int maxRadius, int variation) {
		System.Random rng = new System.Random(seed.GetHashCode());
		int prevRadius = rng.Next(minRadius, maxRadius);

		foreach (Tile tile in tiles) {
			int radius = rng.Next((prevRadius - variation < minRadius) ? minRadius : prevRadius - variation, 
						(prevRadius + variation > maxRadius) ? maxRadius : prevRadius + variation);

			for (int x = -radius; x <= radius; x++) {
				for (int y = -radius; y <= radius; y++) {
					if ((x * x) + (y * y) <= (radius * radius)) {
						int circleX = tile.x + x;
						int circleY = tile.y + y;
						if (IsWithinMap(circleX, circleY)) {
							floorMap[circleX,circleY] = (int) tileType;
						}
					}
				}
			}
		}
	}

	
	#endregion

	#region Water
	void FillWater() {
		System.Random rng = new System.Random(seed.GetHashCode());

		List<Tile> floorTiles = GetRegions((int) TileTypes.floor)[0].tiles;

		Tile startTile = floorTiles[rng.Next(0, floorTiles.Count)];

		int startDirection = rng.Next(0, 359);
		int oppDirection = (startDirection + 180) % 360;

		List<Tile> riverLineTiles = GetWaterLineTiles(startTile, startDirection, new List<Tile>());
		riverLineTiles.AddRange(GetWaterLineTiles(startTile, oppDirection, new List<Tile>()));

		List<Tile> riverTiles = GetWaterTiles(riverLineTiles);
	}

	List<Tile> GetWaterLineTiles(Tile tile, int prevDirection, List<Tile> riverTiles) {
		if (tile == null) {
			return riverTiles;
		}

		riverTiles.Add(tile);

		System.Random rng = new System.Random(seed.GetHashCode());
		int direction = (rng.Next(prevDirection - riverDirectionVariation, prevDirection + riverDirectionVariation) + 360) % 360;
		int quadrant = direction / 45;
		Tile nextTile = null;

		for (int y = tile.y + 1; y >= tile.y - 1; y--) {
			for (int x = tile.x - 1; x <= tile.x + 1; x++)  {
				quadrant--;
				if (quadrant < 0) {
					if (IsWithinMap(x, y) && floorMap[x, y] != (int) TileTypes.outerFloor) {
						nextTile = new Tile(x, y);
					}
					goto AfterLoop;
				}
			}
		}
		AfterLoop:

		return GetWaterLineTiles(nextTile, direction, riverTiles);
	}

	List<Tile> GetWaterTiles(List<Tile> riverLineTiles) {
		List<Tile> finalRiverTiles = new List<Tile>();

		System.Random rng = new System.Random(seed.GetHashCode());
		int prevRadius = rng.Next(minRiverRadius, maxRiverRadius);

		foreach (Tile tile in riverLineTiles) {
			int radius = rng.Next((prevRadius - riverWidthVariation < minRiverRadius) ? minRiverRadius : prevRadius - riverWidthVariation, 
						(prevRadius + riverWidthVariation > maxRiverRadius) ? maxRiverRadius : prevRadius + riverWidthVariation);

			for (int x = -radius; x <= radius; x++) {
				for (int y = -radius; y <= radius; y++) {
					if ((x * x) + (y * y) <= (radius * radius)) {
						int circleX = tile.x + x;
						int circleY = tile.y + y;
						if (IsWithinMap(circleX, circleY) && floorMap[circleX, circleY] == (int) TileTypes.floor) {
							finalRiverTiles.Add(new Tile(circleX, circleY));
							floorMap[circleX, circleY] = (int) TileTypes.water;
						}
					}
				}
			}
		}
		return finalRiverTiles;
	}
	#endregion

	#region Vegetation
	void DrawVegetation(TileTypes tileType, float vegetationScale, float vegetationThreshold) {
		vegetationMap = new int[width, height];
		List<Tile> vegetations = GetVegetation(tileType, vegetationScale, vegetationThreshold);
		foreach(Tile vegetation in vegetations) {
			DrawTile(vegetation.x, vegetation.y, 0, vegetationMap);
		}
	}

	List<Tile> GetVegetation(TileTypes tileType, float vegetationScale, float vegetationThreshold) {
		System.Random rng = new System.Random(seed.GetHashCode());
		int randomOffset = rng.Next(0,100);
		int[,] perlinNoise = new int[width, height];
		List<Tile> vegetations = new List<Tile>();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++)
			{
				float perlin = Mathf.PerlinNoise(((float) x / width) * vegetationScale + randomOffset, ((float) y / height) * vegetationScale + randomOffset);
				if (perlin < vegetationThreshold) {
					if (floorMap[x, y] != (int) TileTypes.outerFloor && floorMap[x, y] != (int) TileTypes.water) {
						vegetationMap[x, y] = (int) tileType;
						vegetations.Add(new Tile(x, y));
					}
				}
			}
		}
		return vegetations;
	}

	#endregion

	#region GetRegion
    List<Region> GetRegions(int tileType) {
		List<Region> regions = new List<Region>();
		visited = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (visited[x,y] == 0 && floorMap[x,y] == tileType) {
					Region newRegion = new Region((TileTypes) tileType, GetRegionTiles(x,y), floorMap, width, height);
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
	#endregion

	#region Draw
	void DrawFloorMap() {
		if (floorMap != null) {
			for (int x = 0; x < width; x ++) {
				for (int y = 0; y < height; y ++) {
                    DrawTile(x, y, 0, floorMap);
				}
			}
		}
	}

    void DrawTile(int x, int y, int id, int[,] map) {
        TileTypes tileType = (TileTypes) map[x, y];
        Vector3 pos = CoordToWorldPoint(x, y);
        GameObject tileToDraw;

        switch(tileType) {
            case TileTypes.outerFloor:
                tileToDraw = outerFloor[id];
                break;
            case TileTypes.floor:
                tileToDraw = floor[id];
                break;
            case TileTypes.outerWall0:
                tileToDraw = outerWall0[id];
                break;
            case TileTypes.outerWall1:
                tileToDraw = outerWall1[id];
                break;
			case TileTypes.outerWall2:
                tileToDraw = outerWall2[id];
                break;
			case TileTypes.outerWall3:
                tileToDraw = outerWall3[id];
                break;
			case TileTypes.outerWall4:
                tileToDraw = outerWall4[id];
                break;
            case TileTypes.innerWall0:
                tileToDraw = innerWall0[id];
                break;
            case TileTypes.innerWall1:
                tileToDraw = innerWall1[id];
                break;
			case TileTypes.innerWall2:
                tileToDraw = innerWall2[id];
                break;
			case TileTypes.innerWall3:
                tileToDraw = innerWall3[id];
                break;
			case TileTypes.innerWall4:
                tileToDraw = innerWall4[id];
                break;
            case TileTypes.leftWall:
                tileToDraw = leftWall[id];
                break;
            case TileTypes.rightWall:
                tileToDraw = rightWall[id];
                break;
			case TileTypes.water:
                tileToDraw = water[id];
                break;
			case TileTypes.bush:
                tileToDraw = bush[id];
                break;
			case TileTypes.tree:
                tileToDraw = tree[id];
                break;
            default:
                tileToDraw = floor[0];
                break;
        }
        Instantiate(tileToDraw, pos, Quaternion.identity);
    }
	#endregion

	bool IsWithinMap(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	Vector3 CoordToWorldPoint(int x, int y) {
		return new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
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

		public Tile(){}
		public Tile(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}

	class Region : IComparable<Region> {
		public TileTypes tileType;
		public List<Tile> tiles;
		public List<Tile> edgeTiles;
		public List<Region> connectedRegions;
		public int regionSize;
		public bool isMainRegion = false;
		public bool isConnectedToMainRegion = false;

		public Region(TileTypes _tileType) {
			tileType = _tileType;
		}

		public Region(TileTypes _tileType, List<Tile> _tiles, int[,] floorMap, int width, int height) {
			tileType = _tileType;
			tiles = _tiles;
			regionSize = tiles.Count;
			connectedRegions = new List<Region>();

			edgeTiles = new List<Tile>();
			foreach (Tile tile in tiles) {
				for (int x = tile.x - 1; x <= tile.x + 1; x++) {
					for (int y = tile.y - 1; y <= tile.y + 1; y++) {
						if (x >= 0 && x < width && y >= 0 && y < height) {
							//exclude diagonals
							if (tileType == TileTypes.floor) {
								if (x == tile.x || y == tile.y) {
									if (floorMap[x,y] != (int) TileTypes.floor) {
										edgeTiles.Add(tile);
									}
								}
							} 
							//include diagonals
							else {
								if (floorMap[x,y] != (int) TileTypes.outerFloor) {
									edgeTiles.Add(tile);
								}
							}
						}
					}
				}
			}
		}

		public void SetConnectedToMainRegion() {
			if (!isConnectedToMainRegion) {
				isConnectedToMainRegion = true;
				foreach(Region region in connectedRegions) {
					region.isConnectedToMainRegion = true;
				}
			}
		}

		public static void ConnectRooms(Region regionA, Region regionB) {
			if (regionA.isConnectedToMainRegion) {
				regionB.SetConnectedToMainRegion();
			} else if (regionB.isConnectedToMainRegion) {
				regionA.SetConnectedToMainRegion();
			}
			regionA.connectedRegions.Add (regionB);
			regionB.connectedRegions.Add (regionA);
		}

		public bool IsConnected(Region otherRegion) {
			return connectedRegions.Contains(otherRegion);
		}

		public int CompareTo(Region otherRegion) {
			return otherRegion.regionSize.CompareTo(regionSize);
		}
	}
}