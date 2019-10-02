using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ShipSteeringTests
    {
        private Ship ship;

        [SetUp]
        public void Setup()
        {
            ship = new GameObject().AddComponent<Ship>();
        }

        [UnityTest]
        public IEnumerator DefaultInitialHeading()
        {
            yield return null;
            Assert.True(Vector2.up == ship.Heading);
        }

        [UnityTest]
        public IEnumerator SteerCounterClockWise()
        {
            ship.SetSpeed(Ship.EOT.FullAhead);
            ship.SteerTickCounterClockWise();
            yield return null;
            var shipH = ship.Heading;
            Assert.True(Vector2.up != ship.Heading);
            Assert.NotZero(ship.Heading.x);
            Assert.Less(ship.Heading.x, 0);
        }

        [UnityTest]
        public IEnumerator SteerClockWise()
        {
            ship.SetSpeed(Ship.EOT.FullAhead);
            ship.SteerTickClockWise();
            yield return null;
            Assert.True(Vector2.up != ship.Heading);
            Assert.NotZero(ship.Heading.x);
            Assert.Greater(ship.Heading.x, 0);
        }

        [UnityTest]
        public IEnumerator ResetSteering()
        {
            ship.SetSpeed(Ship.EOT.FullAhead);
            ship.SteerTickClockWise();
            yield return null;
            Assert.True(Vector2.up != ship.Heading);
            Assert.NotZero(ship.Heading.x);
            Assert.Greater(ship.Heading.x, 0);

            var lastHeading = ship.Heading;
            ship.ResetSteer();
            yield return null;
            Assert.True(lastHeading == ship.Heading);
        }


    }
}
