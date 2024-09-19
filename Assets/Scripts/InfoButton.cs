using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AffinityEditor {
    public class InfoButton : SelectableButton {
        public bool isActive;
        public Transform infoPanel;
        public float hoverIntensity = 0.9f;
        public float baseIntensity = 0.8f;
        
        protected override void Awake() {
            this._hoverIntensity = this.hoverIntensity;
            this._baseIntensity = this.baseIntensity;
            base.Awake();
        }

        protected override void Start() {

            base.Start();
            this.isActive = false;
            this.infoPanel.gameObject.SetActive(false);
        }

        public override void OnSubmit() {
            base.OnSubmit();
            infoPanel.gameObject.SetActive(!infoPanel.gameObject.activeSelf);
        }
    }
}
