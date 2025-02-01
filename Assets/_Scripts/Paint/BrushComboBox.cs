using System.Collections.Generic;
using MyHelpers.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Paint
{
    public class BrushComboBox : MonoBehaviour
    {
        public BrushSet brushSet;
        public GameObject buttonPrefab;
        public IntReference selectedButtonIndex;

        private List<TextMeshProUGUI> texts = new();
        private List<Image> images = new();

        private void Start()
        {
            // spawn buttons
            var types = brushSet.brushes;
            for (var i = 0; i < types.Count; i++)
            {
                GameObject button = Instantiate(buttonPrefab, transform);
                button.GetComponent<Image>().color = Color.white;
                var text = button.GetComponentInChildren<TextMeshProUGUI>();
                if (text)
                {
                    text.text = types[i].brushName;
                    text.color = Color.black;
                    texts.Add(text);
                }

                var image = button.transform.GetChild(0).GetComponent<Image>();
                if (image)
                {
                    image.sprite = types[i].brushIcon;
                    image.color = Color.black;
                    images.Add(image);
                }

                var iCopy = i;
                button.GetComponent<Button>().onClick.AddListener(() => { SetSelectedButton(iCopy); });
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

            for (var i = 0; i < images.Count; i++)
            {
                Color newImageColor = images[i].color;
                newImageColor.a = i == _index ? 1 : 0.3f;
                images[i].color = newImageColor;
            }
        }
    }
}