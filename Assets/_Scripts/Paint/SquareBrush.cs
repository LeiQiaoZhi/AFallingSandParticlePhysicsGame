using UnityEngine;

namespace _Scripts.Paint
{
    [CreateAssetMenu(menuName = "Paint/Square Brush", fileName = "Square Brush")]
    public class SquareBrush : Brush
    {
        public override bool IsInsideMask(Vector2 _uv)
        {
            return true;
        }
    }
}