using System;
using System.Collections.Generic;
using _Scripts.ParticleTypes;
using MyHelpers.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Save
{
    public class SaveManager : MonoBehaviour
    {
        public ParticleEfficientContainer particlesContainer;
        public ParticleTypeSet particleTypeSet;
        public IntReference selectedSlotIndex;
        public List<Button> slotButtons;
        public StringReference status;

        private void Start()
        {
            SetSlot(selectedSlotIndex.Value);
            Load();
        }

        public void SetSlot(int _slotIndex)
        {
            selectedSlotIndex.Value = _slotIndex;
            for (int i = 0; i < slotButtons.Count; i++)
            {
                ColorBlock block = slotButtons[i].colors;
                block.normalColor = i == _slotIndex ? Color.green : Color.white;
                block.selectedColor = i == _slotIndex ? Color.green : Color.white;
                block.pressedColor = i == _slotIndex ? Color.green : Color.white;
                slotButtons[i].colors = block;
            }

            status.Value = $"Selected slot: {_slotIndex + 1}";
        }

        public void Save()
        {
            status.Value = $"Saving to slot {selectedSlotIndex.Value + 1}";
            try
            {
                Saver.SaveData(particlesContainer, particleTypeSet, $"slot{selectedSlotIndex.Value + 1}");
                status.Value = $"Saved to slot {selectedSlotIndex.Value + 1}";
            }
            catch (Exception e)
            {
                status.Value = $"Error: {e.Message}";
            }
        }

        public void Load()
        {
            status.Value = $"Loading from slot {selectedSlotIndex.Value + 1}";
            try
            {
                SerializableContainer data = Saver.LoadData($"slot{selectedSlotIndex.Value + 1}");
                if (data == null)
                {
                    status.Value = $"No data found in slot {selectedSlotIndex.Value + 1}";
                    return;
                }

                status.Value = $"Loaded from slot {selectedSlotIndex.Value + 1}";

                particlesContainer.Load(data, particleTypeSet);
            }
            catch (Exception e)
            {
                status.Value = $"Error: {e.Message}";
            }
        }
    }
}