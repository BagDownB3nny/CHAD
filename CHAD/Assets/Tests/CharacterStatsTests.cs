using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

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
        public void CharacterStats_GetEnemySpawnTime1()
        {
            var testSpawner = new GameObject().AddComponent<EnemySpawner>();
            testSpawner.enemiesAlive = 3;
            Assert.AreEqual(testSpawner.generateNextSpawnTime(), 0.2f);
        }
    }
}