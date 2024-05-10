using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UniversalSettings {

	[AddComponentMenu("Universal Settings/Graphic/Anti Aliasing Dropdown")]
	public class AntiAliasingController : SettingsComponentDropdown {

		[SerializeField] private bool autoApply = false;

		protected override void Setup() {
			CreateOptions(universalSettings.GetDropdownAntiAliasingOptions());
		}

		protected override ref int SettingsValue() {
			return ref universalSettings.viewSettings.antiAliasingIndex;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetAntiAliasing((AntiAliasing)SettingsValue());
		}
	}
}
