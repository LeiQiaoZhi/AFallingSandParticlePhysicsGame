using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.ParticleTypes
{
    [CreateAssetMenu(fileName = "ParticleTypeSet", menuName = "Particles/ParticleTypeSet", order = 0)]
    public class ParticleTypeSet : ScriptableObject
    {
        public List<ParticleType> particleTypes;

        public ParticleType GetInstanceByType(Type _type)
        {
            foreach (var particleType in particleTypes)
            {
                if (particleType.GetType() == _type)
                {
                    return particleType;
                }
            }

            Debug.LogWarning($"ParticleTypeSet does not contain type {_type}");
            return null;
        }

        public int GetIndexByInstance(ParticleType _particleType)
        {
            for (var i = 0; i < particleTypes.Count; i++)
            {
                if (particleTypes[i] == _particleType)
                {
                    return i;
                }
            }

            Debug.LogWarning($"ParticleTypeSet does not contain instance {_particleType}");
            return -1;
        }
        
        public ParticleType GetInstanceByIndex(int _index)
        {
            if (_index < 0 || _index >= particleTypes.Count)
            {
                Debug.LogWarning($"Index out of bounds: {_index}");
                return null;
            }

            return particleTypes[_index];
        }
    }
}