using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArenaMapGenerator : MapGenerator
{
    private string arena;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public override void ClearMap()
    {
        return;
    }

    public override void GenerateMap(string _seed)
    {
        arena = _seed;
        StartCoroutine(GenerateBossMap());
    }

    public IEnumerator GenerateBossMap()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(arena);
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading boss arena...");
            yield return null;
        }
        Debug.Log("Boss arena loaded");
        if (NetworkManager.gameType == GameType.Client)
        {
            ClientSend.MapLoaded();
        }
    }

    public override int[] WorldPointToCoord(Vector3 point)
    {
        return new int[] { };
    }

    public override Vector3 CoordToWorldPoint(int x, int y)
    {
        return Vector3.back;
    }
}
