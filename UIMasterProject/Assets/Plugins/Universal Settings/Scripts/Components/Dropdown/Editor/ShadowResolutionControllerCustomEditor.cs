using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(ShadowResolutionController))]
	public class ShadowResolutionControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(autoApplyProp);

			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}