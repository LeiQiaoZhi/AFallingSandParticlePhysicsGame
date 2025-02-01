using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Paint
{
    public abstract class Brush : ScriptableObject
    {
        public string brushName;
        [FormerlySerializedAs("brushSprite")] public Sprite brushIcon;
        public Material brushPreviewMaterial;

        public abstract bool IsInsideMask(Vector2 _uv);

        public bool IsInsideMask(Vector2 _position, Vector2 _bottomLeft, Vector2 _topRight)
        {
            return IsInsideMask((_position - _bottomLeft) / (_topRight - _bottomLeft));
        }
    }
}