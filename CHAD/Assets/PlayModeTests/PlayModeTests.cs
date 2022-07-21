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
        GameObject projectile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyWeapons/Rifle/RifleBullet"));
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));

        // Setting projectile stats needed for test
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
    }

    [UnityTest]
    public IEnumerator Projectiles_TimeToDespawn2()
    {
        // Instantiating game objects in test scene
        GameObject projectile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyWeapons/Rifle/RifleBullet"));
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));

        // Setting projectile stats needed for test
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
    }

    [UnityTest]
    public IEnumerator Projectiles_TimeToDespawn3()
    {
        // Instantiating game objects in test scene
        GameObject projectile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemyWeapons/Rifle/RifleBullet"));
        GameObject gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"));

        // Setting projectile stats needed for test
        ProjectileStatsManager stats = projectile.GetComponent<ProjectileStatsManager>();
        string projectile_id = "test";
        stats.projectileRefId = projectile_id;
        stats.speed = 2f;
        stats.range = 3f;

        // Adding to GameManager dictionary
        gameManager.GetComponent<GameManager>().projectiles.Add(projectile_id, projectile);

        yield return new WaitForSeconds(1.5f);
        Assert.IsTrue(projectile != null);
        if (projectile != null)
        {
            Object.Destroy(projectile);
        }
        Object.Destroy(gameManager);
    }
}
