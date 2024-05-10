using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {
    public sealed partial class UniversalSettingsRunner : MonoBehaviour {

		private static readonly string DefaultSaveKey = "UniversalSettings:Player";

		private SettingsProfile LoadPlayerSettings() {
			string settingsJson = PlayerPrefs.GetString(DefaultSaveKey, JsonUtility.ToJson(defaultSettings));

			SettingsProfile playerSettings = ScriptableObject.CreateInstance<SettingsProfile>();
			JsonUtility.FromJsonOverwrite(settingsJson, playerSettings);
			playerSettings.UpdateStruct();

			return playerSettings;
		}

		private void SavePlayerSettings(SettingsProfile settingsPreset) {
			string settingsJson = JsonUtility.ToJson(settingsPreset);

			PlayerPrefs.SetString(DefaultSaveKey, settingsJson);
			PlayerPrefs.Save();
		}

	}
}
