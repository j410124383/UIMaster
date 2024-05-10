using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Custom/Custom Boolean Toggle")]
    public class CustomBooleanController : SettingsComponentToggle {

		[SerializeField] private bool autoApply = false;
        [SerializeField] private int customBooleanId;

        protected override ref bool SettingsValue() {
            return ref universalSettings.viewSettings.customBoolean[customBooleanId];
        }

        protected override bool AutoApplyValue() {
            return autoApply;
        }

        protected override void AutoApply() {
            universalSettings.SetCustomBoolean(customBooleanId, SettingsValue());
        }

    }
}
