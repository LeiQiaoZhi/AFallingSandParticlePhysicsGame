using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "WaterParticle", menuName = "Particles/WaterParticle", order = 3)]
    public class WaterParticle : ParticleType
    {
        public override void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet)
        {
            var water = (WaterParticle)_particle.ParticleType;
            var empty = (EmptyParticle)_particleTypeSet.GetInstanceByType(typeof(EmptyParticle));

            Particle bottom = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down);
            if (bottom == null) return; // Hit the bottom of the grid
            if (bottom.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                bottom.SetType(water);
                return;
            }

            Particle bottomLeft =
                _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down + Vector2Int.left);
            if (bottomLeft != null && bottomLeft.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                bottomLeft.SetType(water);
                return;
            }

            Particle bottomRight =
                _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.down + Vector2Int.right);
            if (bottomRight != null && bottomRight.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                bottomRight.SetType(water);
                return;
            }

            // also check left and right
            Particle left = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.left);
            if (left != null && left.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                left.SetType(water);
                return;
            }

            Particle right = _particleContainer.GetParticleByLocalPosition(_position + Vector2Int.right);
            if (right != null && right.ParticleType is EmptyParticle)
            {
                _particle.SetType(empty);
                right.SetType(water);
            }
        }

        private void SwapParticleType(Particle _a, Particle _b)
        {
            ParticleType temp = _a.ParticleType;
            _a.SetType(_b.ParticleType);
            _b.SetType(temp);
        }

        private Vector2Int[] LinePoints(Vector2Int _from, Vector2Int _to)
        {
            var points = new Vector2Int[Mathf.Max(Mathf.Abs(_to.x - _from.x), Mathf.Abs(_to.y - _from.y))];
            for (var i = 0; i < points.Length; i++)
            {
                Vector2 vec = Vector2.Lerp(_from, _to, i / (points.Length - 1));
                points[i] = new Vector2Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
            }
            return points;
        }

        private Vector2Int TryMoveToTarget(Particle _particle, Vector2Int _position, Vector2Int _targetPosition,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet)
        {
            var points = LinePoints(_position, _targetPosition);
            var index = 0;
            for (var i = 1; i < points.Length; i++)
            {
                Vector2Int point = points[i];
                Particle particle = _particleContainer.GetParticleByLocalPosition(point);
                if (particle == null || particle.ParticleType is SandParticle)
                {
                    break;
                }
                index = i;
            }
            return points[index];
        }
    }
}