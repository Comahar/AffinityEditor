using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AffinityEditor {
    public class CloseButton : SelectableButton {
        public float hoverIntensity = 0.9f;
        public float baseIntensity = 0.8f;
        public AffinityEditorPage affinityEditorPage;
        
        protected override void Awake() {
            this._hoverIntensity = this.hoverIntensity;
            this._baseIntensity = this.baseIntensity;
            base.Awake();
        }

        public override void OnSubmit() {
            base.OnSubmit();
            affinityEditorPage.Toggle();
        }
    }
}
