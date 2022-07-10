using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapType {
    lobby = -1,
    city = 0,
    forest = 1,
    desert = 2,
    bossArena = 3
}

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public MapType mapType = MapType.lobby;
    public string seed;
    public static int counter = 0;
    public List<GameObject> mapGenerators = new List<GameObject>();
    public GameObject currentMapGenerator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //TODO
    public MapType GetMapType() {
        if (GameManager.instance.IsBossLevel())
        {
            mapType = MapType.bossArena;
        }
        else
        {
            mapType = MapType.forest;
        }
        return mapType;
    }

    public string GetSeed() {
        if (GameManager.instance.IsBossLevel())
        {
            seed = "BossArena1";
        } else
        {
            seed = Time.time.ToString();
        }
        return seed;
    }

    public void LoadMap() {
        GameUIManager.instance.objectiveText.SetActive(false);
        GameUIManager.instance.holeUI.SetActive(false);
        if (mapType == MapType.lobby)
        {
            GameManager.instance.ResetGame();
        } else
        {
            GameManager.instance.NextGame();
        }
        
        if (currentMapGenerator != null) {
            currentMapGenerator.GetComponent<MapGenerator>().ClearMap();
        }

        StartCoroutine(GenerateScene());
    }

    public IEnumerator GenerateScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EmptyMap");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        currentMapGenerator = Instantiate(mapGenerators[(int)GetMapType()], new Vector3(0, 0, 0), Quaternion.identity);
        currentMapGenerator.GetComponent<MapGenerator>().GenerateMap(GetSeed());
        ServerSend.LoadEmptyMap();
    }

    public void ReceiveLoadMap(MapType _mapType, string _seed) {
        GameManager.instance.ResetGame();
        ItemManager.instance.ResetItems();
        CameraMotor.instance.SetPlayerDeath(false);

        if (currentMapGenerator != null) {
            currentMapGenerator.GetComponent<MapGenerator>().ClearMap();
        }

        GameObject mapGenerator = Instantiate(mapGenerators[(int) _mapType], new Vector3(0, 0, 0), Quaternion.identity);
        mapGenerator.GetComponent<MapGenerator>().GenerateMap(_seed);
    }

    public void ReceiveLoadEmptyMap() {
        GameUIManager.instance.objectiveText.SetActive(false);
        GameUIManager.instance.holeUI.SetActive(false);

        GameManager.instance.ResetGame();

        if (currentMapGenerator != null) {
            currentMapGenerator.GetComponent<MapGenerator>().ClearMap();
        }

        StartCoroutine(GenerateEmptyScene());
    }

    public IEnumerator GenerateEmptyScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EmptyMap");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        ClientSend.EmptyMapLoaded();
    }
}
