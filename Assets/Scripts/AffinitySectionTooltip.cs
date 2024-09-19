using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AffinityEditor {
    public class AffinitySectionTooltip : MonoBehaviour {
        public TextMeshProUGUI tooltipText;
        public List<Image> backgroundImages = new List<Image>();
        public float bgMultiplier = 1.5f;
        public void ShowTooltip(AffinitySection section) {
            if(string.IsNullOrEmpty(section.sectionData.name)) {
                this.gameObject.SetActive(false);
                return;
            }
            this.gameObject.SetActive(true);
            this.tooltipText.text = section.sectionData.name;
            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(this.transform.parent, section.transform);
            var parentRect = this.transform.parent.GetComponent<RectTransform>();
            var thisRect = this.GetComponent<RectTransform>();
            thisRect.anchorMin = bounds.min / parentRect.sizeDelta + parentRect.pivot;
            thisRect.anchorMax = bounds.max / parentRect.sizeDelta + parentRect.pivot;
            thisRect.offsetMin = Vector2.zero;
            thisRect.offsetMax = Vector2.zero;

            // darken the background
            Color color = section.sectionData.color;
            color *= this.bgMultiplier;
            color.a = 1.0f;
            foreach (var image in backgroundImages) {
                image.color = color;
            }
        }

        public void HideTooltip() {
            this.gameObject.SetActive(false);
            this.tooltipText.text = "";
        }
    }
}