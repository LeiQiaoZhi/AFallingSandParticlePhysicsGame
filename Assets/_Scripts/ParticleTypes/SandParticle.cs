using MyBox;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "SandParticle", menuName = "Particles/SandParticle", order = 1)]
    public class SandParticle : ParticleType
    {
        public float buoyancy = 0.5f;

        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            _dt *= speedMultiplier;

            Vector2Int[] pointsToTest =
            {
                _position + Vector2Int.down,
                _position + Vector2Int.down + Vector2Int.left,
                _position + Vector2Int.down + Vector2Int.right,
                _position + Vector2Int.left,
                _position + Vector2Int.right
            };
            if (Random.value < 0.5f)
                (pointsToTest[1], pointsToTest[2]) = (pointsToTest[2], pointsToTest[1]);
            if (Random.value < 0.5f)
                (pointsToTest[^2], pointsToTest[^1]) = (pointsToTest[^1], pointsToTest[^2]);

            foreach (Vector2Int pointToTest in pointsToTest)
            {
                Particle particleToTest = _particleContainer.GetParticleByLocalPosition(pointToTest);
                if (
                    particleToTest != null
                    && particleToTest.ParticleType is EmptyParticle or LiquidParticle or GasParticle
                )
                {
                    // Friction
                    var f = CalculateFriction(_particleContainer, _position);
                    
                    UpdateVelocity(_particle, _dt, particleToTest.ParticleType.resistance, f, horizontalSpeed);

                    // Buoyancy
                    if (particleToTest.ParticleType is LiquidParticle)
                    {
                        var v = Mathf.Clamp(_particle.Velocity.y - buoyancy, 0f, 10f);
                        _particle.Velocity = new Vector2(_particle.Velocity.x, v);
                    }

                    var verticalOffset = (int)(_particle.Velocity.y * _dt);
                    var horizontalOffset = (int)(_particle.Velocity.x * _dt);
                    Assert.IsTrue(verticalOffset >= 0, "Vertical offset must be positive");
                    Assert.IsTrue(horizontalOffset >= 0, "Horizontal offset must be positive");

                    var offset = new Vector2Int(
                        (pointToTest.x - _position.x) * horizontalOffset,
                        (pointToTest.y - _position.y) * verticalOffset
                    );
                    Vector2Int target = _position + offset;
                    Vector2Int destination = TryMoveToTarget(pointToTest, target, _particleContainer,
                        _p => _p.ParticleType is not (EmptyParticle or LiquidParticle or GasParticle)
                    );

                    if (offset.sqrMagnitude > 0.9)
                        _particleContainer.Swap(_position, destination);

                    return;
                }
            }
            
            // react
            reactions.ForEach(_reaction => _reaction.React(_particleContainer, _particle, _position));
        }

    }
}