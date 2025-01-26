using UnityEngine;

namespace _Scripts.Reactions
{
    [CreateAssetMenu(fileName = "Simple Movement", menuName = "Reactions/Movement")]
    public class SimpleMovementReaction : Reaction
    {
        public override void React(ParticleEfficientContainer _particlesContainer, Particle _selfParticle,
            Vector2Int _position)
        {
            if (_selfParticle.Updated) return;

            foreach (Vector2Int point in PointsToTest(_position))
            {
                Particle targetParticle = _particlesContainer.GetParticleByLocalPosition(point);
                if (CheckTarget(targetParticle))
                {
                    _particlesContainer.Swap(_position, point);
                    return;
                }
            }
        }

    }
}