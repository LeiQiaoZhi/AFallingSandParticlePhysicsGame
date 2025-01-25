using System;
using UnityEngine;

namespace _Scripts
{
    /// <summary>
    /// Local coordinates: (0, 0) is the bottom left corner
    /// </summary>
    public class ParticleEfficientContainer : MonoBehaviour
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] private ParticlesRenderer particlesRenderer;

        private Particle[,] particles;

        // Properties
        public Particle[,] Particles => particles;
        public Vector2Int Size => size;

        public Vector2 WorldSize =>
            new Vector2(particlesRenderer.transform.localScale.x, particlesRenderer.transform.localScale.y) * 1;

        public Vector2 BottomLeftWorldPosition => (Vector2)particlesRenderer.transform.position - WorldSize / 2;

        public void Initialize()
        {
            particles = new Particle[size.x, size.y];
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    particles[x, y] = new Particle();
                }
            }
        }

        public Particle GetParticleByLocalPosition(Vector2Int _position)
        {
            if (_position.x < 0 || _position.x >= size.x || _position.y < 0 || _position.y >= size.y)
            {
                // Debug.LogWarning($"Position out of bounds: {_position}");
                return null;
            }

            return particles[_position.x, _position.y];
        }

        public void SetParticleByLocalPosition(Vector2Int _position, Particle _particle)
        {
            if (_position.x < 0 || _position.x >= size.x || _position.y < 0 || _position.y >= size.y)
            {
                return;
            }

            particles[_position.x, _position.y] = _particle;
        }

        public Vector2Int WorldToLocalPosition(Vector2 _position)
        {
            Vector2 bottomLeft = BottomLeftWorldPosition;
            Vector2 worldSize = WorldSize;
            var x = (_position.x - bottomLeft.x) / worldSize.x * size.x;
            var y = (_position.y - bottomLeft.y) / worldSize.y * size.y;
            return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        }

        public Particle GetParticleByWorldPosition(Vector2 _position)
        {
            Vector2Int localPosition = WorldToLocalPosition(_position);
            return GetParticleByLocalPosition(localPosition);
        }

        public void SetParticleByWorldPosition(Vector2 _position, Particle _particle)
        {
            Vector2Int localPosition = WorldToLocalPosition(_position);
            SetParticleByLocalPosition(localPosition, _particle);
        }

        public void Clear()
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    particles[x, y] = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(BottomLeftWorldPosition + WorldSize / 2, WorldSize);
            Gizmos.DrawWireSphere(BottomLeftWorldPosition, 1.0f);
        }
    }
}