using UnityEngine;

namespace _Scripts
{
    /// <summary>
    /// Local coordinates: (0, 0) is the bottom left corner
    /// </summary>
    public class ParticlesRenderer : MonoBehaviour
    {
        public MeshRenderer meshRenderer;

        private void Reset()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void RenderParticles(ParticleEfficientContainer _particlesContainer)
        {
            var width = _particlesContainer.Size.x;
            var height = _particlesContainer.Size.y;

            var texture = new Texture2D(width, height);
            var colors = new Color[width * height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Particle particle = _particlesContainer.GetParticleByLocalPosition(new Vector2Int(x, y));
                    colors[x + y * width] = particle.Color;
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.SetPixels(colors);
            texture.Apply();

            meshRenderer.material.mainTexture = texture;
        }
    }
}