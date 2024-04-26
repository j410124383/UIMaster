using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UniversalSettings {

	[AddComponentMenu("Universal Settings/Display/Resolution Dropdown")]
	public class ResolutionController
		: SettingsComponentDropdown {

		[SerializeField] private bool autoApply = false;

		protected override void Setup() {
			CreateOptions(universalSettings.GetDropdownResolutionOptions());
		}

		protected override ref int SettingsValue() {
			return ref universalSettings.viewSettings.resolutionIndex;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetResolution(SettingsValue());
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			if(SettingsValue() >= universalSettings.GetDropdownResolutionOptions().Count) {
				if(SettingsValue() >= GetOptionsCount()) {
					AddOption("Custom");
				}
			}
			else {
				if(GetOptionsCount() > universalSettings.GetDropdownResolutionOptions().Count) {
					RemoveOption(GetOptionsCount() - 1);
				}
			}

			base.UpdateComponent(settings);
		}
	}
}
