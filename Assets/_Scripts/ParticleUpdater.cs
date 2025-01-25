using System;
using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts
{
    public class ParticleUpdater : MonoBehaviour
    {
        public ParticleGridFiller gridFiller;
        public ParticleTypeSet particleTypeSet;
        [Range(1, 120)]
        public int updatesPerSecond = 30;

        private float nextUpdateTime;
        
        private void Start()
        {
            gridFiller.FillGrid();

            for (var x = gridFiller.Mins.x; x <= gridFiller.Maxs.x; x++)
            {
                for (var y = gridFiller.Mins.y; y <= gridFiller.Maxs.y; y++)
                {
                    Particle particle = gridFiller.gridContainer.GetObjectByCell(new Vector3Int(x, y, 0));
                    if (particle != null)
                    {
                        particle.SetType(particleTypeSet.GetInstanceByType(typeof(EmptyParticle)));
                    }
                }
            }
        }

        private void Update()
        {
            if (Time.time < nextUpdateTime) return;
            nextUpdateTime = Time.time + 1f / updatesPerSecond;
            UpdateParticles();
        }

        public void UpdateParticles()
        {
            ForEachParticle((_particle, x, y) =>
            {
                _particle.Step(new Vector2Int(x, y), gridFiller.gridContainer, particleTypeSet);
            });

            // Reset all particles to not updated
            ForEachParticle((_particle, _, _) => _particle.Updated = false);
        }

        private void ForEachParticle(Action<Particle, int, int> _action)
        {
            for (var x = gridFiller.Mins.x; x <= gridFiller.Maxs.x; x++)
            {
                for (var y = gridFiller.Mins.y; y <= gridFiller.Maxs.y; y++)
                {
                    Particle particle = gridFiller.gridContainer.GetObjectByCell(new Vector3Int(x, y, 0));
                    if (particle != null)
                    {
                        _action(particle, x, y);
                    }
                }
            }
        }
    }
}