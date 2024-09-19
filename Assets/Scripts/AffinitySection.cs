using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace AffinityEditor {
    public class AffinitySection : Selectable {
        public Image background;
        public LayoutElement layoutElement;

        [Header("Auto Assigned")]
        public AffinitySectionData sectionData;
        public AffinityItem parentItem;

        public void Initialize(float previousWidth) {
            this.background.color = this.sectionData.color;
            this.layoutElement.flexibleWidth = this.sectionData.value - previousWidth;
        }

        public override void OnPointerEnter(PointerEventData eventData) {
            parentItem.parentPage.sectionTooltip.ShowTooltip(this);
        }

        public override void OnPointerExit(PointerEventData eventData) {
            parentItem.parentPage.sectionTooltip.HideTooltip();
        }
    }

    [System.Serializable]
    public class AffinitySectionData {
        public string name;
        public float value;
        public Color color;
    }
}