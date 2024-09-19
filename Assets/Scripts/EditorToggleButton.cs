using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AffinityEditor {
    public class EditorToggleButton : SelectableElement {
        public AffinityEditorPage affinityEditorPage;
        public bool isOpen { get; private set; }

        public void Toggle() {
            affinityEditorPage.Toggle();
        }

        public override void OnSelected() {
            base.OnSelected();
            this.Toggle();
        }
    }
}
