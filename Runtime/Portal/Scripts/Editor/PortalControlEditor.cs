#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace VRSYS.Photoportals {
    [CustomEditor(typeof(PortalControl))]
    public class PortalControlEditor : Editor {
        private Vector2 scrollPosition = Vector2.zero;
        private FieldInfo statusMessagesField;

        private void OnEnable() {
            statusMessagesField = typeof(PortalControl).GetField("statusMessages", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI() {
            // Draw the default inspector
            DrawDefaultInspector();

            // Add some spacing
            EditorGUILayout.Space(10);

            // Draw the status messages section
            EditorGUILayout.LabelField("Status Messages", EditorStyles.boldLabel);
            
            // Create a scrollable area for status messages
            EditorGUILayout.BeginVertical(GUI.skin.box);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));

            List<string> statusMessages = statusMessagesField?.GetValue(target) as List<string>;
            if (statusMessages == null) {
                EditorGUILayout.LabelField("Status message list not available.", EditorStyles.miniLabel);
            } else if (statusMessages.Count == 0) {
                EditorGUILayout.LabelField("No status messages yet.", EditorStyles.miniLabel);
            } else {
                for (int i = statusMessages.Count - 1; i >= 0; i--) {
                    EditorGUILayout.LabelField(statusMessages[i], EditorStyles.wordWrappedLabel);
                }

                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField($"Stored messages: {statusMessages.Count}", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            // Add a button to clear status messages
            if (GUILayout.Button("Clear Status Messages")) {
                if (statusMessages != null) {
                    statusMessages.Clear();
                }
            }

            // Update the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif