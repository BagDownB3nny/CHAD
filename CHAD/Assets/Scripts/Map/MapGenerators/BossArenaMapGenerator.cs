using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArenaMapGenerator : MapGenerator
{
    private string arena;

    public override void ClearMap()
    {
        return;
    }

    public override void GenerateMap(string _seed)
    {
        arena = _seed;
        StartCoroutine(GenerateBossMap());
        if (NetworkManager.gameType == GameType.Client)
        {
            ClientSend.MapLoaded();
        }
    }

    public IEnumerator GenerateBossMap()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(arena);
        while (!asyncLoad.isDone)
        {
            yield return null;
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
