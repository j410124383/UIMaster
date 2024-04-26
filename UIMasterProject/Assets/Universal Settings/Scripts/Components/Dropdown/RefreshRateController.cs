using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UniversalSettings {

	[AddComponentMenu("Universal Settings/Display/Refresh Rate Dropdown")]
	public class RefreshRateController : SettingsComponentDropdown {

		[SerializeField] private bool autoApply = false;

		protected override void Setup() {
			invertedIndex = true;
		}

		protected override ref int SettingsValue() {
			return ref universalSettings.viewSettings.refreshRateIndex;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetRefreshRate(SettingsValue());
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			SetDropdownActive(settings.vsync);
			CreateOptions(universalSettings.GetDropdownRefreshRateOptions(settings.resolutionIndex, settings.GetFullScreenMode()));

			base.UpdateComponent(settings);
		}
	}
}
