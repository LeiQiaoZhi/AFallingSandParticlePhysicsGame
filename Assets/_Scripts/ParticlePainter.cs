using _Scripts.ParticleTypes;
using MyHelpers;
using MyHelpers.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class ParticlePainter : MonoBehaviour
    {
        [SerializeField] private ParticleEfficientContainer particlesContainer;
        public ParticleTypeSet particleTypes;
        public IntReference selectedButtonIndex;
        public IntReference brushSize;
        public FloatReference brushDensity;

        private Vector2 mousePosition;
        private Vector2 mouseWorldPosition;
        private bool isPainting;

        private void Update()
        {
            if (isPainting)
            {
                mouseWorldPosition = Helpers.Camera.ScreenToWorldPoint(mousePosition);
                Vector2Int center = particlesContainer.WorldToLocalPosition(mouseWorldPosition);
                Vector2Int bottomLeft = center - Vector2Int.one * brushSize.Value;
                Vector2Int topRight = center + Vector2Int.one * brushSize.Value;

                for (var x = bottomLeft.x; x <= topRight.x; x++)
                {
                    for (var y = bottomLeft.y; y <= topRight.y; y++)
                    {
                        if (Random.value > brushDensity.Value) continue;
                        
                        Particle particle = particlesContainer.GetParticleByLocalPosition(new Vector2Int(x, y));
                        if (particle != null)
                        {
                            particle.SetType(particleTypes.particleTypes[selectedButtonIndex.Value]);
                        }
                    }
                }
            }
        }

        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isPainting = true;
            }

            if (context.canceled)
            {
                isPainting = false;
            }
        }

        public void OnMouseMoved(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                mousePosition = context.ReadValue<Vector2>();
            }
        }

        private void OnDrawGizmos()
        {
            Vector2 cellWorldSize = particlesContainer.WorldSize / particlesContainer.Size;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(mouseWorldPosition, cellWorldSize * brushSize.Value * 2);
        }
    }
}