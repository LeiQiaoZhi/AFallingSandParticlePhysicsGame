using MyHelpers;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "FireParticle", menuName = "Particles/FireParticle", order = 3)]
    public class FireParticle : ParticleType
    {
        [Header("Fire")] [Range(0, 1)] public float smokeChance = 0.2f;
        [Range(0, 1)] public float ashChance = 0.1f;
        public Color burnOutColor;
        public GasParticle smokeParticle;
        public GasParticle steamParticle;
        public ParticleType ashParticle;

        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            Vector2Int[] neighbours =
            {
                _position + Vector2Int.up,
                _position + Vector2Int.up + Vector2Int.left,
                _position + Vector2Int.up + Vector2Int.right,
                _position + Vector2Int.left,
                _position + Vector2Int.right,
                _position + Vector2Int.down,
                _position + Vector2Int.down + Vector2Int.left,
                _position + Vector2Int.down + Vector2Int.right,
            };

            Vector2Int[] movablePoints =
            {
                _position + Vector2Int.down,
                _position + Vector2Int.down + Vector2Int.left,
                _position + Vector2Int.down + Vector2Int.right,
                _position + Vector2Int.left,
                _position + Vector2Int.right,
            };
            if (Random.value < 0.5f)
                (movablePoints[1], movablePoints[2]) = (movablePoints[2], movablePoints[1]);
            if (Random.value < 0.5f)
                (movablePoints[^2], movablePoints[^1]) = (movablePoints[^1], movablePoints[^2]);

            Inflamme(_particleContainer, _position, _particleTypeSet, neighbours);

            // manage lifetime -- lack of oxygen or fuel reduces lifetime
            if (lifetime.Enabled)
            {
                var numExtinguishing = 0;
                Particle emptyParticle = null;
                foreach (Vector2Int neighbour in neighbours)
                {
                    Particle particleToTest = _particleContainer.GetParticleByLocalPosition(neighbour);
                    if (particleToTest == null) continue;
                    if (particleToTest.ParticleType is not EmptyParticle &&
                        particleToTest.ParticleType.infammability == 0)
                        numExtinguishing++;
                    else if (particleToTest.ParticleType is EmptyParticle)
                        emptyParticle = particleToTest;
                }

                if (_particle.ReduceLifetime(Helpers.Remap(numExtinguishing, 0, 8, 1, 3) * _dt) &&
                    Random.value < ashChance)
                {
                    _particle.SetType(ashParticle);
                }
                else
                {
                    _particle.Color = Color.Lerp(_particle.ParticleType.Color, burnOutColor,
                        _particle.TimeAlive / lifetime.Value);
                    if (emptyParticle != null && Random.value < smokeChance)
                    {
                        emptyParticle.SetType(smokeParticle);
                    }
                }
            }

            _dt *= speedMultiplier;

            // movement
            foreach (Vector2Int pointToTest in movablePoints)
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
                        _p => _p.ParticleType is not (EmptyParticle or WaterParticle)
                    );

                    // fire + water => steam
                    Particle destinationParticle = _particleContainer.GetParticleByLocalPosition(destination);
                    if (destinationParticle != null && destinationParticle.ParticleType is WaterParticle)
                    {
                        _particle.SetType(_particleTypeSet.GetInstanceByType(typeof(EmptyParticle)));
                        destinationParticle.SetType(steamParticle);
                        return;
                    }

                    if (offset.sqrMagnitude > 0.9)
                        _particleContainer.Swap(_position, destination);

                    return;
                }
            }
        }

        private void Inflamme(ParticleEfficientContainer _particleContainer, Vector2Int _position,
            ParticleTypeSet _particleTypeSet, Vector2Int[] _pointsToTest)
        {
            foreach (Vector2Int pointToTest in _pointsToTest)
            {
                Particle particleToInflamme = _particleContainer.GetParticleByLocalPosition(pointToTest);
                if (particleToInflamme != null &&
                    particleToInflamme.ParticleType.infammability > 0)
                {
                    var inflammed = Random.value < particleToInflamme.ParticleType.infammability;
                    if (inflammed)
                    {
                        particleToInflamme.SetType(_particleTypeSet.GetInstanceByType(typeof(FireParticle)));
                    }

                    break;
                }
            }
        }
    }
}