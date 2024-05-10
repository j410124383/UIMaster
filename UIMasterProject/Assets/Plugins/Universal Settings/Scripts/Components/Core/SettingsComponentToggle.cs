using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

	[DisallowMultipleComponent]
	public abstract class SettingsComponentToggle : SettingsComponent {

		private Toggle toggle = null;

		protected virtual ref bool SettingsValue() {
			Exception ex = new Exception("SettingsValue was not implemented!");
			Debug.LogException(ex, gameObject);
			throw ex;
		}

		protected virtual bool AutoApplyValue() {
			Exception ex = new Exception("AutoApplyValue was not implemented");
			Debug.LogException(ex, gameObject);
			throw ex;
		}

		protected virtual void OnValueChanged(bool value) {
			SettingsValue() = value;
			if(AutoApplyValue()) {
				AutoApply();
			}
			else {
				universalSettings.RegisterSettingsChange();
			}
		}

		internal override void Initialize(UniversalSettingsRunner universalSettings) {
			base.Initialize(universalSettings);

			toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(OnValueChanged);
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			toggle.SetIsOnWithoutNotify(SettingsValue());
		}

		protected void SetToggleActive(bool value) {
			toggle.interactable = value;
        }

		private void OnDestroy() {
			if (toggle) toggle.onValueChanged.RemoveListener(OnValueChanged);
		}

#if UNITY_EDITOR
		private void Reset() {
			Toggle _toggle = GetComponent<Toggle>();

			if(_toggle == null) {
				EditorUtility.DisplayDialog("Component not found", "Toggle component not found!", "Ok");
				DestroyImmediate(this);
			}
		}
#endif
	}
}
