using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(CustomBooleanController))]
	public class CustomBooleanControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;
		private SerializedProperty customBooleanIdProp;

		private string[] options;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
			customBooleanIdProp = serializedObject.FindProperty("customBooleanId");

			options = new string[SettingsProfile.MAX_CUSTOM_BOOLEAN];
			for(int i = 0; i < options.Length; i++) {
				options[i] = $"Settings Boolean {i}";
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			customBooleanIdProp.intValue = EditorGUILayout.Popup("Identifier", customBooleanIdProp.intValue, options);
			EditorGUILayout.PropertyField(autoApplyProp);

			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}