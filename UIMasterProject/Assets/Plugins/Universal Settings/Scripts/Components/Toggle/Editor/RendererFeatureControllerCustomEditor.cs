using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniversalSettings.Editor {

	[CustomEditor(typeof(RendererFeatureController))]
	public class RendererFeatureControllerCustomEditor : UnityEditor.Editor {

		private SerializedProperty autoApplyProp;
		private SerializedProperty renderFeatureIdProp;

		private string[] options;

		private void OnEnable() {
			autoApplyProp = serializedObject.FindProperty("autoApply");
			renderFeatureIdProp = serializedObject.FindProperty("renderFeatureId");


#if URP_10_0_0_OR_NEWER
			var rendererFeatureAssets = UniversalSettingsRunner.GetRendererFeatures();
			options = new string[rendererFeatureAssets.Count];
			for(int i = 0; i < options.Length; i++) {
				options[i] = rendererFeatureAssets[i].name;
			}
#else
			options = new string[0];
#endif
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			renderFeatureIdProp.intValue = EditorGUILayout.Popup("Renderer Feature", renderFeatureIdProp.intValue, options);
			EditorGUILayout.PropertyField(autoApplyProp);

			// Built-in pipeline
			if(GraphicsSettings.defaultRenderPipeline == null) {
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox($"Not supported in Built-in pipeline!", MessageType.Error);
			}


			if(EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}