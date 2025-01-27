using MyBox;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "StaticParticle", menuName = "Particles/StaticParticle", order = 2)]
    public class StaticParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt)
        {
            // react
            reactions.ForEach(_reaction => _reaction.React(_particleContainer, _particle, _position));
        }
    }
}