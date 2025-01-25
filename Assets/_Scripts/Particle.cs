using System;
using _Scripts.ParticleTypes;
using MyHelpers.Grid;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class Particle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color color;
        private ParticleType particleType;
        
        public bool Updated { get; set; } 

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void SetColor(Color _color)
        {
            color = _color;
            spriteRenderer.color = color;
        }

        public void SetType(ParticleType _particleType)
        {
            Debug.Log($"Setting type to {_particleType}");
            particleType = _particleType;
            SetColor(particleType.color);
            Updated = true;
        }

        public void Step(Vector2Int _position, GenericGridContainer<Particle> _gridContainer, ParticleTypeSet _particleTypeSet)
        {
            if (Updated) return;
            particleType.Step(this, _position, _gridContainer, _particleTypeSet);
        }

        public ParticleType ParticleType()
        {
            return particleType;
        }
    }
}