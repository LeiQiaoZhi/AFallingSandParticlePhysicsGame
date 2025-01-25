using System;
using _Scripts.ParticleTypes;
using MyHelpers.Grid;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class Particle
    {
        private Color color;
        private ParticleType particleType;
        
        public Color Color => color;
        public bool Updated { get; set; } 

        private void SetColor(Color _color)
        {
            color = _color;
        }

        public void SetType(ParticleType _particleType)
        {
            // Debug.Log($"Setting type to {_particleType}");
            particleType = _particleType;
            SetColor(particleType.color);
            Updated = true;
        }

        public void Step(Vector2Int _position, ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet)
        {
            if (Updated) return;
            particleType.Step(this, _position, _particleContainer, _particleTypeSet);
        }

        public ParticleType ParticleType()
        {
            return particleType;
        }
    }
}