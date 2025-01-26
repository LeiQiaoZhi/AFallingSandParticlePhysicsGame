using System;
using UnityEngine;

namespace _Scripts
{
    /// <summary>
    /// Local coordinates: (0, 0) is the bottom left corner
    /// </summary>
    public class ParticlesRenderer : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        
        private Texture2D texture;
        private Color[] colors;

        private void Reset()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Initialize(Vector2Int _size)
        {
            texture = new Texture2D(_size.x, _size.y);
            colors = new Color[_size.x * _size.y];
            for (var x = 0; x < _size.x; x++)
            {
                for (var y = 0; y < _size.y; y++)
                {
                    colors[x + y * _size.x] = Color.black;
                }
            }
            texture.filterMode = FilterMode.Point;
        }

        public void RenderParticles(ParticleEfficientContainer _particlesContainer)
        {
            var width = _particlesContainer.Size.x;
            var height = _particlesContainer.Size.y;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Particle particle = _particlesContainer.GetParticleByLocalPosition(new Vector2Int(x, y));
                    colors[x + y * width] = particle.Color;
                }
            }

            texture.SetPixels(colors);
            texture.Apply();

            meshRenderer.material.mainTexture = texture;
        }
    }
}