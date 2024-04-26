using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings{

    [AddComponentMenu("Universal Settings/Custom/Custom Float Slider")]
    public class CustomFloatController : SettingsComponentSlider {

		[SerializeField] private bool autoApply = false;
		[SerializeField] private int customFloatId;

		protected override ref float SettingsValue() {
			return ref universalSettings.viewSettings.customFloat[customFloatId];
		}

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
			universalSettings.SetCustomFloat(customFloatId, SettingsValue());
		}

	}
}
