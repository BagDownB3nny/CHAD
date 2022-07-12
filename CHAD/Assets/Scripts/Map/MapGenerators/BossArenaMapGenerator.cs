using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BossArenaMapGenerator : MapGenerator {

    #region SquareTypes
    public enum SquareTypes {
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
        cliff = 16,
        playerSpawner = 17,
        enemySpawner = 18,
        exit = 19,
        liliypad = 20,
    }
    #endregion

	[Header("Floor")]
    private float a;
    private float b;
    private float c;

	[Header("Branch")]
	public int branchRadius = 3;

	[Header("River")]
	public int minRiverRadius = 3;
	public int maxRiverRadius = 5;
	public int riverWidthVariation = 2;
	public int riverDirectionVariation = 45;
    
	[Header("Tree")]
	public float treeThreshold = 0.2f;
	public float treeScale = 20;

    [Header("Seed")]
	public string seed;

    [Header("Squares")]
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
    public GameObject[] lilypad;
	public GameObject[] bush;
	public GameObject[] tree;
	public GameObject[] cliff;
	public GameObject[] playerSpawner;
	public GameObject[] enemySpawner;
	public GameObject[] exit;

	int[,] vegetationMap;
	int[,] wallMap;
	int[,] cliffMap;
	int[,] visited;
    List<Square> hexagonPoints;
	Square playerSpawnerSquare;
	Square exitSquare;
	List<GameObject> mapObjects = new List<GameObject>();

    // void Update() {
    // 	if (Input.GetMouseButtonDown(0)) {
    //         ClearMap();
    //         seed = Time.time.ToString();
    // 	    GenerateMap(seed);
    // 	}
    // }

    #region GenerateMap
    public override void GenerateMap(string _seed) {
        Camera.main.backgroundColor = new Color32(122, 176, 85, 255);
        height = width;
        floorMap = new int[width, height];
        vegetationMap = new int[width, height];

        //fill floorMap with outerFloor
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                floorMap[x, y] = (int) SquareTypes.outerFloor;
                vegetationMap[x, y] = (int) SquareTypes.outerFloor;
            }
        }


        FillHexOutline();

        FillHexagons();

		FillWater();

        FillHexInline();

		DrawFloorMap();

        DrawVegetationMap();

		DrawPlayerSpawner();

		DrawEnemySpawner();

		DrawVegetation(SquareTypes.tree, treeScale, treeThreshold);
		
		DrawWallMap();

		if (NetworkManager.gameType == GameType.Client)
		{
			SendMapLoaded();
		}
	}


	public void SendMapLoaded() {
		ClientSend.MapLoaded();
	}
	#endregion

    #region Floor

    private void FillHexOutline() {
        b = (float) MathF.Floor((width - 3) / 6);
        a = b / MathF.Sqrt(3);
        c = a * 2;

        float[,] points = {{b, a + c}, {2*b, c}, {2*b, 0}, {3*b, -a}, {3*b, -a-c}, {2*b, -2*a-c}, {2*b, -2*a-2*c},
                {b, -3*a-2*c}, {0, -2*a-2*c}, {-b, -3*a-2*c}, {-2*b, -2*a-2*c}, {-2*b, -2*a-c}, {-3*b, -a-c}, {-3*b, -a},
                {-2*b, 0}, {-2*b, c}, {-b, a+c}, {0, c}, {0, 0}, {b, -a}, {b, -a-c}, {0, -2*a-c}, {-b, -a-c}, {-b, -a}};

        //adjust center back to origin (+1 to create a border)
        for (int i = 0; i < points.GetLength(0); i++) {
            points[i, 0] += (3*b + 1);
            points[i, 1] += (3*a + 2*c + 1);
        }

        hexagonPoints = new List<Square>();

        //convert coords into list of squares
        for (int i = 0; i < points.GetLength(0); i++) {
            hexagonPoints.Add(new Square((int) MathF.Round(points[i, 0]), (int) MathF.Round(points[i, 1])));
        }

        //fill hexagon lines
        for (int i = 0; i < hexagonPoints.Count - 1; i++) {
            CreateBranch(hexagonPoints[i], hexagonPoints[i + 1], SquareTypes.floor, floorMap);
        }

        int[,] lines = {{23, 18}, {0, 17}, {14, 23}, {11, 22}, {8, 21}, {5, 20}, {2, 19}};

        for (int i = 0; i < lines.GetLength(0); i++) {
            CreateBranch(hexagonPoints[lines[i, 0]], hexagonPoints[lines[i, 1]], SquareTypes.floor, floorMap);
        }
    }

    private void FillHexagons() {
        List<Region> emptyRegions = GetRegions((int) SquareTypes.outerFloor);
        emptyRegions.RemoveAt(0);
        foreach (Region region in emptyRegions) {
            foreach (Square square in region.squares) {
                floorMap[square.x, square.y] = (int) SquareTypes.floor;
            }
        }
    }

    private void FillHexInline() {
        int[,] lines = {{17, 18}, {2, 19}, {5, 20}, {8, 21}, {11, 22}, {14, 23}, 
                {18, 19}, {19, 20}, {21, 20}, {21, 22}, {22, 23}, {18, 23}};

        for (int i = 0; i < lines.GetLength(0); i++) {
            CreateBranch(hexagonPoints[lines[i, 0]], hexagonPoints[lines[i, 1]], SquareTypes.bush, vegetationMap);
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (vegetationMap[x, y] == (int) SquareTypes.bush && floorMap[x, y] == (int) SquareTypes.water) {
                    vegetationMap[x, y] = (int) SquareTypes.liliypad;
                }
            }
        }
    }
    #endregion

	#region Walls
	void DrawWallMap() {
		List<Square> walls = GetWalls();
		foreach (Square wall in walls) {
			DrawSquare(wall.x, wall.y, 0, wallMap);
		}
	}

	List<Square> GetWalls() {
		wallMap = new int[width, height];
		List<Region> outerRegions = GetRegions((int) SquareTypes.outerFloor);
		List<Square> walls = new List<Square>();
		List<Square> outerWalls = new List<Square>();

		foreach (Region outerRegion in outerRegions) {
			foreach (Square square in outerRegion.edgeSquares) {
				List<bool> neighbours = new List<bool>(8);
				for (int neighbourY = square.y + 1; neighbourY >= square.y - 1; neighbourY--) {
					for (int neighbourX = square.x - 1; neighbourX <= square.x + 1; neighbourX++)  {
						if (IsWithinMap(neighbourX, neighbourY) ) {
							if ((neighbourX != square.x || neighbourY != square.y)) {
								if (floorMap[neighbourX, neighbourY] != (int) SquareTypes.outerFloor) {
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
				if (wallType != (int) SquareTypes.outerFloor) {
					//if is an outerwall, add to outerWalls list
					if (wallType == (int) SquareTypes.outerWall0 || wallType == (int) SquareTypes.outerWall1 ||
							wallType == (int) SquareTypes.outerWall2 || wallType == (int) SquareTypes.outerWall3 ||
							wallType == (int) SquareTypes.outerWall4) {
								outerWalls.Add(new Square(square.x, square.y));
							}
					wallMap[square.x, square.y] = wallType;
					walls.Add(square);
				}
			}
		}

		// List<Square> cliffStart = new List<Square>();
		// //add an additional floor below the outerwalls and cliff
		// foreach(Square square in outerWalls) {
		// 	// floorMap[square.x, square.y] = (int) SquareTypes.floor;
		// 	// DrawSquare(square.x, square.y, 0, floorMap);

		// 	cliffStart.Add(new Square(square.x, square.y - 1));
		// }

		// DrawCliff(cliffStart);

		return walls;
	}

	int GetWallType(List<bool> n) {
		//outer
		if (n[1]) {
			if (n[1] && n[2] && n[4] && n[6] && n[7]) {
				return (int) SquareTypes.innerWall1;
			} else if (n[0] && n[1] && n[3] && n[5] && n[6]) {
				return (int) SquareTypes.innerWall2;
			} else if (n[1] && n[2] && n[4]) {
				return (int) SquareTypes.outerWall4;
			} else if (n[1] && n[0] && n[3]) {
				return (int) SquareTypes.outerWall3;
			} else if (n[0] && n[1] && n[2]) {
				return (int) SquareTypes.outerWall0;
			} else if (n[1] && n[2]) {
				return (int) SquareTypes.outerWall1;
			} else if (n[0] && n[1]) {
				return (int) SquareTypes.outerWall2;
			} else if (n[1]) {
				return (int) SquareTypes.outerWall0;
			}
		} 
		//inner
		else if (n[6]) {
			if (n[3] && n[5] && n[6]) {
				return (int) SquareTypes.innerWall3;
			} else if (n[4] && n[6] && n[7]) {
				return (int) SquareTypes.innerWall4;
			} else if (n[5] && n[6] && n[7]) {
				return (int) SquareTypes.innerWall0;
			} else if (n[6] && n[7]) {
				return (int) SquareTypes.innerWall1;
			} else if (n[5] && n[6]) {
				return (int) SquareTypes.innerWall2;
			} else if (n[6]) {
				return (int) SquareTypes.innerWall0;
			}
		} 
		//sides
		else {
			if (n[2] && n[4] && n[7]) {
				return (int) SquareTypes.leftWall;
			} else if (n[0] && n[3] && n[5]) {
				return (int) SquareTypes.rightWall;
			} else if (n[7]) {
				return (int) SquareTypes.leftWall;
			} else if (n[5]) {
				return (int) SquareTypes.rightWall;
			} else if (n[4]) {
				return (int) SquareTypes.leftWall;
			} else if (n[3]) {
				return (int) SquareTypes.rightWall;
			}
		}
		return (int) SquareTypes.outerFloor;
	}
	#endregion

	#region Cliff
	void DrawCliff(List<Square> cliffStart) {
		cliffStart.Sort();
		List<List<GameObject>> columns = new List<List<GameObject>>();
		List<GameObject> columnGroup = new List<GameObject>();
		int prevStartHeight = -1;

		foreach (Square square in cliffStart) {
			if (square.y != prevStartHeight) {
				prevStartHeight = square.y;
				if (columnGroup.Count > 0) {
					columns.Add(columnGroup);
				}
				columnGroup = new List<GameObject>();
			}

			for (int y = square.y; y >= -10; y--) {
				if (!IsWithinMap(square.x, y) || floorMap[square.x, y] == (int) SquareTypes.outerFloor) {
					GameObject cliffSquare = Instantiate(cliff[0], CoordToWorldPoint(square.x, y), Quaternion.identity);
					mapObjects.Add(cliffSquare);
					columnGroup.Add(cliffSquare);
				} else {
					break;
				}
			}
		}

		if (columnGroup.Count > 0) {
			columns.Add(columnGroup);
		}

		int layerCount = columns.Count;
		float colourStep = 0.9f / layerCount;

		for (int i = 0; i < layerCount; i++) {
			List<GameObject> group = columns[i];
			foreach(GameObject square in group) {
				SpriteRenderer sprite = square.GetComponent<SpriteRenderer>();
				float rgb = 1 - (i * colourStep);
				sprite.color = new Color(rgb, rgb, rgb, 1);
				sprite.sortingOrder = (int) (-5000 - i);
			}
		}
	}
	#endregion

	#region ConnectRegions

	void CreateBranch(Square startSquare, Square endSquare, SquareTypes squareType, int[,] map) {
		List<Square> lineSquares = GetLine(startSquare, endSquare);

        foreach (Square square in lineSquares) {
		    map[square.x, square.y] = (int) squareType;
		}
	}

	List<Square> GetLine(Square start, Square end) {
		List<Square> line = new List<Square> ();

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
			line.Add(new Square(x,y));

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
	#endregion

	#region Water
	void FillWater() {
		System.Random rng = new System.Random(seed.GetHashCode());

		List<Square> floorSquares = GetRegions((int) SquareTypes.floor)[0].squares;

		Square startSquare = floorSquares[rng.Next(0, floorSquares.Count)];

		int startDirection = rng.Next(0, 359);
		int oppDirection = (startDirection + 180) % 360;

		List<Square> riverLineSquares = GetWaterLineSquares(startSquare, startDirection, new List<Square>());
		riverLineSquares.AddRange(GetWaterLineSquares(startSquare, oppDirection, new List<Square>()));

		List<Square> riverSquares = GetWaterSquares(riverLineSquares);
	}

	List<Square> GetWaterLineSquares(Square square, int prevDirection, List<Square> riverSquares) {
		if (square == null) {
			return riverSquares;
		}

		riverSquares.Add(square);

		System.Random rng = new System.Random(seed.GetHashCode());
		int direction = (rng.Next(prevDirection - riverDirectionVariation, prevDirection + riverDirectionVariation) + 360) % 360;
		int quadrant = direction / 45;
		Square nextSquare = null;

		for (int y = square.y + 1; y >= square.y - 1; y--) {
			for (int x = square.x - 1; x <= square.x + 1; x++)  {
				quadrant--;
				if (quadrant < 0) {
					if (IsWithinMap(x, y) && floorMap[x, y] != (int) SquareTypes.outerFloor) {
						nextSquare = new Square(x, y);
					}
					goto AfterLoop;
				}
			}
		}
		AfterLoop:

		return GetWaterLineSquares(nextSquare, direction, riverSquares);
	}

	List<Square> GetWaterSquares(List<Square> riverLineSquares) {
		List<Square> finalRiverSquares = new List<Square>();

		System.Random rng = new System.Random(seed.GetHashCode());
		int prevRadius = rng.Next(minRiverRadius, maxRiverRadius);

		foreach (Square square in riverLineSquares) {
			int radius = rng.Next((prevRadius - riverWidthVariation < minRiverRadius) ? minRiverRadius : prevRadius - riverWidthVariation, 
						(prevRadius + riverWidthVariation > maxRiverRadius) ? maxRiverRadius : prevRadius + riverWidthVariation);

			for (int x = -radius; x <= radius; x++) {
				for (int y = -radius; y <= radius; y++) {
					if ((x * x) + (y * y) <= (radius * radius)) {
						int circleX = square.x + x;
						int circleY = square.y + y;
						if (IsWithinMap(circleX, circleY) && floorMap[circleX, circleY] == (int) SquareTypes.floor) {
							finalRiverSquares.Add(new Square(circleX, circleY));
							floorMap[circleX, circleY] = (int) SquareTypes.water;
						}
					}
				}
			}
		}
		return finalRiverSquares;
	}
	#endregion

	#region Vegetation
	void DrawVegetation(SquareTypes squareType, float vegetationScale, float vegetationThreshold) {
		vegetationMap = new int[width, height];
		List<Square> vegetations = GetVegetation(squareType, vegetationScale, vegetationThreshold);
		foreach(Square vegetation in vegetations) {
			DrawSquare(vegetation.x, vegetation.y, 0, vegetationMap);
		}
	}

	List<Square> GetVegetation(SquareTypes squareType, float vegetationScale, float vegetationThreshold) {
		System.Random rng = new System.Random(seed.GetHashCode());
		int randomOffset = rng.Next(0,100);
		int[,] perlinNoise = new int[width, height];
		List<Square> vegetations = new List<Square>();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++)
			{
				if ((x != playerSpawnerSquare.x && y != playerSpawnerSquare.y) && (x != exitSquare.y && y != exitSquare.y)) {
					float perlin = Mathf.PerlinNoise(((float) x / width) * vegetationScale + randomOffset, ((float) y / height) * vegetationScale + randomOffset);
					if (perlin < vegetationThreshold) {
						if (floorMap[x, y] != (int) SquareTypes.outerFloor && floorMap[x, y] != (int) SquareTypes.water) {
							vegetationMap[x, y] = (int) squareType;
							vegetations.Add(new Square(x, y));
						}
					}
				}
			}
		}
		return vegetations;
	}

	#endregion

	#region PlayerSpawner
	void DrawPlayerSpawner() {
		List<Region> floors = GetRegions((int) SquareTypes.floor);
		List<Square> floorSquares = new List<Square>();
		foreach (Region region in floors) {
			floorSquares.AddRange(region.squares);
		}
		Region floor = new Region(SquareTypes.floor, floorSquares, floorMap, width, height);
		playerSpawnerSquare = GetMostIsolatedSquare(floor);

		Vector3 position = CoordToWorldPoint(playerSpawnerSquare.x, playerSpawnerSquare.y);
		GameObject spawner = Instantiate(playerSpawner[0], position, Quaternion.identity);
		mapObjects.Add(spawner);

		DrawExit(playerSpawnerSquare, floor);
	}

	Square GetMostIsolatedSquare(Region region) {
		List<Square> edgeSquares = region.edgeSquares;
		List<Square> squares = region.squares;
		float largestMinDist = float.MinValue;
		Square mostIsolatedSquare = new Square(0, 0);

		foreach(Square square in squares) {
			float squareMinDist = float.MaxValue;
			
			foreach(Square edgeSquare in edgeSquares) {
				int dx = edgeSquare.x - square.x;
				int dy = edgeSquare.y - square.y;
				float dist = dx * dx + dy * dy;

				if (dist < squareMinDist) {
					squareMinDist = dist;
				}
			}
			if (squareMinDist > largestMinDist) {
				largestMinDist = squareMinDist;
				mostIsolatedSquare = square;
			}
		}

		return mostIsolatedSquare;
	}

	#endregion

	#region hole
	void DrawExit(Square playerSpawnerSquare, Region floorSquares) {
		float largestDist = float.MinValue;

		foreach (Square edgeSquare in floorSquares.edgeSquares) {
			int dx = edgeSquare.x - playerSpawnerSquare.x;
				int dy = edgeSquare.y - playerSpawnerSquare.y;
				float dist = dx * dx + dy * dy;

			if (dist > largestDist) {
				largestDist = dist;
				exitSquare = edgeSquare;
			}
		}

		Vector3 position = CoordToWorldPoint(exitSquare.x, exitSquare.y);
		GameObject hole = Instantiate(exit[0], position, Quaternion.identity);
		mapObjects.Add(hole);
	}
	#endregion

	#region EnemySpawner
	void DrawEnemySpawner() {
		GameObject spawner = Instantiate(enemySpawner[0], new Vector3(0, 0, 0), Quaternion.identity);
		mapObjects.Add(spawner);
		GameManager.instance.enemySpawners.Add("ES", spawner);
		spawner.GetComponent<EnemySpawner>().map = gameObject;
	}

	#endregion

	#region GetRegion
    List<Region> GetRegions(int squareType) {
		List<Region> regions = new List<Region>();
		visited = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (visited[x,y] == 0 && floorMap[x,y] == squareType) {
					Region newRegion = new Region((SquareTypes) squareType, GetRegionSquares(x,y), floorMap, width, height);
					regions.Add(newRegion);
				}
			}
		}

		return regions;
	}

	List<Square> GetRegionSquares(int originX, int originY) {
		List<Square> region = new List<Square>();
		int squareType = floorMap[originX, originY];

		Queue<Square> queue = new Queue<Square>();
		queue.Enqueue (new Square(originX, originY));
		visited[originX, originY] = 1;

		while (queue.Count > 0) {
			Square square = queue.Dequeue();
			region.Add(square);

			for (int x = square.x - 1; x <= square.x + 1; x++) {
				for (int y = square.y - 1; y <= square.y + 1; y++) {
					if (IsWithinMap(x,y) && (y == square.y || x == square.x)) {
						if (visited[x,y] == 0 && floorMap[x,y] == squareType) {
							visited[x,y] = 1;
							queue.Enqueue(new Square(x,y));
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
                    DrawSquare(x, y, 0, floorMap);
				}
			}
		}
	}
    void DrawVegetationMap() {
		if (vegetationMap != null) {
			for (int x = 0; x < width; x ++) {
				for (int y = 0; y < height; y ++) {
                    DrawSquare(x, y, 0, vegetationMap);
				}
			}
		}
	}

    void DrawSquare(int x, int y, int id, int[,] map) {
        SquareTypes squareType = (SquareTypes) map[x, y];
        Vector3 pos = CoordToWorldPoint(x, y);
        GameObject squareToDraw;

        switch(squareType) {
            case SquareTypes.outerFloor:
                squareToDraw = outerFloor[id];
                break;
            case SquareTypes.floor:
                squareToDraw = floor[id];
                break;
            case SquareTypes.outerWall0:
                squareToDraw = outerWall0[id];
                break;
            case SquareTypes.outerWall1:
                squareToDraw = outerWall1[id];
                break;
			case SquareTypes.outerWall2:
                squareToDraw = outerWall2[id];
                break;
			case SquareTypes.outerWall3:
                squareToDraw = outerWall3[id];
                break;
			case SquareTypes.outerWall4:
                squareToDraw = outerWall4[id];
                break;
            case SquareTypes.innerWall0:
                squareToDraw = innerWall0[id];
                break;
            case SquareTypes.innerWall1:
                squareToDraw = innerWall1[id];
                break;
			case SquareTypes.innerWall2:
                squareToDraw = innerWall2[id];
                break;
			case SquareTypes.innerWall3:
                squareToDraw = innerWall3[id];
                break;
			case SquareTypes.innerWall4:
                squareToDraw = innerWall4[id];
                break;
            case SquareTypes.leftWall:
                squareToDraw = leftWall[id];
                break;
            case SquareTypes.rightWall:
                squareToDraw = rightWall[id];
                break;
			case SquareTypes.water:
                squareToDraw = water[id];
                break;
			case SquareTypes.bush:
                squareToDraw = bush[id];
                break;
			case SquareTypes.tree:
                squareToDraw = tree[id];
                break;
			case SquareTypes.cliff:
                squareToDraw = cliff[id];
                break;
			case SquareTypes.playerSpawner:
                squareToDraw = playerSpawner[id];
                break;
			case SquareTypes.enemySpawner:
                squareToDraw = enemySpawner[id];
                break;
			case SquareTypes.exit:
                squareToDraw = exit[id];
                break;
            case SquareTypes.liliypad:
                squareToDraw = lilypad[id];
                break;
            default:
                squareToDraw = floor[0];
                break;
        }
		if (squareType != SquareTypes.outerFloor) {
			mapObjects.Add(Instantiate(squareToDraw, pos, Quaternion.identity));
		}
    }
	#endregion

	bool IsWithinMap(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	public override Vector3 CoordToWorldPoint(int x, int y) {
		return new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
	}

	public override int[] WorldPointToCoord(Vector3 point)
    {
		return new int[] { (int)Math.Round(point.x + width / 2 - 0.5f, 0), (int)Math.Round(point.y + height / 2 - 0.5f, 0)};
    }

    public override void ClearMap() {
        // GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        // GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		// GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        // foreach (GameObject floor in floors) {
        //     Destroy(floor);
        // }
        // foreach (GameObject wall in walls) {
        //     Destroy(wall);
        // }
		// foreach (GameObject spawner in spawners) {
        //     Destroy(spawner);
        // }

		foreach (GameObject mapObject in mapObjects) {
            Destroy(mapObject);
        }
    }

    public class Square : IComparable<Square> {
		public int x;
		public int y;

		public Square(){}
		public Square(int _x, int _y) {
			x = _x;
			y = _y;
		}
		public int CompareTo(Square otherSquare) {
			return y.CompareTo(otherSquare.y);
		}
	}

	class Region : IComparable<Region> {
		public SquareTypes squareType;
		public List<Square> squares;
		public List<Square> edgeSquares;
		public List<Region> connectedRegions;
		public int regionSize;
		public bool isMainRegion = false;
		public bool isConnectedToMainRegion = false;

		public Region(SquareTypes _squareType, List<Square> _squares, int[,] floorMap, int width, int height) {
			squareType = _squareType;
			squares = _squares;
			regionSize = squares.Count;
			connectedRegions = new List<Region>();

			edgeSquares = new List<Square>();
			foreach (Square square in squares) {
				for (int x = square.x - 1; x <= square.x + 1; x++) {
					for (int y = square.y - 1; y <= square.y + 1; y++) {
						if (x >= 0 && x < width && y >= 0 && y < height) {
							//exclude diagonals
							if (squareType == SquareTypes.floor) {
								if (x == square.x || y == square.y) {
									if (floorMap[x,y] != (int) SquareTypes.floor) {
										edgeSquares.Add(square);
									}
								}
							} 
							//include diagonals
							else {
								if (floorMap[x,y] != (int) SquareTypes.outerFloor) {
									edgeSquares.Add(square);
								}
							}
						}
					}
				}
			}
		}

		public Region(SquareTypes _squareType) {
			squareType = _squareType;
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