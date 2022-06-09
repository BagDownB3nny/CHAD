using UnityEngine;
using System.Collections;
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
	public int width;
	public int height;

    [Header("Seed")]
	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int fillPercent;
    public int smoothing;

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
        DrawMap();
	}


	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random rng = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					map[x,y] = (int) Tiles.leftWall;
				}
				else {
					map[x,y] = (rng.Next(0,100) < fillPercent)? (int) Tiles.leftWall: (int) Tiles.floor;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = (int) Tiles.leftWall;
				else if (neighbourWallTiles < 4)
					map[x,y] = (int) Tiles.floor;

			}
		}
	}

	int GetSurroundingWallCount(int tileX, int tileY) {
		int wallCount = 0;
		for (int neighbourX = tileX - 1; neighbourX <= tileX + 1; neighbourX ++) {
			for (int neighbourY = tileY - 1; neighbourY <= tileY + 1; neighbourY ++) {
                //if within the map
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    //if not the tile itself
					if (neighbourX != tileX || neighbourY != tileY) {
                        if (map[neighbourX, neighbourY] >= 1 && map[neighbourX, neighbourY] <= 8) {
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
}