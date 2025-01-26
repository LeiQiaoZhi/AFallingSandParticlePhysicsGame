using MyHelpers;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "AcidParticle", menuName = "Particles/AcidParticle", order = 4)]
    public class AcidParticle : ParticleType
    {
        public float corrosionStrength;

        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            // update speed
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
            
            var particlesToTest = new Particle[pointsToTest.Length];
            for (int i = 0; i < pointsToTest.Length; i++)
                particlesToTest[i] = _particleContainer.GetParticleByLocalPosition(pointsToTest[i]);

            Corrode(_particleContainer, _position, _particleTypeSet, pointsToTest);

            // movement
            foreach (Vector2Int pointToTest in pointsToTest)
            {
                Particle particleToTest = _particleContainer.GetParticleByLocalPosition(pointToTest);
                if (particleToTest != null && particleToTest.ParticleType is EmptyParticle)
                {
                    // Friction
                    var f = CalculateFriction(_particleContainer, _position);

                    UpdateVelocity(_particle, _dt, particleToTest.ParticleType.resistance, f, horizontalSpeed);

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
                        _p => _p.ParticleType is not EmptyParticle
                    );

                    if (offset.sqrMagnitude > 0.9)
                        _particleContainer.Swap(_position, destination);

                    return;
                }
            }
        }

        private void Corrode(ParticleEfficientContainer _particleContainer, Vector2Int _position,
            ParticleTypeSet _particleTypeSet, Vector2Int[] _pointsToTest)
        {
            foreach (Vector2Int pointToTest in _pointsToTest)
            {
                Particle particleToCorrode = _particleContainer.GetParticleByLocalPosition(pointToTest);
                if (particleToCorrode != null && particleToCorrode.ParticleType is not (EmptyParticle or AcidParticle))
                {
                    var corroded = particleToCorrode.Corrode(corrosionStrength);
                    if (corroded)
                    {
                        particleToCorrode.SetType(_particleTypeSet.GetInstanceByType(typeof(EmptyParticle)));
                        _particleContainer.Swap(pointToTest, _position);
                    }
                    break;
                }
            }
        }
    }
}