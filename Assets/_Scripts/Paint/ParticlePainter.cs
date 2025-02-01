using System.Collections.Generic;
using _Scripts.ParticleTypes;
using MyHelpers;
using MyHelpers.Variables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Scripts.Paint
{
    public class ParticlePainter : MonoBehaviour
    {
        [SerializeField] private ParticleEfficientContainer particlesContainer;
        public ParticleTypeSet particleTypes;
        public IntReference selectedButtonIndex;
        [Range(0, 120)] public float updatesPerSecond = 30;

        [Header("Brush Settings")] public IntReference brushSize;
        public FloatReference brushDensity;
        public SpriteRenderer brushPreview;
        [FormerlySerializedAs("brushes")] public BrushSet brushSet;
        public IntReference selectedBrushIndex;

        private Vector2 mousePosition;
        private Vector2 mouseWorldPosition;
        private bool isPainting;
        private float nextUpdateTime;

        private void Update()
        {
            mouseWorldPosition = Helpers.Camera.ScreenToWorldPoint(mousePosition);
            UpdateBrushPreview(mouseWorldPosition);

            if (isPainting)
            {
                if (Time.time < nextUpdateTime) return;
                nextUpdateTime = Time.time + 1f / updatesPerSecond;

                Vector2Int center = particlesContainer.WorldToLocalPosition(mouseWorldPosition);
                Vector2Int bottomLeft = center - Vector2Int.one * brushSize.Value;
                Vector2Int topRight = center + Vector2Int.one * brushSize.Value;
                
                Brush brush = brushSet.brushes[selectedBrushIndex.Value];

                for (var x = bottomLeft.x; x <= topRight.x; x++)
                {
                    for (var y = bottomLeft.y; y <= topRight.y; y++)
                    {
                        var position = new Vector2Int(x, y);
                        if (!brush.IsInsideMask(position, bottomLeft, topRight)) continue;
                        if (Random.value > brushDensity.Value) continue;

                        Particle particle = particlesContainer.GetParticleByLocalPosition(position);
                        if (particle != null)
                        {
                            particle.SetType(particleTypes.particleTypes[selectedButtonIndex.Value]);
                        }
                    }
                }
            }
        }

        private void UpdateBrushPreview(Vector2 _mouseWorldPosition)
        {
            // only show brush preview if mouse is inside the canvas
            Vector2 bottomLeft = particlesContainer.BottomLeftWorldPosition;
            Vector2 topRight = particlesContainer.TopRightWorldPosition;
            if (_mouseWorldPosition.x < bottomLeft.x || _mouseWorldPosition.x > topRight.x ||
                _mouseWorldPosition.y < bottomLeft.y || _mouseWorldPosition.y > topRight.y)
            {
                brushPreview.gameObject.SetActive(false);
                Cursor.visible = true;
                return;
            }

            brushPreview.gameObject.SetActive(true);
            Cursor.visible = false;
            brushPreview.transform.position = _mouseWorldPosition;
            brushPreview.transform.localScale = particlesContainer.CellWorldSize * (brushSize.Value * 2 + 1);
            
            Material brushMaterial = brushSet.brushes[selectedBrushIndex.Value].brushPreviewMaterial;
            brushPreview.material = brushMaterial;
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