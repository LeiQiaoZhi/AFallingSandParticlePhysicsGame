using MyHelpers.Grid;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "EmptyParticle", menuName = "Particles/EmptyParticle", order = 0)]
    public class EmptyParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet)
        {
        }
    }
}