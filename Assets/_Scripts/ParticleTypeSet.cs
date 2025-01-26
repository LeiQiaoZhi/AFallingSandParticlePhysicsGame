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
    }
}