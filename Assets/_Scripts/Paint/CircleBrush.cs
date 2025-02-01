using UnityEngine;

namespace _Scripts.Paint
{
    [CreateAssetMenu(menuName = "Paint/Circle Brush", fileName = "Circle Brush")]
    public class CircleBrush : Brush
    {
        public override bool IsInsideMask(Vector2 _uv)
        {
            return (_uv - Vector2.one * 0.5f).sqrMagnitude < 0.25f;
        }
    }
}