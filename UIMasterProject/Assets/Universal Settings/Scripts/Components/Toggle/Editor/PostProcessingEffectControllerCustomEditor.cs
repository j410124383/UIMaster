using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Rendering;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(PostProcessingEffectController))]
	public class PostProcessingEffectControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;
		private SerializedProperty postProcessingEffectProp;

		private string[] options;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
			postProcessingEffectProp = serializedObject.FindProperty("postProcessingEffect");

			options = new string[Enum.GetNames(typeof(PostProcessingEffect)).Length];
			for(int i = 0;i < options.Length; i++) {
				PostProcessingEffect effect = (PostProcessingEffect)i;

				if (effect == PostProcessingEffect.PaniniProjection) {
					options[i] = $"{effect} (URP only)";
				}
				else if (effect == PostProcessingEffect.AutoExposure || effect == PostProcessingEffect.ScreenSpaceReflections || effect == PostProcessingEffect.AmbientOcclusion) {
					options[i] = $"{effect} (Built-in only)";
				}
				else {
					options[i] = $"{effect}";
				}
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			postProcessingEffectProp.intValue = EditorGUILayout.Popup("Effect", postProcessingEffectProp.intValue, options);
			EditorGUILayout.PropertyField(autoApplyProp);


			PostProcessingEffect effect = (PostProcessingEffect)postProcessingEffectProp.intValue;

			// Built-in pipeline
			if(GraphicsSettings.defaultRenderPipeline == null) {
				EditorGUILayout.Space();
				if(effect == PostProcessingEffect.PaniniProjection) {
					EditorGUILayout.HelpBox($"Not supported in Built-in pipeline!", MessageType.Error);
				}
				else {
					EditorGUILayout.HelpBox($"Make sure that you have added the {(PostProcessingEffect)postProcessingEffectProp.intValue} effect to your post-processing profile, which should be defined in the Universal Settings Runner.", MessageType.Info);
				}
			}
			// URP pipeline
			else {
				EditorGUILayout.Space();
				if(effect == PostProcessingEffect.AutoExposure || effect == PostProcessingEffect.ScreenSpaceReflections || effect == PostProcessingEffect.AmbientOcclusion) {
					EditorGUILayout.HelpBox($"Not supported in URP pipeline!", MessageType.Error);
				}
				else {
					EditorGUILayout.HelpBox($"Make sure that you have added the {(PostProcessingEffect)postProcessingEffectProp.intValue} effect to your VolumeProfile, which should be defined in the Universal Settings Runner.", MessageType.Info);
				} 
			}


			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}