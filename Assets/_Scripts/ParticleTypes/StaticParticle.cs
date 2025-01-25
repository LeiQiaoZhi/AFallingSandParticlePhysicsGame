using MyHelpers.Grid;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "StaticParticle", menuName = "Particles/StaticParticle", order = 2)]
    public class StaticParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            GenericGridContainer<Particle> _gridContainer, ParticleTypeSet _particleTypeSet)
        {
        }
    }
}