using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace UniversalSettings.Editor {

	public class AssetHandler {

		[OnOpenAsset()]
		public static bool OpenEditor(int instaceId, int line) {

			SettingsProfile obj = EditorUtility.InstanceIDToObject(instaceId) as SettingsProfile;
			if (obj != null) {
				SettingsProfileEditorWindow.Open(obj);
				return true;
			}

			return false;
		}
	}

    [CustomEditor(typeof(SettingsProfile))]
    public class SettingsProfileCustomEditor : UnityEditor.Editor {

		public override void OnInspectorGUI() {
			
			if (GUILayout.Button("Open Editor")) {
				SettingsProfileEditorWindow.Open((SettingsProfile)target);
			}

		}
	}
}
