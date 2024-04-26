using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(BrightnessController))]
	public class BrightnessControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(autoApplyProp);

#if POSTPROCESSING_3_0_0_OR_NEWER
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Make sure that you have added the ColorGrading effect to your post-processing profile, which should be defined in the Universal Settings Runner.", MessageType.Info);
#endif
#if URP_10_0_0_OR_NEWER
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Make sure that you have added the ColorAdjustments effect to your VolumeProfile, which should be defined in the Universal Settings Runner.", MessageType.Info);
#endif

			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}