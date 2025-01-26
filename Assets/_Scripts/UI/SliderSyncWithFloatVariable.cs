using MyBox;
using MyHelpers.Lifecycle;
using MyHelpers.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderSyncWithFloatVariable : InvokeBasedOnLifeCycle
    {
        public FloatReference value;
        public FloatReference minValue;
        public FloatReference maxValue;

        private Slider slider;

        protected override void MethodToInvoke()
        {
            Sync();
        }
        
        protected override void Reset()
        {
            whenToInvoke = UnityLifeCycle.Awake | UnityLifeCycle.Reset | UnityLifeCycle.Update;
            SetUp();
            base.Reset();
        }

        protected override void OnValidate()
        {
            SetUp();
            base.OnValidate();
        }

        protected override void Awake()
        {
            SetUp();
            base.Awake();
        }

        [ButtonMethod]
        private void Sync()
        {
            if (value == null || slider == null) return;
            slider.minValue = minValue.Value;
            slider.maxValue = maxValue.Value;
            slider.value = value.Value;
        }

        private void SetUp()
        {
            if (slider == null)
                slider = GetComponent<Slider>();
        }
    }
}