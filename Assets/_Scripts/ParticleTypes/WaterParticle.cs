using MyHelpers.Grid;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "WaterParticle", menuName = "Particles/WaterParticle", order = 3)]
    public class WaterParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            GenericGridContainer<Particle> _gridContainer, ParticleTypeSet _particleTypeSet)
        {
            var water = (WaterParticle)_particle.ParticleType();
            var empty = (EmptyParticle)_particleTypeSet.GetInstanceByType(typeof(EmptyParticle));

            Particle bottom = _gridContainer.GetObjectByCell(_position + Vector2Int.down);
            if (bottom == null) return; // Hit the bottom of the grid
            if (bottom.ParticleType() is EmptyParticle)
            {
                _particle.SetType(empty);
                bottom.SetType(water);
                return;
            }

            Particle bottomLeft = _gridContainer.GetObjectByCell(_position + Vector2Int.down + Vector2Int.left);
            if (bottomLeft != null && bottomLeft.ParticleType() is EmptyParticle)
            {
                _particle.SetType(empty);
                bottomLeft.SetType(water);
                return;
            }

            Particle bottomRight = _gridContainer.GetObjectByCell(_position + Vector2Int.down + Vector2Int.right);
            if (bottomRight != null && bottomRight.ParticleType() is EmptyParticle)
            {
                _particle.SetType(empty);
                bottomRight.SetType(water);
            }
            
            // also check left and right
            Particle left = _gridContainer.GetObjectByCell(_position + Vector2Int.left);
            if (left != null && left.ParticleType() is EmptyParticle)
            {
                _particle.SetType(empty);
                left.SetType(water);
                return;
            }

            Particle right = _gridContainer.GetObjectByCell(_position + Vector2Int.right);
            if (right != null && right.ParticleType() is EmptyParticle)
            {
                _particle.SetType(empty);
                right.SetType(water);
            }
        }
    }
}