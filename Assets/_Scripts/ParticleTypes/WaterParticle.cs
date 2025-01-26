using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "WaterParticle", menuName = "Particles/WaterParticle", order = 3)]
    public class WaterParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            // update speed
            _dt *= speedMultiplier;

            var verticalOffset = (int)(_particle.Velocity.y * _dt);
            var horizontalOffset = (int)(horizontalSpeed * _dt);
            Assert.IsTrue(verticalOffset >= 0, "Vertical offset must be positive");

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
                    var offset = new Vector2Int(
                        (pointToTest.x - _position.x) * horizontalOffset,
                        (pointToTest.y - _position.y) * verticalOffset
                    );

                    Vector2Int target = _position + offset;
                    Vector2Int destination = TryMoveToTarget(pointToTest, target, _particleContainer);
                    Particle destinationParticle = _particleContainer.GetParticleByLocalPosition(destination);
                    var destinationResistance = destinationParticle.ParticleType.resistance;
                    UpdateVelocity(_particle, _dt, destinationResistance);

                    if (offset.sqrMagnitude > 1)
                        _particleContainer.Swap(_position, destination);

                    return;
                }
            }
        }
    }
}