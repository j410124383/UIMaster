using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Display/Fps Dropdown")]
    public class FpsController : SettingsComponentDropdown {

		[SerializeField] private bool autoApply = false;

		protected override void Setup() {
            CreateOptions(universalSettings.GetDropdownFpsOptions());
        }

		protected override ref int SettingsValue() {
            return ref universalSettings.viewSettings.fpsIndex;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetFps(SettingsValue());
		}

		internal override void UpdateComponent(SettingsProfile settings) {
			SetDropdownActive(settings.vsync == false);

			base.UpdateComponent(settings);
		}


	}
}
