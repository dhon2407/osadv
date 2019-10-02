using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ShipMovementTests
    {
        private Ship ship;
        private Vector2 initializedPosition;

        [SetUp]
        public void Setup()
        {
            ship = new GameObject().AddComponent<Ship>();
            initializedPosition = ship.Position;
        }

        [UnityTest]
        public IEnumerator Initialization()
        {
            yield return null;
            Assert.NotNull(ship);
            Assert.True(Vector2.up == ship.Heading);
            Assert.True(initializedPosition == ship.Position);
        }

        [UnityTest]
        public IEnumerator Stationary()
        {
            ship.SetSpeed(Ship.EOT.Stop);
            yield return null;
            Assert.True(initializedPosition == ship.Position);
        }

        [UnityTest]
        public IEnumerator MovingForward()
        {
            ship.SetSpeed(Ship.EOT.FullAhead);
            yield return null;
            Assert.True(initializedPosition != ship.Position);
            Assert.True(ship.Heading * 1 == (ship.Position - initializedPosition).normalized);
        }

        [UnityTest]
        public IEnumerator KeepMovingForwardThenStopMoving()
        {
            ship.SetSpeed(Ship.EOT.FullAhead);

            float timeLapse = 0;
            float testTime = 1f;
            var currentPosition = ship.Position;

            while (timeLapse < testTime)
            {
                timeLapse += Time.deltaTime;
                yield return null;
                Assert.True(currentPosition != ship.Position);
                Assert.True(ship.Heading * 1 == (ship.Position - currentPosition).normalized);
            }

            currentPosition = ship.Position;
            ship.SetSpeed(Ship.EOT.Stop);
            yield return null;
            Assert.True(currentPosition == ship.Position);
        }

        [UnityTest]
        public IEnumerator MovingBackwards()
        {
            ship.SetSpeed(Ship.EOT.FullAstern);
            yield return null;
            Assert.True(initializedPosition != ship.Position);
            Assert.True(ship.Heading * -1 == (ship.Position - initializedPosition).normalized);
        }

        [UnityTest]
        public IEnumerator KeepMovingBackwardsThenStopMoving()
        {
            ship.SetSpeed(Ship.EOT.FullAstern);

            float timeLapse = 0;
            float testTime = 1f;
            var currentPosition = ship.Position;

            while (timeLapse < testTime)
            {
                timeLapse += Time.deltaTime;
                yield return null;
                Assert.True(currentPosition != ship.Position);
                Assert.True(ship.Heading * -1 == (ship.Position - currentPosition).normalized);
            }

            currentPosition = ship.Position;
            ship.SetSpeed(Ship.EOT.Stop);
            yield return null;
            Assert.True(currentPosition == ship.Position);
        }
    }
}
