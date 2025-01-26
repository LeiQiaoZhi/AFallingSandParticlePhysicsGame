using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts.Save
{
    public class SaveManager : MonoBehaviour
    {
        public ParticleEfficientContainer particlesContainer;
        public ParticleTypeSet particleTypeSet;

        public void Save()
        {
            Saver.SaveData(particlesContainer, particleTypeSet);
        }
        
        public void Load()
        {
            SerializableContainer data = Saver.LoadData();
            if (data == null) return;
            
            particlesContainer.Load(data, particleTypeSet);
        }
    }
}