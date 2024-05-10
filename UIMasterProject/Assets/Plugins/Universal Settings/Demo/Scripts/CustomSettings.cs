using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniversalSettings.Demo {
    public class CustomSettings : MonoBehaviour {

		[SerializeField] private PulseScale pulseScale;
		[SerializeField] private MeshRenderer cubeRenderer;
		[SerializeField] private Material[] cubeColors;

		private void Start() {
			UniversalSettingsRunner.Instance.onApplySettings += UpdateCustomSettings;

			UpdateCustomSettings();
		}

		private void OnDestroy() {
			UniversalSettingsRunner.Instance.onApplySettings -= UpdateCustomSettings;
		}

		private void UpdateCustomSettings() {
			bool fog = UniversalSettingsRunner.Instance.GetCustomBoolean(0);
			float sphereSpeed = UniversalSettingsRunner.Instance.GetCustomFloat(0);
			int cubeColor = UniversalSettingsRunner.Instance.GetCustomInteger(0);

			RenderSettings.fog = fog;
			pulseScale.speed = sphereSpeed * 10;
			cubeRenderer.sharedMaterial = cubeColors[cubeColor];
		}
	}
}
