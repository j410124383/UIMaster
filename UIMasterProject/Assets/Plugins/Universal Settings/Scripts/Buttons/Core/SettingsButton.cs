using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

    [DisallowMultipleComponent]
    public abstract class SettingsButton : MonoBehaviour {

        protected UniversalSettingsRunner universalSettings;
        protected Button button = null;

		private void Awake() {
			if (universalSettings == null && UniversalSettingsRunner.Instance != null) {
                UniversalSettingsRunner.Instance.RegisterButton(this);
            }
		}

		internal void Initialize(UniversalSettingsRunner universalSettings) {
            this.universalSettings = universalSettings;

            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        internal virtual void UpdateButton(SettingsProfile settings) {
            throw new Exception("UpdateButton was not implemented!");
        }

        protected virtual void OnClick() {
            throw new Exception("OnClick was not implemented!");
        }


		private void OnDestroy() {
            if (button) button.onClick.RemoveListener(OnClick);
		}

#if UNITY_EDITOR
        private void Reset() {
            Button _button = GetComponent<Button>();

            if(_button == null) {
                EditorUtility.DisplayDialog("Component not found", "Button component not found!", "Ok");
                DestroyImmediate(this);
            }
        }
#endif


    }
}
