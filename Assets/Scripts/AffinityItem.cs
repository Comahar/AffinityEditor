using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AffinityEditor {
    public class AffinityItem : MonoBehaviour {
        public Slider slider;
        public Image icon;
        public TMP_InputField valueTextInput;
        public Transform indicatorsContainer;
        public GameObject affinityItemIndicatorPrefab;
        public GameObject sectionPrefab;
        public Transform sectionContainer;

        [Header("Auto Assigned")]
        public AffinityEditorPage parentPage;
        public List<AffinityItemIndicator> itemIndicators = new List<AffinityItemIndicator>();
        public List<AffinitySection> sections = new List<AffinitySection>();
        public AffinityItemData itemData;

        public int value = 0;

        public void Start() {
            #if !UNITY_EDITOR
                object value = Mgr_InkVariables.GetVariableValue(this.itemData.inkVariableName);
                if (value == null){
                    Plugin.Logger.LogWarning($"Could not find ink variable {this.itemData.inkVariableName}");
                    return;
                }
                this.value = (int)value;
                this.Refresh();
                InkMaster.ActiveInstance.InkStory.ObserveVariable(this.itemData.inkVariableName, this.OnInkVariableChanged);
            #endif

            this.slider.onValueChanged.AddListener(this.OnSliderValueChanged);
            this.valueTextInput.onEndEdit.AddListener(this.OnTextInputValueChanged);
        }

        public void OnEnable() {
            #if !UNITY_EDITOR
                // if this is not done rounded corners breaks after a toggle
                HarmonyLib.AccessTools.Method(typeof(RoundRectAuto), "reassignAllProps").Invoke(this.slider.GetComponent<RoundRectAuto>(), null);
            #endif
        }

        public void Initialize(){
            //Plugin.Logger.LogWarning($"{this.gameObject.name}: icon null {this.icon == null}, itemData null {this.itemData == null}, itemData.icon null {this.itemData.icon == null}");
            this.icon.sprite = this.itemData.icon;
            this.slider.maxValue = this.itemData.sections.Last().value;
            CreateItemIndicators();
            CreateSections();
        }

        public void CreateItemIndicators() {
            float currentWidth = 0;
            foreach (var itemIndicatorData in this.itemData.itemIndicators) {
                var itemIndicator = Instantiate(this.affinityItemIndicatorPrefab);
                itemIndicator.name = "AffinityItemIndicator";
                itemIndicator.transform.SetParent(this.indicatorsContainer);
                itemIndicator.transform.localScale = Vector3.one;
                var component = itemIndicator.GetComponent<AffinityItemIndicator>();
                component.itemIndicatorData = itemIndicatorData;
                component.layoutElement.flexibleWidth = itemIndicatorData.value - currentWidth;
                component.parentItem = this;
                currentWidth = itemIndicatorData.value;
                this.itemIndicators.Add(itemIndicator.GetComponent<AffinityItemIndicator>());
            }
            if (itemData.sections.Last().value - currentWidth != 0) {
                var paddingItem = Instantiate(this.affinityItemIndicatorPrefab);
                paddingItem.name = "RightPadding";
                paddingItem.transform.SetParent(this.indicatorsContainer);
                paddingItem.transform.localScale = Vector3.one;
                paddingItem.GetComponent<AffinityItemIndicator>().layoutElement.flexibleWidth = itemData.sections.Last().value - currentWidth;
                paddingItem.GetComponent<AffinityItemIndicator>().enabled = false;
                paddingItem.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        private void CreateSections() {
            foreach (var section in this.sections) {
                if (Application.isEditor)
                    DestroyImmediate(section);
                else
                    Destroy(section);
            }
            sections.Clear();

            float currentWidth = 0;
            foreach (var sectionData in this.itemData.sections) {
                var section = Instantiate(this.sectionPrefab, this.sectionContainer);
                section.name = "SliderBackground";
                section.transform.localScale = Vector3.one;
                var component = section.GetComponent<AffinitySection>();
                component.sectionData = sectionData;
                component.parentItem = this;
                component.Initialize(currentWidth);
                sections.Add(component);
                currentWidth = sectionData.value;
            }
        }

        public void OnInkVariableChanged(string variableName, object value) {
            if (variableName == this.itemData.inkVariableName) {
                this.value = (int)value;
                this.Refresh();
            }
        }

        public void OnSliderValueChanged(float value) {
            if (this.parentPage.editLockButton.isLocked){
                this.parentPage.editLockButton.Blink();
                this.slider.value = this.value;
                return;
            }
            this.value = (int)value;
            this.Refresh();
            this.SetInkVariable();
        }

        public void OnTextInputValueChanged(string text) {
            if (this.parentPage.editLockButton.isLocked){
                this.parentPage.editLockButton.Blink();
                this.valueTextInput.text = this.value.ToString();
                return;
            }
            if (int.TryParse(text, out int value)) {
                this.value = value;
                this.Refresh();
                this.SetInkVariable();
            } else {
                this.valueTextInput.text = this.value.ToString();
            }
        }

        public void Refresh() {
            //this.slider.value = this.value;
            this.slider.SetValueWithoutNotify(this.value);
            this.valueTextInput.text = this.value.ToString();
        }

        public void SetInkVariable() {
            #if !UNITY_EDITOR
                Mgr_InkVariables.Instance.ChangeGlobalVar(this.itemData.inkVariableName, this.value, true);
                HarmonyLib.AccessTools.Method(typeof(Affinity), "RefreshCharacterTiers").Invoke(Plugin.affinity, null);
            #endif
        }
    }

    [System.Serializable]
    public class AffinityItemData {
        public string name;
        public string iconAssetName;
        public string inkVariableName;
        [JsonIgnore]
        public Sprite icon { get { return AssetBundleHelper.LoadAsset<Sprite>(iconAssetName); } }
        public List<AffinityItemIndicatorData> itemIndicators = new List<AffinityItemIndicatorData>();
        public List<AffinitySectionData> sections = new List<AffinitySectionData>();
    }
}