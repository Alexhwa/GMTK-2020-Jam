using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelRunner))]
public class LevelRunnerEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        LevelRunner runner = target as LevelRunner;

        if (GUILayout.Button("Set")) {
            List<Dispenser> dispensers = new List<Dispenser>();

            foreach (LevelElement elem in runner.GetComponentsInChildren<LevelElement>()) {
                // set the levelRunner field of elem using this crazy hack
                var serializedElem = new SerializedObject(elem);
                var runnerProperty = serializedElem.FindProperty("levelRunner");
                runnerProperty.objectReferenceValue = runner;
                serializedElem.ApplyModifiedProperties();
            
                if (elem is Dispenser disp) {
                    dispensers.Add(disp);
                }
            }

            // set the dispensers field of runner using this crazy hack
            var serializedRun = new SerializedObject(runner);
            var dispProperty = serializedRun.FindProperty("dispensers");
            dispProperty.arraySize = dispensers.Count;
            for (int d = 0; d < dispensers.Count; d++) {
                dispProperty.GetArrayElementAtIndex(d).objectReferenceValue = dispensers[d];
            }
            serializedRun.ApplyModifiedProperties();
        }
    }
}
