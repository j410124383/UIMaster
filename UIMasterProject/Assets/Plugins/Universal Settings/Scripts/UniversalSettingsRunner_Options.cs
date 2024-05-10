using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {
    public sealed partial class UniversalSettingsRunner : MonoBehaviour {

		// Computed options
		private List<string> fpsTextOptions = null;
		private List<string> resolutionTextOptions = null;
		private List<string>[] refreshRateTextOptions = null;
		private List<string> refreshRateTextFallbackOptions = null;
		private List<string> antiAliasingOptions = null;
		private List<string> shadowModeOptions = null;
		private List<string> shadowDistanceOptions = null;
		private List<string> shadowResolutionOptions = null;
		private List<string> textureResolutionOptions = null;

		private void UpdateFpsOptions() {
			fpsTextOptions = new List<string>();

			foreach(int fps in fpsOptions) {
				string text;

				if(fps == -1)
					text = "Unlimited FPS";
				else
					text = $"{fps} FPS";

				fpsTextOptions.Add(text);
			}
		}

		private void UpdateResolutionOptions() {
			resolutionTextOptions = new List<string>();

			foreach(Resolution resolution in SupportedScreenResolution) {
				resolutionTextOptions.Add($"{resolution.width}x{resolution.height}");
			}
		}

		private void UpdateRefreshRateOptions() {
			refreshRateTextOptions = new List<string>[SupportedRefreshRate.Length];

			for(int i = 0; i < refreshRateTextOptions.Length; i++) {
				refreshRateTextOptions[i] = new List<string>();

				foreach(int refreshRate in SupportedRefreshRate[i]) {
					refreshRateTextOptions[i].Add($"{refreshRate} Hz");
				}
			}

			refreshRateTextFallbackOptions = new List<string>();
			refreshRateTextFallbackOptions.Add($"{Screen.currentResolution.refreshRate} Hz");
		}

		private void UpdateAntiAliasingOptions() {
			antiAliasingOptions = new List<string>();
			antiAliasingOptions.Add("Disabled");
			antiAliasingOptions.Add("2x MSAA");
			antiAliasingOptions.Add("4x MSAA");
			antiAliasingOptions.Add("8x MSAA");
		}

		private void UpdateShadowOptions() {
			shadowModeOptions = new List<string>();
			shadowModeOptions.Add("No shadows");
			shadowModeOptions.Add("Hard shadows");
			shadowModeOptions.Add("Soft shadows");
		}

		private void UpdateShadowDistanceOptions() {
			shadowDistanceOptions = new List<string>();
			shadowDistanceOptions.Add("Low");
			shadowDistanceOptions.Add("Medium");
			shadowDistanceOptions.Add("High");
			shadowDistanceOptions.Add("Ultra");
		}

		private void UpdateShadowResolutionOptions() {
			shadowResolutionOptions = new List<string>();
			shadowResolutionOptions.Add("Low");
			shadowResolutionOptions.Add("Medium");
			shadowResolutionOptions.Add("High");
			shadowResolutionOptions.Add("Ultra");
		}

		private void UpdateTextureResolutionOptions() {
			textureResolutionOptions = new List<string>();
			textureResolutionOptions.Add("Full Resolution");
			textureResolutionOptions.Add("Half Resolution");
			textureResolutionOptions.Add("Quarter Resolution");
			textureResolutionOptions.Add("Eighth Resolution");
		}

		/// <summary>
		/// Updates all dropdown options.
		/// </summary>
		public void UpdateDropdownOptions() {
			UpdateFpsOptions();
			UpdateResolutionOptions();
			UpdateRefreshRateOptions();
			UpdateAntiAliasingOptions();
			UpdateShadowOptions();
			UpdateShadowDistanceOptions();
			UpdateShadowResolutionOptions();
			UpdateTextureResolutionOptions();
		}

		public List<string> GetDropdownFpsOptions() {
			return fpsTextOptions;
		}

		public List<string> GetDropdownResolutionOptions() {
			return resolutionTextOptions;
		}

		public List<string> GetDropdownRefreshRateOptions(int resolutionIndex, FullScreenMode fullScreenMode) {
			if(resolutionIndex >= refreshRateTextOptions.Length || fullScreenMode == FullScreenMode.Windowed) {
				return refreshRateTextFallbackOptions;
			}

			return refreshRateTextOptions[resolutionIndex];
		}

		public List<string> GetDropdownAntiAliasingOptions() {
			return antiAliasingOptions;
		}

		public List<string> GetDropdownShadowModeOptions() {
			return shadowModeOptions;
		}

		public List<string> GetDropdownShadowDistanceOptions() {
			return shadowDistanceOptions;
		}

		public List<string> GetDropdownShadowResolutionOptions() {
			return shadowResolutionOptions;
		}

		public List<string> GetDropdownTextureResolutionOptions() {
			return textureResolutionOptions;
		}
	}
}
