using _Scripts.ParticleTypes;
using MyHelpers;
using UnityEngine;

namespace _Scripts.Reactions
{
    [CreateAssetMenu(fileName = "Simple Merge", menuName = "Reactions/Merge")]
    public class SimpleMergeReaction : Reaction
    {
        public Optional<ParticleType> selfBecomes;
        public Optional<ParticleType> targetBecomes;
        [Range(0, 1)] public float chance = 1;

        public override void React(ParticleEfficientContainer _particlesContainer, Particle _selfParticle,
            Vector2Int _position)
        {
            if (_selfParticle.Updated) return;

            foreach (Vector2Int point in PointsToTest(_position))
            {
                Particle targetParticle = _particlesContainer.GetParticleByLocalPosition(point);
                if (CheckTarget(targetParticle) && Random.value < chance)
                {
                    if (selfBecomes.Enabled)
                        _selfParticle.SetType(selfBecomes.Value);
                    if (targetBecomes.Enabled)
                        targetParticle.SetType(targetBecomes.Value);
                    return;
                }
            }
        }
    }
}