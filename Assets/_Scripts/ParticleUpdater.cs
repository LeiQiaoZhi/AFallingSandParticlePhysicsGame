using System;
using _Scripts.ParticleTypes;
using MyBox;
using UnityEngine;

namespace _Scripts
{
    public class ParticleUpdater : MonoBehaviour
    {
        [Header("Dependencies")]
        public ParticleEfficientContainer particlesContainer;
        public ParticlesRenderer particlesRenderer;
        [Header("Settings")]
        public ParticleTypeSet particleTypeSet;
        [Range(1, 120)]
        public int updatesPerSecond = 30;

        private float nextUpdateTime;
        
        private void Awake()
        {
            particlesContainer.Initialize();

            ForEachParticle((_particle, _x, _y) =>
            {
                _particle.SetType(particleTypeSet.GetInstanceByType(typeof(EmptyParticle)));
            });
            
        }

        private void Update()
        {
            if (Time.time < nextUpdateTime) return;
            nextUpdateTime = Time.time + 1f / updatesPerSecond;
            UpdateParticles();
        }

        private void UpdateParticles()
        {
            TimeTest.Start("UpdateParticles", true);
            ForEachParticle((_particle, _x, _y) =>
            {
                _particle.Step(new Vector2Int(_x, _y), particlesContainer, particleTypeSet, 1f / updatesPerSecond);
            });
            TimeTest.End();

            // Reset all particles to not updated
            TimeTest.Start("Reset Particles States", true);
            ForEachParticle((_particle, _, _) => _particle.Updated = false);
            TimeTest.End();
            
            TimeTest.Start("Render Particles", true);
            particlesRenderer.RenderParticles(particlesContainer);
            TimeTest.End();
        }

        private void ForEachParticle(Action<Particle, int, int> _action)
        {
            Vector2Int size = particlesContainer.Size;
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    Particle particle = particlesContainer.GetParticleByLocalPosition(new Vector2Int(x, y));
                    if (particle != null)
                    {
                        _action(particle, x, y);
                    }
                }
            }
        }
    }
}