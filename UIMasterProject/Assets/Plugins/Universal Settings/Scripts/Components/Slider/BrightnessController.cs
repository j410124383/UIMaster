using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalSettings{

    [AddComponentMenu("Universal Settings/Effect/Brightness Slider")]
    public class BrightnessController : SettingsComponentSlider {

		[SerializeField] private bool autoApply = false;

		protected override ref float SettingsValue() {
			return ref universalSettings.viewSettings.brightness;
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetBrightness(SettingsValue());
		}

	}
}
