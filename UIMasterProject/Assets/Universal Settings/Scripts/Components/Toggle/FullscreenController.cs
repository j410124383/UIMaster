using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Display/Fullscreen Toggle")]
    public class FullscreenController : SettingsComponentToggle {

		[SerializeField] private bool autoApply = false;

		protected override ref bool SettingsValue() {
			return ref universalSettings.viewSettings.fullscreen;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetFullscreen(SettingsValue());
		}

	}
}
