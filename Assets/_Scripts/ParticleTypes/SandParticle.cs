using MyHelpers.Grid;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "SandParticle", menuName = "Particles/SandParticle", order = 1)]
    public class SandParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet)
        {
            var sandParticle = (SandParticle)_particle.ParticleType();
            var emptyParticle = (EmptyParticle)_particleTypeSet.GetInstanceByType(typeof(EmptyParticle));
            
            Particle bottom = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down);
            if (bottom == null) return; // Hit the bottom of the grid
            if (bottom.ParticleType() is EmptyParticle)
            {
                _particle.SetType(emptyParticle);
                bottom.SetType(sandParticle);
                return;
            }

            Particle bottomLeft = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down + Vector2Int.left);
            if (bottomLeft != null && bottomLeft.ParticleType() is EmptyParticle)
            {
                _particle.SetType(emptyParticle);
                bottomLeft.SetType(sandParticle);
                return;
            }

            Particle bottomRight =
                _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down + Vector2Int.right);
            if (bottomRight != null && bottomRight.ParticleType() is EmptyParticle)
            {
                _particle.SetType(emptyParticle);
                bottomRight.SetType(sandParticle);
            }
        }
    }
}