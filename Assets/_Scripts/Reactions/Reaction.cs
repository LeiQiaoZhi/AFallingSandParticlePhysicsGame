using System.Collections.Generic;
using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts.Reactions
{
    public abstract class Reaction : ScriptableObject
    {
        public enum TargetCheckMethod
        {
            Instance,
            Type
        }
        public TargetCheckMethod targetCheckMethod;
        public ParticleType targetParticleType;
        public List<Vector2Int> directionsToTest;

        public abstract void React(ParticleEfficientContainer _particlesContainer, Particle _selfParticle,
            Vector2Int _position);
        
        protected bool CheckTarget(Particle _targetParticle)
        {
            if (_targetParticle == null) return false;
            switch (targetCheckMethod)
            {
                case TargetCheckMethod.Instance:
                    return _targetParticle.ParticleType == targetParticleType;
                case TargetCheckMethod.Type:
                    return _targetParticle.ParticleType.GetType() == targetParticleType.GetType();
                default:
                    return false;
            }
        }

        protected Vector2Int[] PointsToTest(Vector2Int _position)
        {
            var points = new Vector2Int[directionsToTest.Count];
            for (int i = 0; i < directionsToTest.Count; i++)
            {
                points[i] = _position + directionsToTest[i];
            }

            return points;
        }
    }
}