using System;
using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class Particle
    {
        // states
        private Color color;
        private ParticleType particleType;
        private Vector2 velocity = Vector2.zero;

        // properties
        public Color Color { get; private set; }
        public bool Updated { get; set; }
        public ParticleType ParticleType => particleType;

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public void SetType(ParticleType _particleType)
        {
            // Debug.Log($"Setting type to {_particleType}");
            particleType = _particleType;
            Color = _particleType.Color;
            Updated = true;
        }

        public void Step(Vector2Int _position, ParticleEfficientContainer _particleContainer,
            ParticleTypeSet _particleTypeSet, float _dt)
        {
            if (Updated) return;
            particleType.Step(this, _position, _particleContainer, _particleTypeSet, _dt);
        }
    }
}