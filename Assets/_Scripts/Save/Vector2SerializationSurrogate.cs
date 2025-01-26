using System.Runtime.Serialization;
using UnityEngine;

namespace _Scripts.Save
{
    public class Vector2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object _obj, SerializationInfo _info, StreamingContext _context)
        {
            Vector2 vector = (Vector2)_obj;
            _info.AddValue("x", vector.x);
            _info.AddValue("y", vector.y);
        }

        public object SetObjectData(object _obj, SerializationInfo _info, StreamingContext _context, ISurrogateSelector _selector)
        {
            Vector2 vector = (Vector2)_obj;
            vector.x = _info.GetSingle("x");
            vector.y = _info.GetSingle("y");
            return vector;
        }
    }
}