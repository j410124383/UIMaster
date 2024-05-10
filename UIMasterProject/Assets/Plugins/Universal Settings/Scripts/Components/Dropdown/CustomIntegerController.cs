using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Custom/Custom Integer Dropdown")]
    public class CustomIntegerController : SettingsComponentDropdown {

		[SerializeField] private bool autoApply = false;
		[SerializeField] private int customIntegerId;
		[SerializeField] private List<string> options = new List<string>();

		protected override void Setup() {
            CreateOptions(options);
        }

		protected override ref int SettingsValue() {
            return ref universalSettings.viewSettings.customInteger[customIntegerId];
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetCustomInteger(customIntegerId, SettingsValue());
		}

	}
}
