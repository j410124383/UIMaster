using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Button/Apply Settings")]
    public class ApplySettingsButton : SettingsButton {

        protected override void OnClick() {
			universalSettings.ApplySettings();
		}

		internal override void UpdateButton(SettingsProfile settings) {
			button.interactable = universalSettings.IsDirty;
		}

	}
}
