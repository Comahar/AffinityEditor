using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AffinityEditor {
    public class AffinityEditorPage : MonoBehaviour {
        [Header("Affinity Editor Page")]
        public AffinityItemsData affinityItemsData;
        public List<AffinityItem> affinityItems = new List<AffinityItem>();
        public GameObject affinityItemPrefab;
        public Transform itemsContainer;
        public AffinityIndicatorTooltip tooltip;
        public AffinitySectionTooltip sectionTooltip;
        public EditLockButton editLockButton;

        public void Start() {
            //base.Start();
            //if (!Application.isEditor) 
            //    this.CreateAffinityItems();
        }

        public void OnEnable(){
            // refresh else it does not fit the contents
            // hours wasted: 4
            this.StartCoroutine(ContentFitUpdate());
        }

        private IEnumerator ContentFitUpdate(){
            ContentSizeFitter contentSizeFitter = this.GetComponent<ContentSizeFitter>();
            contentSizeFitter.enabled = false;
            yield return new WaitForEndOfFrame();
            contentSizeFitter.enabled = true;
        }

        public void CreateAffinityItems() {
            foreach (var item in this.affinityItems){
                if (item == null) continue;
                if (Application.isEditor)
                    DestroyImmediate(item.gameObject);
                else
                    Destroy(item.gameObject);
            }
            this.affinityItems.Clear();
            foreach (var itemData in this.affinityItemsData.items) {
                this.CreateAffinityItem(itemData);
            }
        }

        public void CreateAffinityItem(AffinityItemData itemData) {
            GameObject item = Instantiate(this.affinityItemPrefab, this.itemsContainer);
            item.name = "AffinityItem " + itemData.name;
            item.transform.localScale = Vector3.one;
            AffinityItem component = item.GetComponent<AffinityItem>();
            component.itemData = itemData;
            component.parentPage = this;
            this.affinityItems.Add(component);
            component.Initialize();
        }

        public void Toggle() {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
            // lock on toggle
            this.editLockButton.SetLock(true);
        }
    }

    [Serializable]
    public class AffinityItemsData {
        public string infoText;
        public List<AffinityItemData> items = new List<AffinityItemData>();
        // public AffinityItemData[] items = {};
    }

}