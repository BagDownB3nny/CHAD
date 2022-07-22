using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTests
{
    [UnityTest]
    public IEnumerator Projectiles_TimeToDespawn1()
    {
        // Instantiating game objects in test scene
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        // Setting projectile stats needed for test
        GameObject projectile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyWeapons/Rifle/RifleBullet"));
        ProjectileStatsManager stats = projectile.GetComponent<ProjectileStatsManager>();
        string projectile_id = "test";
        stats.projectileRefId = projectile_id;
        stats.speed = 10f;
        stats.range = 20f;

        // Adding to GameManager dictionary
        gameManager.GetComponent<GameManager>().projectiles.Add(projectile_id, projectile);

        yield return new WaitForSeconds(5f);
        Assert.IsTrue(projectile == null);
        if (projectile != null)
        {
            Object.Destroy(projectile);
        }
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator Projectiles_TimeToDespawn2()
    {
        // Instantiating game objects in test scene
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        // Setting projectile stats needed for test
        GameObject projectile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyWeapons/Rifle/RifleBullet"));
        ProjectileStatsManager stats = projectile.GetComponent<ProjectileStatsManager>();
        string projectile_id = "test";
        stats.projectileRefId = projectile_id;
        stats.speed = 10f;
        stats.range = 20f;

        // Adding to GameManager dictionary
        gameManager.GetComponent<GameManager>().projectiles.Add(projectile_id, projectile);

        yield return new WaitForSeconds(3f);
        Assert.IsTrue(projectile != null);
        if (projectile != null)
        {
            Object.Destroy(projectile);
        }
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator Projectiles_TimeToDespawn3()
    {
        // Instantiating game objects in test scene
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        // Setting projectile stats needed for test
        GameObject projectile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyWeapons/Rifle/RifleBullet"));
        ProjectileStatsManager stats = projectile.GetComponent<ProjectileStatsManager>();
        string projectile_id = "test";
        stats.projectileRefId = projectile_id;
        stats.speed = 10f;
        stats.range = 20f;

        // Adding to GameManager dictionary
        gameManager.GetComponent<GameManager>().projectiles.Add(projectile_id, projectile);

        yield return new WaitForSeconds(3f);
        Assert.IsTrue(projectile != null);
        if (projectile != null)
        {
            Object.Destroy(projectile);
        }
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator MeleeEnemies_MoveToPlayer1()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(5, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject whiteDude = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/WhiteDude")
                , new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);

        Assert.Greater(whiteDude.transform.position.x, 0);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(whiteDude);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator MeleeEnemies_MoveToPlayer2()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject whiteDude = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/WhiteDude")
                , new Vector3(5, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.Less(whiteDude.transform.position.x, 5);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(whiteDude);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator MeleeEnemies_MoveToPlayer3()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 5, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject whiteDude = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/WhiteDude")
                , new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.AreEqual(whiteDude.transform.position.x, 0);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(whiteDude);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedEnemies_MoveToPlayer1()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(4, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(-4, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.Greater(maskedGuy.transform.position.x, -4);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedEnemies_MoveToPlayer2()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(-4, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(4, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.Less(maskedGuy.transform.position.x, 4);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedEnemies_MoveToPlayer3()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 4, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(0, -4, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.AreEqual(maskedGuy.transform.position.x, 0);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedEnemies_RetreatFromPlayer1()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(1, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.Less(maskedGuy.transform.position.x, 0);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedEnemies_RetreatFromPlayer2()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(1, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.Greater(maskedGuy.transform.position.x, 1);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedEnemies_RetreatFromPlayer3()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 1, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Assert.AreEqual(maskedGuy.transform.position.x, 0);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedProjectile_ShootsAtPlayer1()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(5, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(-5, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        string projectile_id = "null";
        GameObject projectile = new GameObject();
        foreach (KeyValuePair<string, GameObject> pair in GameManager.instance.projectiles)
        {
            if (projectile_id == "null")
            {
                projectile_id = pair.Key;
                projectile = pair.Value;
                break;
            }
        }
        Vector3 initialPos = projectile.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 finalPos = projectile.transform.position;
        Assert.Less(initialPos.x, finalPos.x);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedProjectile_ShootsAtPlayer2()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(-5, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(5, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        string projectile_id = "null";
        GameObject projectile = new GameObject();
        foreach (KeyValuePair<string, GameObject> pair in GameManager.instance.projectiles)
        {
            if (projectile_id == "null")
            {
                projectile_id = pair.Key;
                projectile = pair.Value;
                break;
            }
        }
        Vector3 initialPos = projectile.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 finalPos = projectile.transform.position;
        Assert.Greater(initialPos.x, finalPos.x);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator RangedProjectile_ShootsAtPlayer3()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(-5, -5, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(5, 5, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        string projectile_id = "null";
        GameObject projectile = new GameObject();
        foreach (KeyValuePair<string, GameObject> pair in GameManager.instance.projectiles)
        {
            if (projectile_id == "null")
            {
                projectile_id = pair.Key;
                projectile = pair.Value;
                break;
            }
        }
        Vector3 initialPos = projectile.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 finalPos = projectile.transform.position;
        Assert.IsTrue(initialPos.x > finalPos.x && initialPos.y > finalPos.y);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator Stats_PlayerTakesDamage1()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 0, 0), Quaternion.identity);
        camera.GetComponent<CameraMotor>().player = player;
        GameObject maskedGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/MaskedGuyPrefab")
                , new Vector3(5, 0, 0), Quaternion.identity);
        float initialHealth = player.GetComponent<CharacterStatsManager>().hp;
        yield return new WaitForSeconds(2.0f);
        float finalHealth = player.GetComponent<CharacterStatsManager>().hp;
        Assert.Less(finalHealth, initialHealth);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(maskedGuy);
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator Stats_PlayerTakesDamage2()
    {
        NetworkManager.gameType = GameType.Server;
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));
        GameObject camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera/Main Camera"));
        yield return new WaitForSeconds(0.2f);
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Players/Player")
                , new Vector3(0, 0, 0), Quaternion.identity);
        camera.GetComponent<Camera>().orthographicSize = 9;
        GameObject whiteGuy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/WhiteDude")
                , new Vector3(2, 0, 0), Quaternion.identity);
        float initialHealth = player.GetComponent<CharacterStatsManager>().hp;
        yield return new WaitForSeconds(2.0f);
        float finalHealth = player.GetComponent<CharacterStatsManager>().hp;
        Assert.Less(finalHealth, initialHealth);
        Object.Destroy(gameManager);
        Object.Destroy(camera);
        Object.Destroy(player);
        Object.Destroy(whiteGuy);
        yield return new WaitForSeconds(1f);
    }
}
