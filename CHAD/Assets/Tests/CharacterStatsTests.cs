using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;
using UnityEngine.TestTools;

namespace Tests
{
    public class CharacterStatsTests
    {

        [Test]
        public void CharacterStats_CalcDamageTaken1()
        {
            var testStats = new GameObject().AddComponent<PlayerStatsManager>();
            testStats.armour = 0.2f;
            testStats.armourEffectiveness = 0.2f;

            Assert.AreEqual(testStats.CalcDamageTaken(10f, 0.1f), 10);
        }

        [Test]
        public void CharacterStats_CalcDamageTaken2()
        {
            var testStats = new GameObject().AddComponent<PlayerStatsManager>();
            testStats.armour = 0.4f;
            testStats.armourEffectiveness = 0.3f;

            Assert.AreEqual(testStats.CalcDamageTaken(50f, 0.2f), 46);
        }

        [Test]
        public void CharacterStats_CalcDamageTaken3()
        {
            var testStats = new GameObject().AddComponent<PlayerStatsManager>();
            testStats.armour = 0.5f;
            testStats.armourEffectiveness = 0.1f;

            Assert.AreEqual(testStats.CalcDamageTaken(100f, 0.6f), 98);
        }

        [Test]
        public void CharacterStats_CalcDamageDealt1()
        {
            var testDamager = new GameObject().AddComponent<MeleeDirectDamager>();
            Assert.AreEqual(Mathf.RoundToInt(testDamager.CalculateDamageDealt(50f, 1.2f)), 60);
        }

        [Test]
        public void CharacterStats_CalcDamageDealt2()
        {
            var testDamager = new GameObject().AddComponent<RangedDirectDamager>();
            Assert.AreEqual(Mathf.RoundToInt(testDamager.CalculateDamageDealt(22f, 1f)), 22);
        }

        [Test]
        public void CharacterStats_CalcDamageDealt3()
        {
            var testDamager = new GameObject().AddComponent<MeleeDirectDamager>();
            Assert.AreEqual(Mathf.RoundToInt(testDamager.CalculateDamageDealt(33f, 3.0f)), 99);
        }

        [Test]
        public void EnemySpawner_GetEnemySpawnTime1()
        {
            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            testSpawner.enemiesAlive = 10;
            testSpawner.enemiesLeftToSpawn = 30;
            testSpawner.totalEnemiesToSpawn = 100;

            Server.NumberOfPlayers = 1;
            Assert.AreEqual(testSpawner.generateNextSpawnTime(), 2.0f);
        }

        [Test]
        public void EnemySpawner_GetEnemySpawnTime2()
        {
            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            testSpawner.enemiesAlive = 3;
            Assert.AreEqual(testSpawner.generateNextSpawnTime(), 0.2f);
        }

        [Test]
        public void EnemySpawner_GetEnemySpawnTime3()
        {
            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            testSpawner.enemiesAlive = 6;
            testSpawner.enemiesLeftToSpawn = 44;
            testSpawner.totalEnemiesToSpawn = 50;

            Server.NumberOfPlayers = 2;
            Assert.AreEqual(testSpawner.generateNextSpawnTime(), 4.0f);
        }

        [Test]
        public void ItemManager_GetDropProbability1()
        {
            var testItemManager = new GameObject().AddComponent<ItemManager>();
            testItemManager.itemsToDrop = 4;
            testItemManager.itemsDropped = 4;

            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            EnemySpawner.instance = testSpawner;
            testSpawner.totalEnemiesToSpawn = 100;
            testSpawner.enemiesKilled = 50;

            Assert.AreEqual(testItemManager.GetDropProbability(), 0);
        }

        [Test]
        public void ItemManager_GetDropProbability2()
        {
            var testItemManager = new GameObject().AddComponent<ItemManager>();
            testItemManager.itemsToDrop = 4;
            testItemManager.itemsDropped = 2;

            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            EnemySpawner.instance = testSpawner;
            testSpawner.totalEnemiesToSpawn = 100;
            testSpawner.enemiesKilled = 30;

            Assert.AreEqual(Mathf.RoundToInt(testItemManager.GetDropProbability() * 100000), 0.02857f * 100000);
        }

        [Test]
        public void ItemManager_GetDropProbability3()
        {
            var testItemManager = new GameObject().AddComponent<ItemManager>();
            testItemManager.itemsToDrop = 12;
            testItemManager.itemsDropped = 5;

            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            EnemySpawner.instance = testSpawner;
            testSpawner.totalEnemiesToSpawn = 100;
            testSpawner.enemiesKilled = 75;

            Assert.AreEqual(Mathf.RoundToInt(testItemManager.GetDropProbability() * 10000), 0.28f * 10000);
        }
    }
}