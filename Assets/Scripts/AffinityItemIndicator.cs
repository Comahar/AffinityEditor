using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace AffinityEditor {
    public class AffinityItemIndicator : SelectableElement {
        public LayoutElement layoutElement;
        public Image iconImage;

        [Header("Auto Assigned")]
        public AffinityItem parentItem;
        public AffinityItemIndicatorData itemIndicatorData;

        protected override void Awake() {
            base.Awake();
            //this._selectOnPointerEnter = true;
        }

        protected override void Start() {
            base.Start();
        }

        /*public override void OnPointerEnter(PointerEventData eventData) {
            base.OnPointerEnter(eventData);
            this.parentItem.parentPage.tooltip.ShowTooltip(this);
        }

        public override void OnPointerExit(PointerEventData eventData) {
            base.OnPointerExit(eventData);
            this.parentItem.parentPage.tooltip.HideTooltip();
        }*/

        public override void OnSelected() {
            base.OnSelected();
            this.parentItem.parentPage.tooltip.ShowTooltip(this);
        }

        public override void OnBeginHover() {
            base.OnBeginHover();
            this.parentItem.parentPage.tooltip.ShowTooltip(this);
        }

        public override void OnEndHover() {
            base.OnEndHover();
            this.parentItem.parentPage.tooltip.HideTooltip();
        }
    }

    [System.Serializable]
    public class AffinityItemIndicatorData {
        public float value;
        public string name;
        public string description;
        //public Sprite icon;
    }
}