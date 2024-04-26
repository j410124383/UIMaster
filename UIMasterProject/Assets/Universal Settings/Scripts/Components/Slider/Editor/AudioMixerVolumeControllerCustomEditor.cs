using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(AudioMixerVolumeController))]
	public class AudioMixerVolumeControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;
		private SerializedProperty audioMixerConfigIdProp;

		private string[] options;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
			audioMixerConfigIdProp = serializedObject.FindProperty("audioMixerConfigId");

			options = new string[SettingsProfile.MAX_AUDIO_MIXERS];
			for(int i = 0;i < options.Length; i++) {
				options[i] = $"Element {i}";
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			audioMixerConfigIdProp.intValue = EditorGUILayout.Popup("Audio Mixer", audioMixerConfigIdProp.intValue, options);
			EditorGUILayout.PropertyField(autoApplyProp);

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("The selected Audio Mixer must be defined in the Universal Settings Runner.", MessageType.Info);

			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}