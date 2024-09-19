using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace AffinityEditor {
    [BepInPlugin(GUID: PLUGIN_GUID, Name: PLUGIN_NAME, Version: PLUGIN_VERSION)]
    [BepInDependency("MeteorCore", "0.0.2")]
    [BepInProcess("Goodbye Volcano High.exe")]
    public class Plugin : BaseUnityPlugin {
        public const string PLUGIN_GUID = "AffinityEditor";
        public const string PLUGIN_NAME = "Affinity Editor";
        public const string PLUGIN_VERSION = "0.0.1";

        internal static new ManualLogSource Logger;
        internal static Harmony harmony;
        internal static BepInPlugin metadata;


        private AffinityItemsData affinityItemsData;
        public static Affinity affinity;
        public static TMPro.TMP_FontAsset font { get; private set; }

        private void Awake() {
            metadata = MetadataHelper.GetMetadata(this);

            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();

            // Load AssetBundle
            try {
                string path = Path.Combine(Path.GetDirectoryName(this.Info.Location), "affinityeditor.assetbundle");
                AssetBundleHelper.LoadBundle(path);
            } catch (System.Exception e) {
                Logger.LogError($"AssetBundle load failed: {e}");
                return;
            }
            //bundle.Unload(false);
            Logger.LogInfo($"AssetBundle loaded");

            // Load json
            string json = File.ReadAllText(Path.Combine(Path.GetDirectoryName(this.Info.Location), "affinityEditorData.json"));
            if (string.IsNullOrEmpty(json)) {
                Logger.LogError("affinityEditorData.json is empty");
            }
            try {
                this.affinityItemsData = JsonConvert.DeserializeObject<AffinityItemsData>(json);
            } catch (System.Exception e) {
                Logger.LogError($"Cloud not parse affinityEditorData.json\nContent:\n{json}\nError: {e}");
                return;
            }
            Logger.LogInfo($"affinityEditorData.json imported, items count: {affinityItemsData.items.Count}");
            if (affinityItemsData.items.Count == 0) {
                Logger.LogError($"No items in affinityEditorData.json\nContent:\n{json}");
                return;
            }

            SceneManager.sceneLoaded += OnScneLoaded;
        }

        private void OnScneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == "title" || scene.name == "splash")
                return;

            // find font asset
            TMPro.TextMeshProUGUI text = GameObject.Find("UI_MASTER/UICanvas/SafeArea/PausePanel/PauseMenu/TabIconsRow/RowParent/Row2/text label").GetComponent<TMPro.TextMeshProUGUI>();
            if (text != null) {
                Plugin.font = text.font;
            }
            
            CreateEditor();
        }

        private void SetChildrenFont(Transform transform) {
            if (Plugin.font == null) {
                Plugin.Logger.LogWarning("font is null, skipping font setting");
                return;
            }
            foreach (Transform child in transform) {
                if (child.GetComponent<TMPro.TextMeshProUGUI>() != null) {
                    child.GetComponent<TMPro.TextMeshProUGUI>().font = Plugin.font;
                }
                SetChildrenFont(child);
            }
        }

        private void CreateEditor() {
            Logger.LogInfo($"Creating Affinty Editor");

            Transform pageContent = GameObject.Find("UI_MASTER/UICanvas/SafeArea/PausePanel/PausePage_Affinity/PageContent").transform;
            affinity = GameObject.Find("UI_MASTER/UICanvas/SafeArea/PausePanel/PausePage_Affinity").GetComponent<Affinity>();

            // create editor page from assetbundle
            GameObject editorPagePrefab = AssetBundleHelper.LoadAsset<GameObject>("AffinityEditorPage");
            GameObject editorPage = Instantiate(editorPagePrefab, pageContent);

            RectTransform editorPageRect = editorPage.GetComponent<RectTransform>();
            editorPageRect.offsetMin = new Vector2(155, -960);
            editorPageRect.offsetMax = new Vector2(1420, 1000);

            AffinityEditorPage affinityEditorPage = editorPage.GetComponent<AffinityEditorPage>();
            affinityEditorPage.affinityItemsData = this.affinityItemsData;
            affinityEditorPage.CreateAffinityItems();
            SetChildrenFont(affinityEditorPage.transform);

            affinityEditorPage.gameObject.SetActive(false);

            // create toggleButton
            GameObject toggleButton = Instantiate(AssetBundleHelper.LoadAsset<GameObject>("AffinityEditorButton"), pageContent.Find("AffinityVisualization/rings"));
            //toggleButton.transform.position = new Vector3(1070, 340, 0);
            RectTransform toggleButtonTransfrom = toggleButton.GetComponent<RectTransform>();
            toggleButtonTransfrom.anchorMin = new Vector2(0.5f, 0.5f);
            toggleButtonTransfrom.anchorMax = new Vector2(0.5f, 0.5f);
            toggleButtonTransfrom.anchoredPosition = new Vector2(1330/2-toggleButtonTransfrom.sizeDelta.x/2, -(1330/2-toggleButtonTransfrom.sizeDelta.y/2));

            toggleButton.GetComponent<EditorToggleButton>().affinityEditorPage = affinityEditorPage;
        }
    }
}