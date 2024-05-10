using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(CustomIntegerController))]
	public class CustomIntegerControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;
		private SerializedProperty customIntegerIdProp;
		private SerializedProperty optionsProp;

		private string[] options;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
			customIntegerIdProp = serializedObject.FindProperty("customIntegerId");
			optionsProp = serializedObject.FindProperty("options");


			options = new string[SettingsProfile.MAX_CUSTOM_INTEGER];
			for(int i = 0;i < options.Length; i++) {
				options[i] = $"Settings Integer {i}";
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			customIntegerIdProp.intValue = EditorGUILayout.Popup("Identifier", customIntegerIdProp.intValue, options);
			EditorGUILayout.PropertyField(autoApplyProp);
			EditorGUILayout.PropertyField(optionsProp);


			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}