using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Display/Vsync Toggle")]
    public class VsyncController : SettingsComponentToggle {

		[SerializeField] private bool autoApply = false;

        protected override ref bool SettingsValue() {
            return ref universalSettings.viewSettings.vsync;
        }

        protected override bool AutoApplyValue() {
            return autoApply;
        }

        protected override void AutoApply() {
            universalSettings.SetVsync(SettingsValue());
        }

    }
}
