using System;
using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public struct SerializableParticle
    {
        public Color color;
        public int particleTypeIndex;
        public Vector2 velocity;
        public float corrosion;
        public float timeAlive;
    }

    [Serializable]
    public class Particle
    {
        // states
        private Color color;
        private ParticleType particleType;
        private Vector2 velocity = Vector2.zero;
        private float corrosion = 0;
        private float timeAlive = 0;

        // properties
        public Color Color { get; set; }
        public bool Updated { get; set; }
        public ParticleType ParticleType => particleType;
        public float Corrosion => corrosion;
        public float TimeAlive => timeAlive;

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }


        #region API

        public void SetType(ParticleType _particleType)
        {
            // Debug.Log($"Setting type to {_particleType}");
            particleType = _particleType;
            Color = _particleType.Color;
            Updated = true;
        }

        public void Step(Vector2Int _position, ParticleEfficientContainer _particleContainer,
            ParticleTypeSet _particleTypeSet, float _dt)
        {
            if (Updated) return;
            particleType.Step(this, _position, _particleContainer, _particleTypeSet, _dt);
        }


        public bool Corrode(float _corrosion)
        {
            corrosion += _corrosion;
            if (corrosion >= particleType.corrosionResistance)
            {
                return true;
            }

            return false;
        }

        public bool ReduceLifetime(float _dt)
        {
            timeAlive += _dt;
            if (timeAlive >= particleType.lifetime.Value)
            {
                return true;
            }

            return false;
        }

        #endregion API

        public void SetCorrosion(float _getSingle)
        {
            corrosion = _getSingle;
        }

        public void SetTimeAlive(float _getSingle)
        {
            timeAlive = _getSingle;
        }
        
        public SerializableParticle Serialize(ParticleTypeSet _particleTypeSet)
        {
            return new SerializableParticle
            {
                color = color,
                particleTypeIndex = _particleTypeSet.GetIndexByInstance(particleType),
                velocity = velocity,
                corrosion = corrosion,
                timeAlive = timeAlive
            };
        }
        
        public void Deserialize(SerializableParticle _data, ParticleTypeSet _particleTypeSet)
        {
            color = _data.color;
            particleType = _particleTypeSet.GetInstanceByIndex(_data.particleTypeIndex);
            SetType(particleType);
            velocity = _data.velocity;
            corrosion = _data.corrosion;
            timeAlive = _data.timeAlive;
        }
    }
}