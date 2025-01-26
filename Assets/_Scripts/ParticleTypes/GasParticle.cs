using MyBox;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "GasParticle", menuName = "Particles/GasParticle", order = 5)]
    public class GasParticle : ParticleType
    {
        public Color endColor;

        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            if (lifetime.Enabled)
            {
                if (_particle.ReduceLifetime(_dt))
                    _particle.SetType(_particleTypeSet.GetInstanceByType(typeof(EmptyParticle)));
                else
                    _particle.Color = Color.Lerp(_particle.ParticleType.Color, endColor,
                        _particle.TimeAlive / lifetime.Value);
            }

            _dt *= speedMultiplier;

            Vector2Int[] pointsToTest =
            {
                _position + Vector2Int.up,
                _position + Vector2Int.up,
                _position + Vector2Int.up + Vector2Int.left,
                _position + Vector2Int.up + Vector2Int.right,
                _position + Vector2Int.left,
                _position + Vector2Int.right
            };

            // shuffle to simulate brownian motion
            pointsToTest.Shuffle();

            foreach (Vector2Int pointToTest in pointsToTest)
            {
                Particle particleToTest = _particleContainer.GetParticleByLocalPosition(pointToTest);
                if (
                    particleToTest != null && particleToTest.ParticleType is EmptyParticle or WaterParticle
                )
                {
                    UpdateVelocity(_particle, _dt, particleToTest.ParticleType.resistance, friction, horizontalSpeed);

                    var verticalOffset = (int)(_particle.Velocity.y * _dt);
                    var horizontalOffset = (int)(_particle.Velocity.x * _dt);
                    Assert.IsTrue(horizontalOffset >= 0, "Horizontal offset must be positive");

                    if (particleToTest.ParticleType is WaterParticle)
                    {
                        verticalOffset = 2;
                        horizontalOffset = 2;
                    }

                    var offset = new Vector2Int(
                        (pointToTest.x - _position.x) * horizontalOffset,
                        (pointToTest.y - _position.y) * verticalOffset
                    );
                    Vector2Int target = _position + offset;
                    Vector2Int destination = TryMoveToTarget(pointToTest, target, _particleContainer,
                        _p => _p.ParticleType is not (EmptyParticle or WaterParticle)
                    );

                    if (offset.sqrMagnitude > 0.9)
                        _particleContainer.Swap(_position, destination);

                    return;
                }
            }
        }
    }
}