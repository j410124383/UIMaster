using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Graphic/Shadow Resolution Dropdown")]
    public class ShadowResolutionController : SettingsComponentDropdown {

		[SerializeField] private bool autoApply = false;

		protected override void Setup() {
            CreateOptions(universalSettings.GetDropdownShadowResolutionOptions());
        }

		protected override ref int SettingsValue() {
            return ref universalSettings.viewSettings.shadowResolutionIndex;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetShadowResolution((ShadowResolution)SettingsValue());
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			SetDropdownActive(settings.shadowModeIndex > 0);

			base.UpdateComponent(settings);
		}

	}
}
