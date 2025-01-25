using _Scripts.ParticleTypes;
using MyHelpers;
using MyHelpers.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class ParticlePainter : MonoBehaviour
    {
        [SerializeField] private ParticleGridContainer gridContainer;
        public ParticleTypeSet particleTypes;
        private Vector2 mousePosition;
        private bool isPainting;
        public IntReference selectedButtonIndex;

        private void Update()
        {
            if (isPainting)
            {
                Vector3 mouseWorldPosition = Helpers.Camera.ScreenToWorldPoint(mousePosition);
                mouseWorldPosition.z = 0;
                Particle particle = gridContainer.GetObjectByWorld(mouseWorldPosition);
                if (particle != null)
                {
                    Debug.Log("Painting");
                    particle.SetType(particleTypes.particleTypes[selectedButtonIndex.Value]);
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
    }
}