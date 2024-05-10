using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {
    public abstract class SettingsComponent : MonoBehaviour {

        protected UniversalSettingsRunner universalSettings;

        private void Awake() {
            if(universalSettings == null && UniversalSettingsRunner.Instance != null) {
                UniversalSettingsRunner.Instance.RegisterComponent(this);
            }
        }

        internal virtual void Initialize(UniversalSettingsRunner universalSettings) {
            this.universalSettings = universalSettings;
		}

        internal virtual void UpdateComponent(SettingsProfile settings) {
            throw new Exception("UpdateComponent was not implemented!");
        }

        protected virtual void AutoApply() {
            throw new Exception("AutoApply was not implemented!");
		}

        protected virtual void Setup() {
            throw new Exception("Setup was not implemented!");
        }
    }
}
