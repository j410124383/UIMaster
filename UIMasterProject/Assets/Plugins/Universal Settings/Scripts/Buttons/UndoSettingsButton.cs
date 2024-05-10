using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [AddComponentMenu("Universal Settings/Button/Undo Settings")]
    public class UndoSettingsButton : SettingsButton {

        protected override void OnClick() {
			universalSettings.UndoSettings();
		}

		internal override void UpdateButton(SettingsProfile settings) {
			button.interactable = universalSettings.IsDirty;
		}

	}
}
