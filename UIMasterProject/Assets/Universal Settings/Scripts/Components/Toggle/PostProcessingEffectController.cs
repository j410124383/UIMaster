using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Effect/Post Processing Effect Toggle")]
    public class PostProcessingEffectController : SettingsComponentToggle {

		[SerializeField] private bool autoApply = false;
        [SerializeField] private PostProcessingEffect postProcessingEffect;

        protected override ref bool SettingsValue() {
            return ref universalSettings.viewSettings.postProcessingEffect[(int)postProcessingEffect];
        }

		protected override bool AutoApplyValue() {
			return autoApply;
		}

		protected override void AutoApply() {
            universalSettings.SetPostProcessingEffect(postProcessingEffect, SettingsValue());
        }

		internal override void UpdateComponent(SettingsProfile settings) {
            SetToggleActive(universalSettings.viewSettings.postProcessing);	

            base.UpdateComponent(settings);
        }

    }
}
