using MyBox;
using MyHelpers;
using MyHelpers.Lifecycle;
using MyHelpers.Variables;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    [ExecuteInEditMode]
    public class TextSyncWithVariable : InvokeBasedOnLifeCycle
    {
        public TMP_Text text;
        [SerializeReference] public VariableBase variable;
        public Optional<StringReference> prefix = new(false, new StringReference(""));
        public Optional<StringReference> suffix = new(false, new StringReference(""));

        protected override void MethodToInvoke()
        {
            Sync();
        }

        protected override void Reset()
        {
            whenToInvoke = UnityLifeCycle.Update;
            text = GetComponent<TMP_Text>();
            base.Reset();
        }


        [ButtonMethod]
        private void Sync()
        {
            if (!text || !variable) return;

            var prefixString = prefix.Enabled ? prefix.Value.Value : "";
            var suffixString = suffix.Enabled ? suffix.Value.Value : "";
            text.text = $"{prefixString}{variable.GetValueAsString()}{suffixString}";
        }
    }
}