using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AffinityEditor {
    public class InfoText : MonoBehaviour, IPointerClickHandler {
        public AffinityEditorPage parentPage;
        public Dictionary<string, string> infoData;
        public void Start() {
            this.GetComponent<TextMeshProUGUI>().text = parentPage.affinityItemsData.infoText;
        }

        public void OnPointerClick(PointerEventData eventData) {
            TMP_Text textComponent = GetComponent<TMP_Text>();
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textComponent, eventData.position, null);
            if (linkIndex != -1) {
                TMP_LinkInfo linkInfo = textComponent.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}
