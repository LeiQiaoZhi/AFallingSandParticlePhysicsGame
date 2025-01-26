using MyBox;
using MyHelpers.Lifecycle;
using MyHelpers.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class IntVariableSyncWithSlider : InvokeBasedOnLifeCycle
    {
        public IntVariable variable;

        private Slider slider;

        protected override void MethodToInvoke()
        {
            Sync();
        }

        protected override void Reset()
        {
            whenToInvoke = UnityLifeCycle.Update;
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
            if (variable == null) return;
            variable.Value = (int)slider.value;
        }

        private void SetUp()
        {
            slider = GetComponent<Slider>();
        }
    }
}