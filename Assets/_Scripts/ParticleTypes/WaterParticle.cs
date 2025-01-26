using System.Collections.Generic;
using System.Linq;
using MyHelpers;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "WaterParticle", menuName = "Particles/WaterParticle", order = 3)]
    public class WaterParticle : ParticleType
    {
        public float speedMultiplier = 1;
        public float airResistance = 1;
        public float gravity = 9.81f;
        public float mass = 1;
        public int horizontalSpeed = 2;

        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            // update speed
            _dt *= speedMultiplier;

            var verticalOffset = (int)(_particle.Velocity.y * _dt);
            var horizontalOffset = (int)(horizontalSpeed * _dt);

            Vector2Int[] pointsToTest =
            {
                _position + Vector2Int.down,
                _position + Vector2Int.down + Vector2Int.left,
                _position + Vector2Int.down + Vector2Int.right,
                _position + Vector2Int.left,
                _position + Vector2Int.right
            };

            foreach (Vector2Int pointToTest in pointsToTest)
            {
                Particle particleToTest = _particleContainer.GetParticleByLocalPosition(pointToTest);
                if (particleToTest != null && particleToTest.ParticleType is EmptyParticle)
                {
                    UpdateVelocity(_particle, _dt);
                    Vector2Int target = _position + new Vector2Int(
                        (pointToTest.x - _position.x) * horizontalOffset,
                        (pointToTest.y - _position.y) * verticalOffset
                    );
                    if (Mathf.Abs(horizontalOffset) < 1 && Mathf.Abs(verticalOffset) < 1)
                        return;
                    Vector2Int destination = TryMoveToTarget(pointToTest, target, _particleContainer);
                    _particleContainer.Swap(_position, destination);
                    return;
                }
            }
        }

        private void UpdateVelocity(Particle _particle, float _dt)
        {
            Debug.Log("Updating velocity");
            var currentVelocity = _particle.Velocity.y;
            var acc = (-airResistance * currentVelocity * currentVelocity + mass * gravity) / mass;
            var newVelocity = Mathf.Clamp(_particle.Velocity.y + acc * _dt, -10, 10);
            _particle.Velocity = new Vector2(_particle.Velocity.x, newVelocity);
        }

        private Vector2Int TryMoveToTarget(Vector2Int _position, Vector2Int _targetPosition,
            ParticleEfficientContainer _particleContainer)
        {
            var points = Helpers.LazyLinePoints(_position, _targetPosition);

            Vector2Int lastValidPoint = _position; // Default to starting position if no points are valid
            var index = 0;

            using (var pointEnumerator = points.GetEnumerator())
            {
                while (pointEnumerator.MoveNext())
                {
                    Vector2Int point = pointEnumerator.Current;

                    // Skip the first point, continue with the rest
                    if (index > 0)
                    {
                        Particle particle = _particleContainer.GetParticleByLocalPosition(point);
                        if (particle == null || particle.ParticleType is not EmptyParticle)
                        {
                            break; // Stop evaluating and break the loop as soon as a condition fails
                        }

                        lastValidPoint = point; // Update last valid point
                    }

                    index++; // Increment index
                }
            }

            return lastValidPoint; // Return the last valid point processed
        }
    }
}