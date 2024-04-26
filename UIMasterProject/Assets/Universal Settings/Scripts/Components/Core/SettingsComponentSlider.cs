using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

	[DisallowMultipleComponent]
	public abstract class SettingsComponentSlider : SettingsComponent {

		private Slider slider = null;

		protected virtual ref float SettingsValue() {
			Exception ex = new Exception("SettingsValue was not implemented!");

			Debug.LogException(ex, gameObject);
			throw ex;
		}

		protected virtual bool AutoApplyValue() {
			Exception ex = new Exception("AutoApplyValue was not implemented");
			Debug.LogException(ex, gameObject);
			throw ex;
		}

		protected virtual void OnValueChanged(float value) {
			SettingsValue() = value / 100f;
			if(AutoApplyValue()) {
				AutoApply();
			}
			else {
				universalSettings.RegisterSettingsChange();
			}
		}

		internal override void Initialize(UniversalSettingsRunner universalSettings) {
			base.Initialize(universalSettings);

			slider = GetComponent<Slider>();
			slider.minValue = 0;
			slider.maxValue = 100;
			slider.wholeNumbers = true;
			slider.onValueChanged.AddListener(OnValueChanged);
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			slider.SetValueWithoutNotify(SettingsValue() * 100f);
		}

		private void OnDestroy() {
			if (slider) slider.onValueChanged.RemoveListener(OnValueChanged);
		}

#if UNITY_EDITOR
		private void Reset() {
			Slider _slider = GetComponent<Slider>();

			if(_slider == null) {
				EditorUtility.DisplayDialog("Component not found", "Slider component not found!", "Ok");
				DestroyImmediate(this);
			}
		}
#endif
	}
}
