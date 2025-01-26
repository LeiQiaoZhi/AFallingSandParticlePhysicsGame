using UnityEngine;

namespace _Scripts.ParticleTypes
{
    public abstract class ParticleType : ScriptableObject
    {
        public Color color;
        public string particleName;

        public abstract void Step(Particle _particle, Vector2Int _position,
            ParticleEfficientContainer _particleContainer, ParticleTypeSet _particleTypeSet, float _dt);
    }
}