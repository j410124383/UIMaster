using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(CustomFloatController))]
	public class CustomFloatControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;
		private SerializedProperty customFloatIdProp;

		private string[] options;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
			customFloatIdProp = serializedObject.FindProperty("customFloatId");

			options = new string[SettingsProfile.MAX_CUSTOM_FLOAT];
			for(int i = 0; i < options.Length; i++) {
				options[i] = $"Settings Float {i}";
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			customFloatIdProp.intValue = EditorGUILayout.Popup("Identifier", customFloatIdProp.intValue, options);
			EditorGUILayout.PropertyField(autoApplyProp);

			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}