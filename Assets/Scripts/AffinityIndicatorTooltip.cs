using System;
using UnityEngine;
using TMPro;

namespace AffinityEditor {
    public class AffinityIndicatorTooltip : MonoBehaviour {
        
        public TextMeshProUGUI titleText;
        public GameObject divider;
        public TextMeshProUGUI tooltipText;
        
        public void Start() {
            this.HideTooltip();
        }

        public void ShowTooltip(AffinityItemIndicator tooltipIndicator) {
            // disable if no name or description
            if (string.IsNullOrEmpty(tooltipIndicator.itemIndicatorData.name) && string.IsNullOrEmpty(tooltipIndicator.itemIndicatorData.description)) {
                this.gameObject.SetActive(false);
                return;
            } else {
                this.gameObject.SetActive(true);
            }

            if (string.IsNullOrEmpty(tooltipIndicator.itemIndicatorData.description)) {
                this.divider.SetActive(false);
                this.tooltipText.gameObject.SetActive(false);
            } else {
                this.divider.SetActive(true);
                this.tooltipText.gameObject.SetActive(true);
            }

            this.titleText.text = tooltipIndicator.itemIndicatorData.name;  
            this.tooltipText.text = tooltipIndicator.itemIndicatorData.description;

            // set position
            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(this.transform.parent, tooltipIndicator.iconImage.transform);
            var parentRect = this.transform.parent.GetComponent<RectTransform>();
            var thisRect = this.GetComponent<RectTransform>();
            thisRect.anchorMin = bounds.min / parentRect.sizeDelta + parentRect.pivot;
            thisRect.anchorMax = bounds.max / parentRect.sizeDelta + parentRect.pivot;
            thisRect.offsetMin = Vector2.zero;
            thisRect.offsetMax = Vector2.zero;
        }

        public void HideTooltip() {
            this.gameObject.SetActive(false);
        }
    }
}