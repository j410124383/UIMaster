using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UniversalSettings {

	[AddComponentMenu("Universal Settings/Quality Settings Dropdown")]
	public class QualitySettingsController : SettingsComponentDropdown {

		private int index = 0;
		
		protected override void Setup() {
			List<string> options = new List<string>();
			foreach(SettingsProfile settingsProfile in universalSettings.GetQualitySettings()) {
				options.Add(settingsProfile.name);
			}

			CreateOptions(options);
		}

		protected override ref int SettingsValue() {
			return ref index;
		}
		
		protected override bool AutoApplyValue() {
			return false;
		}

		protected override void OnValueChanged(int value) {
			if (value < universalSettings.GetQualitySettings().Count) {
				SettingsProfile selectedQuality = universalSettings.GetQualitySettings()[value];

				universalSettings.viewSettings = Instantiate(universalSettings.viewSettings);

				universalSettings.viewSettings.antiAliasingIndex = selectedQuality.antiAliasingIndex;
				universalSettings.viewSettings.shadowModeIndex = selectedQuality.shadowModeIndex;
				universalSettings.viewSettings.shadowDistanceIndex = selectedQuality.shadowDistanceIndex;
				universalSettings.viewSettings.shadowResolutionIndex = selectedQuality.shadowResolutionIndex;
				universalSettings.viewSettings.textureResolutionIndex = selectedQuality.textureResolutionIndex;

				universalSettings.viewSettings.postProcessing = selectedQuality.postProcessing;
				universalSettings.viewSettings.postProcessingEffect = selectedQuality.postProcessingEffect;
				universalSettings.viewSettings.rendererFeatures = selectedQuality.rendererFeatures;
			}

			base.OnValueChanged(value);
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			index = 0;
			foreach(SettingsProfile settingsProfile in universalSettings.GetQualitySettings()) {
				if(settingsProfile.CompareGraphicQuality(settings)) {
					break;
				}
				index++;
			}

			if(SettingsValue() >= universalSettings.GetQualitySettings().Count) {
				if(SettingsValue() >= GetOptionsCount()) {
					AddOption("Custom");
				}
			}
			else {
				if(GetOptionsCount() > universalSettings.GetQualitySettings().Count) {
					RemoveOption(GetOptionsCount() - 1);
				}
			}

			base.UpdateComponent(settings);
		}
		
	}
}
