using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "SandParticle", menuName = "Particles/SandParticle", order = 1)]
    public class SandParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            var sand = (SandParticle)_particle.ParticleType;
            var empty = (EmptyParticle)_particleTypeSet.GetInstanceByType(typeof(EmptyParticle));
            var water = (WaterParticle)_particleTypeSet.GetInstanceByType(typeof(WaterParticle));
            
            Particle bottom = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down);
            if (bottom == null) return; // Hit the bottom of the grid
            if (bottom.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                bottom.SetType(sand);
                return;
            }
            if (bottom.ParticleType is WaterParticle)
            {
                _particle.SetType(water);
                bottom.SetType(sand);
                return;
            }

            Particle bottomLeft = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down + Vector2Int.left);
            if (bottomLeft != null && bottomLeft.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                bottomLeft.SetType(sand);
                return;
            }

            if (bottomLeft != null && bottomLeft.ParticleType is WaterParticle)
            {
                _particle.SetType(water);
                bottomLeft.SetType(sand);
                return;
            }

            Particle bottomRight = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down + Vector2Int.right);
            if (bottomRight != null && bottomRight.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                bottomRight.SetType(sand);
                return;
            }
            if (bottomRight != null && bottomRight.ParticleType is WaterParticle)
            {
                _particle.SetType(water);
                bottomRight.SetType(sand);
            }
        }
    }
}