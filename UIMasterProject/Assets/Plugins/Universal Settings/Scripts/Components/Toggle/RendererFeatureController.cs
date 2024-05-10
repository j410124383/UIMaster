using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Effect/Renderer Feature Toggle (URP only)")]
    public class RendererFeatureController : SettingsComponentToggle {

		[SerializeField] private bool autoApply = false;
        [SerializeField] private int renderFeatureId;

        protected override ref bool SettingsValue() {
            return ref universalSettings.viewSettings.rendererFeatures[renderFeatureId];
        }

        protected override bool AutoApplyValue() {
            return autoApply;
        }

        protected override void AutoApply() {
            universalSettings.SetRendererFeature(renderFeatureId, SettingsValue());
        }

    }
}
