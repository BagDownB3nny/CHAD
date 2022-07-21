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
    }
}