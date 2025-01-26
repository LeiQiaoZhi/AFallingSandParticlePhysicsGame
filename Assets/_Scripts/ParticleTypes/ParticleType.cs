using System;
using _Scripts.Reactions;
using MyBox;
using MyHelpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.ParticleTypes
{
    public enum ColorType
    {
        SingleColor,
        RandomBetweenTwoColors,
    }

    public abstract class ParticleType : ScriptableObject
    {
        public string particleName;
        [Header("Color")] public ColorType colorType;
        [SerializeField] protected Color color;

        [ConditionalField(nameof(colorType), false, ColorType.RandomBetweenTwoColors)] [SerializeField]
        private Color color2;

        [Header("Physics Settings")] public float speedMultiplier = 1;
        public float resistance = 1;
        public float gravity = 9.81f;
        public int horizontalSpeed = 2;
        public float friction = 0;
        [Header("Other States")]
        public float corrosionResistance;
        public Optional<float> lifetime;
        [Range(0, 1)]
        public float infammability;
        [Header("Reactions")]
        public Reaction[] reactions;

        public Color Color
        {
            get
            {
                if (colorType == ColorType.SingleColor)
                    return color;
                else
                    return Random.value < 0.5f ? color : color2;
            }
        }


        public abstract void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt);
        
        protected static Vector2Int TryMoveToTarget(Vector2Int _position, Vector2Int _targetPosition,
            ParticleEfficientContainer _particleContainer)
        {
            return TryMoveToTarget(_position, _targetPosition, _particleContainer,
                _particle => _particle.ParticleType is not EmptyParticle);
        }

        protected static Vector2Int TryMoveToTarget(Vector2Int _position, Vector2Int _targetPosition,
            ParticleEfficientContainer _particleContainer, Predicate<Particle> _stopCondition)

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
                        if (particle == null || _stopCondition(particle))
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

        protected void UpdateVelocity(Particle _particle, float _dt, float _resistance, float _friction, float _horizontalAcc)
        {
            // vertical
            var vertical = _particle.Velocity.y;
            var verticalAcc = -_resistance * vertical * vertical + gravity;
            var newVertical = Mathf.Clamp(_particle.Velocity.y + verticalAcc * _dt, 0, 10);
            
            // horizontal
            var horizontalAcc = -_friction + _horizontalAcc;
            var newHorizontal = Mathf.Clamp(_particle.Velocity.x + horizontalAcc * _dt, 0, 10);
            
            _particle.Velocity = new Vector2(newHorizontal, newVertical);
        }
        
        
        protected float CalculateFriction(ParticleEfficientContainer _particleContainer, Vector2Int _position)
        {
            Particle particleGivingFriction = _particleContainer.GetParticleByLocalPosition(
                _position + Vector2Int.down
            );
            return particleGivingFriction != null ? particleGivingFriction.ParticleType.friction : friction;
        }
    }
}