using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Paint
{
    [CreateAssetMenu(menuName = "Paint/Brush Set", fileName = "Brush Set")]
    public class BrushSet : ScriptableObject
    {
        public List<Brush> brushes;
    }
}