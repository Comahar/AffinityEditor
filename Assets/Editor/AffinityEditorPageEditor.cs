using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace AffinityEditor {
    [CustomEditor(typeof(AffinityEditorPage))]
    public class AffinityEditorPageEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Export JSON")) {
                this.ExportJson();
            }
            if (GUILayout.Button("Import JSON")) {
                this.ImportJson();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Refresh Items")) {
                this.RefreshItemsFromData();
            }
        }

        private void ExportJson() {
            AffinityEditorPage affinityEditorPage = (AffinityEditorPage)this.target;
            string json = JsonUtility.ToJson(affinityEditorPage.affinityItemsData);
            string path = EditorUtility.SaveFilePanel("Save Affinity Editor Data", "", "affinityEditorData.json", "json");
            if(!string.IsNullOrEmpty(path)) {
                File.WriteAllText(path, json);
                Debug.Log("Affinity Editor Data Exported to " + path);
            }
        }

        private void ImportJson() {
            AffinityEditorPage affinityEditorPage = (AffinityEditorPage)this.target;
            string path = EditorUtility.OpenFilePanel("Load Affinity Editor Data", "", "json");
            if(!string.IsNullOrEmpty(path)) {
                string json = File.ReadAllText(path);
                affinityEditorPage.affinityItemsData = JsonUtility.FromJson<AffinityItemsData>(json);
                EditorUtility.SetDirty(affinityEditorPage);
                Debug.Log("Affinity Editor Data Imported from " + path);
            }
        }

        private void RefreshItemsFromData() {
            AffinityEditorPage affinityEditorPage = (AffinityEditorPage)this.target;
            // Remove all items
            List<GameObject> oldItems = new();
            foreach (Transform item in affinityEditorPage.itemsContainer) {
                if(item.gameObject.GetComponent<AffinityItem>() != null){
                    oldItems.Add(item.gameObject);
                }
            }
            foreach (var item in oldItems) {
                DestroyImmediate(item);
            }

            // Add new items
            affinityEditorPage.CreateAffinityItems();
        }
    }
}