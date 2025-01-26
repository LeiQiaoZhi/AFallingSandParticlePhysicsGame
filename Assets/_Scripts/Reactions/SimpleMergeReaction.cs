using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts.Reactions
{
    [CreateAssetMenu(fileName = "Simple Merge", menuName = "Reactions/Merge")]
    public class SimpleMergeReaction : Reaction
    {
        public ParticleType selfBecomes;
        public ParticleType targetBecomes;
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
                    _selfParticle.SetType(selfBecomes);
                    targetParticle.SetType(targetBecomes);
                    return;
                }
            }
        }
    }
}