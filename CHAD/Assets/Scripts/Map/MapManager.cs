using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapType {
    city = 0,
    forest = 1,
    desert = 2
}

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public MapType mapType;
    public string seed;
    public static int counter = 0;
    public List<GameObject> mapGenerators = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //TODO
    public MapType GetMapType() {
        mapType = MapType.forest;
        return mapType;
    }

    public string GetSeed() {
        seed = Time.time.ToString();
        return seed;
    }

    public void LoadMap() {
        GameManager.instance.ResetGame();
        StartCoroutine(GenerateScene());
    }

    public IEnumerator GenerateScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EmptyMap");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        GameObject mapGenerator = Instantiate(mapGenerators[(int)GetMapType()], new Vector3(0, 0, 0), Quaternion.identity);
        mapGenerator.GetComponent<MapGenerator>().GenerateMap(GetSeed());
        ServerSend.LoadEmptyMap();
    }

    public void ReceiveLoadMap(MapType _mapType, string _seed) {
        GameObject mapGenerator = Instantiate(mapGenerators[(int) _mapType], new Vector3(0, 0, 0), Quaternion.identity);
        mapGenerator.GetComponent<MapGenerator>().GenerateMap(_seed);
    }
}
