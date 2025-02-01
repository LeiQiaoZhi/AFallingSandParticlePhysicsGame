using System.Collections.Generic;
using _Scripts.Paint;
using MyHelpers;
using MyHelpers.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ParticlePainterButtons : MonoBehaviour
    {
        public ParticlePainter particlePainter;
        public GameObject buttonPrefab;
        public IntReference selectedButtonIndex;

        private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

        private void Start()
        {
            // spawn buttons
            var types = particlePainter.particleTypes.particleTypes;
            for (var i = 0; i < types.Count; i++)
            {
                GameObject button = Instantiate(buttonPrefab, transform);
                button.GetComponent<Image>().color = types[i].Color;
                var text = button.GetComponentInChildren<TextMeshProUGUI>();
                text.text = types[i].particleName;
                text.color = Helpers.GetBlackWhiteContrastColor(types[i].Color);
                texts.Add(text);

                var iCopy = i;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SetSelectedButton(iCopy);
                });
            }
            
            SetSelectedButton(selectedButtonIndex.Value);
        }
        
        private void SetSelectedButton(int _index)
        {
            selectedButtonIndex.Value = _index;
            for (var i = 0; i < texts.Count; i++)
            {
                Color newColor = texts[i].color;
                newColor.a = i == _index ? 1 : 0.3f;
                texts[i].fontStyle = i == _index ? FontStyles.Bold : FontStyles.Normal;
                texts[i].color = newColor;
            }
        }
    }
}