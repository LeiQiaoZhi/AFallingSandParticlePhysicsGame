using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using _Scripts.ParticleTypes;
using UnityEngine;

namespace _Scripts.Save
{
    public static class Saver 
    {
        public static void SaveData(ParticleEfficientContainer _particlesContainer, ParticleTypeSet _particleTypeSet,
            string _fileName)
        {
            BinaryFormatter formatter = GetFormatterWithSurrogates();
            var path = Application.persistentDataPath + $"/{_fileName}.sav";

            using (var stream = new FileStream(path, FileMode.Create))
            {
                var serializableParticles = new SerializableParticle[_particlesContainer.Size.x, _particlesContainer.Size.y];
                for (var x = 0; x < _particlesContainer.Size.x; x++)
                {
                    for (var y = 0; y < _particlesContainer.Size.y; y++)
                    {
                        serializableParticles[x, y] = _particlesContainer.Particles[x, y].Serialize(_particleTypeSet);
                    }
                }
                
                var data = new SerializableContainer
                {
                    width = _particlesContainer.Size.x,
                    height = _particlesContainer.Size.y,
                    particles = serializableParticles
                };

                formatter.Serialize(stream, data);
            }

            Debug.Log($"Data saved to {path}");
        }
        
        public static BinaryFormatter GetFormatterWithSurrogates()
        {
            var formatter = new BinaryFormatter();
            var surrogateSelector = new SurrogateSelector();

            surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector2SerializationSurrogate());
            surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), new ColorSerializationSurrogate());

            formatter.SurrogateSelector = surrogateSelector;
            return formatter;
        }
        
        public static SerializableContainer LoadData(string _fileName)
        {
            var path = Application.persistentDataPath + $"/{_fileName}.sav";
            if (!File.Exists(path))
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }

            BinaryFormatter formatter = GetFormatterWithSurrogates();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                // Deserialize the data
                SerializableContainer data = formatter.Deserialize(stream) as SerializableContainer;
                return data;
            }
        }
    }

    [System.Serializable]
    public class SerializableContainer
    {
        public int width;
        public int height;
        public SerializableParticle[,] particles;
    }
}