using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AffinityEditor {
    public class EditLockButton : SelectableButton {
        public Sprite unlockedSprite;
        public Sprite lockedSprite;
        public Image image;
        public bool isLocked {
            get { return _isLocked; }
            private set {
                _isLocked = value;
                if (_isLocked) {
                    this.image.sprite = lockedSprite;
                } else {
                    this.image.sprite = unlockedSprite;
                }
            }
        }
        [SerializeField]
        private bool _isLocked = false;
        
        // button blinking props
        [SerializeField]
        private float blinkInterval = 0.5f;
        [SerializeField]
        private int blinkCount = 3;
        [SerializeField]
        private float stroke = 4;
        
        public AffinityEditorPage affinityEditorPage;
        public RoundRectAuto bg;

        public float hoverIntensity = 0.9f;
        public float baseIntensity = 0.8f;

        protected override void Awake() {
            this._hoverIntensity = this.hoverIntensity;
            this._baseIntensity = this.baseIntensity;
            base.Awake();
        }
        
        protected override void Start() {
            base.Start();
            this.isLocked = true;
            this.image.sprite = this.lockedSprite;
        }

        public override void OnSubmit() {
            base.OnSubmit();
            this.SetLock(!this.isLocked);
        }

        public void SetLock(bool locked){
            this.isLocked = locked;
        }

        public void Blink(){
            StartCoroutine(this.BlinkCoroutine());
        }

        private bool isRunning = false;
        private IEnumerator BlinkCoroutine(){
            if (this.isRunning) yield break;
            isRunning = true;
            for (int i = 0; i < this.blinkCount; i++) {
                bg.stroke = this.stroke;
                yield return new WaitForSeconds(this.blinkInterval);
                bg.stroke = 0;
                yield return new WaitForSeconds(this.blinkInterval);
            }
            isRunning = false;
        }
    }
}
