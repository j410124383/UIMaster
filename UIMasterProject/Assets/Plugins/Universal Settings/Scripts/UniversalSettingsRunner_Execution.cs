using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniversalSettings {
    public sealed partial class UniversalSettingsRunner : MonoBehaviour {

		private class DisplayBuffer {
			public int width;
			public int height;
			public int refreshRate;
			public FullScreenMode fullScreenMode;
		}

		private bool updatingDisplay = false;
		private DisplayBuffer displayBuffer = new DisplayBuffer();

#if URP_10_0_0_OR_NEWER
		private FieldInfo mainShadowResolution = typeof(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset).GetField("m_MainLightShadowmapResolution", BindingFlags.NonPublic | BindingFlags.Instance);
		private FieldInfo additionalShadowResolution = typeof(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset).GetField("m_AdditionalLightsShadowmapResolution", BindingFlags.NonPublic | BindingFlags.Instance);
		private FieldInfo softShadowField = typeof(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset).GetField("m_SoftShadowsSupported", BindingFlags.NonPublic | BindingFlags.Instance);
#endif

		internal void ClearDisplayBuffer() {
			displayBuffer.width = Screen.width;
			displayBuffer.height = Screen.height;
			displayBuffer.refreshRate = Screen.currentResolution.refreshRate;
			displayBuffer.fullScreenMode = Screen.fullScreenMode;
		}

		internal void ApplyDisplayBuffer() {
			Screen.SetResolution(displayBuffer.width, displayBuffer.height, displayBuffer.fullScreenMode, displayBuffer.refreshRate);
			updatingDisplay = true;
		}

		private void CallApplySettingsCallback() {
			onApplySettings?.Invoke();
		}

		/// <summary>
		/// Sets the settings from the settingsProfile.
		/// </summary>
		/// <param name="settingsProfile">Settings to be applied.</param>
		public void SetSettings(SettingsProfile settingsProfile) {
			ClearDisplayBuffer();


			SetBrightness_Internal(settingsProfile.brightness);
			SetFps_Internal(settingsProfile.fpsIndex);
			SetFullscreen_Internal(settingsProfile.fullscreen);
			SetResolution_Internal(settingsProfile.resolutionIndex);
			SetVsync_Internal(settingsProfile.vsync);
			SetRefreshRate_Internal(settingsProfile.refreshRateIndex);
			SetAntiAliasing_Internal(settingsProfile.antiAliasingIndex);
			SetShadowMode_Internal(settingsProfile.shadowModeIndex);
			SetShadowDistance_Internal(settingsProfile.shadowDistanceIndex);
			SetShadowResolution_Internal(settingsProfile.shadowResolutionIndex);
			SetTextureResolution_Internal(settingsProfile.textureResolutionIndex);
			SetPostProcessing_Internal(settingsProfile.postProcessing);
			for(int i = 0;i < Enum.GetNames(typeof(PostProcessingEffect)).Length; i++) {
				SetPostProcessingEffect_Internal((PostProcessingEffect)i, settingsProfile.postProcessingEffect[i]);
			}
#if URP_10_0_0_OR_NEWER
			for(int i = 0;i < SettingsProfile.MAX_RENDERER_FEATURES; i++) {
				SetRendererFeature_Internal(i, settingsProfile.rendererFeatures[i]);
			}
#endif
			SetMasterVolume_Internal(settingsProfile.masterVolume);
			for(int i = 0; i < SettingsProfile.MAX_AUDIO_MIXERS; i++) {
				SetAudioMixerVolume_Internal(i, settingsProfile.audioMixerVolume[i]);
			}
			for(int i = 0; i < SettingsProfile.MAX_CUSTOM_FLOAT; i++) {
				SetCustomFloat_Internal(i, settingsProfile.customFloat[i]);
			}
			for(int i = 0; i < SettingsProfile.MAX_CUSTOM_BOOLEAN; i++) {
				SetCustomBoolean_Internal(i, settingsProfile.customBoolean[i]);
			}
			for(int i = 0; i < SettingsProfile.MAX_CUSTOM_INTEGER; i++) {
				SetCustomInteger_Internal(i, settingsProfile.customInteger[i]);
			}


			ApplyDisplayBuffer();

			IsDirty = false;
			UpdateUI();

			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Applies all the not yet applied settings.
		/// </summary>
		public void ApplySettings() {
            SetSettings(viewSettings);
		}

		/// <summary>
		/// Clears all the not yet applied settings.
		/// </summary>
		public void UndoSettings() {
			viewSettings = Instantiate(appliedSettings);
			IsDirty = false;
			UpdateUI();
		}

		/// <summary>
		/// Resets settings to default, which is defined in the UniversalSettingsRunner game object.
		/// </summary>
		public void ResetSettings() {
			SetSettings(defaultSettings);
		}

		#region Refresh Rate
		internal void SetRefreshRate_Internal(int refreshRateIndex) {
			if (enableRefreshRate == false) return;

			viewSettings.refreshRateIndex = refreshRateIndex;
			appliedSettings.refreshRateIndex = refreshRateIndex;

			displayBuffer.refreshRate = GetRefreshRateFromIndex(refreshRateIndex);
		}

		/// <summary>
		/// Sets refresh rate.
		/// </summary>
		/// <param name="refreshRateIndex">Index from UniversalSettingsRunner.Instance.SupportedRefreshRate list.</param>
		public void SetRefreshRate(int refreshRateIndex) {
			if(enableRefreshRate == false) return;

			ClearDisplayBuffer();
			SetRefreshRate_Internal(refreshRateIndex);
			ApplyDisplayBuffer();

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied index from UniversalSettingsRunner.Instance.SupportedRefreshRate list.
		/// </summary>
		public int GetRefreshRate() {
			return appliedSettings.refreshRateIndex;
		}
		#endregion

		#region Vsync
		internal void SetVsync_Internal(bool value) {
			if (enableVsync == false) return;

			viewSettings.vsync = value;
			appliedSettings.vsync = value;

			if(value) {
				Application.targetFrameRate = -1;
			}
			QualitySettings.vSyncCount = value ? 1 : 0;
		}

		/// <summary>
		/// Sets vsync (on/off).
		/// </summary>
		public void SetVsync(bool value) {
			if (enableVsync == false) return;

			SetVsync_Internal(value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied vsync (on/off).
		/// </summary>
		public bool GetVsync() {
			return appliedSettings.vsync;
		}
		#endregion

		#region Resolution
		internal void SetResolution_Internal(int value) {
			if (enableResolution == false) return;

			viewSettings.resolutionIndex = value;
			appliedSettings.resolutionIndex = value;

			if (value >= SupportedScreenResolution.Length) return;

			displayBuffer.width = SupportedScreenResolution[value].width;
			displayBuffer.height = SupportedScreenResolution[value].height;
		}

		/// <summary>
		/// Sets resolution.
		/// </summary>
		/// <param name="value">Index from UniversalSettingsRunner.Instance.SupportedScreenResolution list.</param>
		public void SetResolution(int value) {
			if (enableResolution == false) return;

			ClearDisplayBuffer();
			SetResolution_Internal(value);
			ApplyDisplayBuffer();

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied index from UniversalSettingsRunner.Instance.SupportedScreenResolution list.
		/// </summary>
		public int GetResolution() {
			return appliedSettings.resolutionIndex;
		}
		#endregion

		#region Fullscreen
		internal void SetFullscreen_Internal(bool value) {
			if (enableFullscreen == false) return;

			viewSettings.fullscreen = value;
			appliedSettings.fullscreen = value;

			displayBuffer.fullScreenMode = appliedSettings.GetFullScreenMode();
		}

		/// <summary>
		/// Sets fullscreen.
		/// </summary>
		public void SetFullscreen(bool value) {
			if (enableFullscreen == false) return;

			ClearDisplayBuffer();
			SetFullscreen_Internal(value);
			ApplyDisplayBuffer();

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied fullscreen.
		/// </summary>
		public bool GetFullscreen() {
			return appliedSettings.fullscreen;
		}
		#endregion

		#region Fps
		internal void SetFps_Internal(int value) {
			if (enableFps == false) return;

			viewSettings.fpsIndex = value;
			appliedSettings.fpsIndex = value;

			int normalizedValue = Math.Min(value, fpsOptions.Count - 1);
			int targetFrameRate = -1;
			if (normalizedValue >= 0) {
				targetFrameRate = fpsOptions[normalizedValue];
			}
			Application.targetFrameRate = targetFrameRate;
		}

		/// <summary>
		/// Sets frames per second by changing the Application.targetFrameRate.
		/// </summary>
		/// <param name="value">Index from UniversalSettingsRunner.Instance.GetFpsOptions() list.</param>
		public void SetFps(int value) {
			if (enableFps == false) return;

			SetFps_Internal(value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied index from UniversalSettingsRunner.Instance.GetFpsOptions() list.
		/// </summary>
		public int GetFps() {
			return appliedSettings.fpsIndex;
		}
		#endregion

		#region Brightness
		internal void SetBrightness_Internal(float value) {
			if (enableBrightness == false) return;

			viewSettings.brightness = value;
			appliedSettings.brightness = value;

			float normalizedValue = Mathf.Lerp(-2f, 2f, value);

			// Built-in pipeline
#if POSTPROCESSING_3_0_0_OR_NEWER
			if(GraphicsSettings.defaultRenderPipeline == null) {
				foreach(UnityEngine.Rendering.PostProcessing.PostProcessProfile profile in postProcessProfiles) {
					if (profile == null) continue;

					UnityEngine.Rendering.PostProcessing.ColorGrading colorGrading;
					if(profile.TryGetSettings<UnityEngine.Rendering.PostProcessing.ColorGrading>(out colorGrading)) {
						colorGrading.postExposure.Override(normalizedValue);
					}
				}
			}
#endif

			// URP pipeline
#if URP_10_0_0_OR_NEWER
			if(GraphicsSettings.defaultRenderPipeline != null) {
				foreach(VolumeProfile profile in volumeProfiles) {
					if (profile == null) continue;

					UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
					if(profile.TryGet<UnityEngine.Rendering.Universal.ColorAdjustments>(out colorAdjustments)) {
						colorAdjustments.postExposure.Override(normalizedValue);
					}
				}
			}
#endif

		}

		/// <summary>
		/// Sets brightness using post processing.
		/// </summary>
		/// <param name="value">Value in [0, 1] range. 0.0 = dark, 0.5 = default, 1.0 = light.</param>
		public void SetBrightness(float value) {
			if (enableBrightness == false) return;

			SetBrightness_Internal(value);

			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied brightness.
		/// </summary>
		public float GetBrightness() {
			return appliedSettings.brightness;
		}
		#endregion

		#region Anti Aliasing
		internal void SetAntiAliasing_Internal(int value) {
			if (enableAntiAliasing == false) return;

			if(value > 3) value = 3;

			viewSettings.antiAliasingIndex = value;
			appliedSettings.antiAliasingIndex = value;


			int msaa = 1;
			for(int i = 0;i < value; i++) {
				msaa *= 2;
			}

			// Built-in pipeline
			QualitySettings.antiAliasing = msaa;

			// URP pipeline
#if URP_10_0_0_OR_NEWER
			UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp = GraphicsSettings.defaultRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
			if(urp != null) {
				urp.msaaSampleCount = msaa;
			}
#endif
		}

		/// <summary>
		/// Sets multisample anti-aliasing (MSAA).
		/// </summary>
		public void SetAntiAliasing(AntiAliasing value) {
			if(enableAntiAliasing == false) return;

			SetAntiAliasing_Internal((int)value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied multisample anti-aliasing (MSAA).
		/// </summary>
		public AntiAliasing GetAntiAliasing() {
			return (AntiAliasing)appliedSettings.antiAliasingIndex;
		}
		#endregion

		#region Shadow Mode
		internal void SetShadowMode_Internal(int value) {
			if (enableShadow == false) return;

			if(value > 2) value = 2;

			viewSettings.shadowModeIndex = value;
			appliedSettings.shadowModeIndex = value;


			// Built-in pipeline
			switch(value) {
				case 0: QualitySettings.shadows = ShadowQuality.Disable; break;
				case 1: QualitySettings.shadows = ShadowQuality.HardOnly; break;
				case 2: QualitySettings.shadows = ShadowQuality.All; break;
			}
			

			// URP pipeline
#if URP_10_0_0_OR_NEWER
			UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp = GraphicsSettings.defaultRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
			if(urp != null) {
				UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset defaultUrp = DefaultRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;

				if (value == 0) {
					urp.shadowDistance = 0;
				}
				else {
					urp.shadowDistance = defaultUrp.shadowDistance;

					if (value == 1) {
						softShadowField.SetValue(urp, false);
					}
					else {
						softShadowField.SetValue(urp, true);
					}
				}

			}
#endif

		}

		/// <summary>
		/// Sets shadow mode.
		/// </summary>
		public void SetShadowMode(ShadowMode value) {
			if (enableShadow == false) return;

			SetShadowMode_Internal((int)value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied shadow mode.
		/// </summary>
		public ShadowMode GetShadowMode() {
			return (ShadowMode)appliedSettings.shadowModeIndex;
		}

		#endregion

		#region Shadow Distance
		internal void SetShadowDistance_Internal(int value) {
			if (enableShadowDistance == false) return;

			if(value > 3) value = 3;

			viewSettings.shadowDistanceIndex = value;
			appliedSettings.shadowDistanceIndex = value;

			if(appliedSettings.shadowModeIndex == 0) return;

			// Built-in pipeline
			switch(value) {
				case 0: 
					QualitySettings.shadowDistance = 10f;
					QualitySettings.shadowCascades = 1;
					break;
				case 1: 
					QualitySettings.shadowDistance = 30f;
					QualitySettings.shadowCascades = 2;
					break;
				case 2: 
					QualitySettings.shadowDistance = 75f;
					QualitySettings.shadowCascades = 4;
					break;
				case 3: 
					QualitySettings.shadowDistance = 150f;
					QualitySettings.shadowCascades = 4;
					break;
			}

			// URP pipeline
#if URP_10_0_0_OR_NEWER
			UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp = GraphicsSettings.defaultRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
			if(urp != null) {
				switch(value) {
					case 0: 
						urp.shadowDistance = 10f; 
						urp.shadowCascadeCount = 1;
						break;
					case 1: 
						urp.shadowDistance = 30f;
						urp.shadowCascadeCount = 2;
						break;
					case 2: 
						urp.shadowDistance = 75f; 
						urp.shadowCascadeCount = 3;
						break;
					case 3: 
						urp.shadowDistance = 150f; 
						urp.shadowCascadeCount = 4;
						break;
				}
			}
#endif
		}

		/// <summary>
		/// Sets shadow distance.
		/// </summary>
		public void SetShadowDistance(ShadowDistance value) {
			if (enableShadowDistance == false) return;

			SetShadowDistance_Internal((int)value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied shadow distance.
		/// </summary>
		public ShadowDistance GetShadowDistance() {
			return (ShadowDistance)appliedSettings.shadowDistanceIndex;
		}
		#endregion

		#region Shadow Resolution
		internal void SetShadowResolution_Internal(int value) {
			if (enableShadowResolution == false) return;

			if(value > 3) value = 3;

			viewSettings.shadowResolutionIndex = value;
			appliedSettings.shadowResolutionIndex = value;

			if(appliedSettings.shadowModeIndex == 0) return;


			// Built-in pipeline
			switch(value) {
				case 0: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low; break;
				case 1: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Medium; break;
				case 2: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.High; break;
				case 3: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.VeryHigh; break;
			}

			// URP pipeline
#if URP_10_0_0_OR_NEWER
			UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp = GraphicsSettings.defaultRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
			if(urp != null) {
				switch(value) {
					case 0:
						mainShadowResolution.SetValue(urp, 512);
						additionalShadowResolution.SetValue(urp, 512);
						break;
					case 1:
						mainShadowResolution.SetValue(urp, 1024);
						additionalShadowResolution.SetValue(urp, 1024);
						break;
					case 2:
						mainShadowResolution.SetValue(urp, 2048);
						additionalShadowResolution.SetValue(urp, 2048);
						break;
					case 3:
						mainShadowResolution.SetValue(urp, 4096);
						additionalShadowResolution.SetValue(urp, 4096);
						break;
				}
			}
#endif
		}

		/// <summary>
		/// Sets shadow resolution.
		/// </summary>
		public void SetShadowResolution(ShadowResolution value) {
			if (enableShadowResolution == false) return;

			SetShadowResolution_Internal((int)value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied shadow resolution.
		/// </summary>
		public ShadowResolution GetShadowResolution() {
			return (ShadowResolution)appliedSettings.shadowResolutionIndex;
		}
		#endregion

		#region Texture Resolution
		internal void SetTextureResolution_Internal(int value) {
			if (enableTextureResolution == false) return;

			if (value > 3) value = 3;

			viewSettings.textureResolutionIndex = value;
			appliedSettings.textureResolutionIndex = value;

			
			QualitySettings.masterTextureLimit = value;
		}

		/// <summary>
		/// Sets texture resolution.
		/// </summary>
		public void SetTextureResolution(TextureResolution value) {
			if (enableTextureResolution == false) return;

			SetTextureResolution_Internal((int)value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied texture resolution.
		/// </summary>
		public TextureResolution GetTextureResolution() {
			return (TextureResolution)appliedSettings.textureResolutionIndex;
		}

		#endregion

		#region Post Processing
		internal void SetPostProcessing_Internal(bool value) {
			if (enablePostProcessing == false) return;

			viewSettings.postProcessing = value;
			appliedSettings.postProcessing = value;
		}

		/// <summary>
		/// Sets post processing. Enable/disable all post processing effects.
		/// </summary>
		public void SetPostProcessing(bool value) {
			if(enablePostProcessing == false) return;

			SetPostProcessing_Internal(value);
			for(int i = 0; i < Enum.GetNames(typeof(PostProcessingEffect)).Length; i++) {
				SetPostProcessingEffect_Internal((PostProcessingEffect)i, viewSettings.postProcessingEffect[i]);
			}

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied post processing (enable/disable).
		/// </summary>
		public bool GetPostProcessing() {
			return appliedSettings.postProcessing;
		}
		#endregion

		#region Post Processing Effect
		internal void SetPostProcessingEffect_Internal(PostProcessingEffect postProcessingEffect, bool value) {
			if (enablePostProcessing == false) return;

			int effectId = (int)postProcessingEffect;

			viewSettings.postProcessingEffect[effectId] = value;
			appliedSettings.postProcessingEffect[effectId] = value;

			value &= appliedSettings.postProcessing;


			// Built-in pipeline
#if POSTPROCESSING_3_0_0_OR_NEWER
			if(GraphicsSettings.defaultRenderPipeline == null) {
				switch(postProcessingEffect) {
					case PostProcessingEffect.Bloom:					SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.Bloom>(value);break;
					case PostProcessingEffect.ChromaticAberration:		SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.ChromaticAberration>(value);break;
					case PostProcessingEffect.DepthOfField:				SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.DepthOfField>(value);break;
					case PostProcessingEffect.FilmGrain:				SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.Grain>(value);break;
					case PostProcessingEffect.LensDistortion:			SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.LensDistortion>(value);break;
					case PostProcessingEffect.MotionBlur:				SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.MotionBlur>(value);break;
					case PostProcessingEffect.PaniniProjection:			/* Not Supported */;break;
					case PostProcessingEffect.Vignette:					SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.Vignette>(value);break;
					case PostProcessingEffect.AutoExposure:				SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.AutoExposure>(value);break;
					case PostProcessingEffect.ScreenSpaceReflections:	SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.ScreenSpaceReflections>(value);break;
					case PostProcessingEffect.AmbientOcclusion:			SetPostProcessingEffectActive_Builtin<UnityEngine.Rendering.PostProcessing.AmbientOcclusion>(value);break;
				}
			}
#endif

			// URP pipeline
#if URP_10_0_0_OR_NEWER
			if(GraphicsSettings.defaultRenderPipeline != null) {
				switch(postProcessingEffect) {
					case PostProcessingEffect.Bloom:					SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.Bloom>(value);break;
					case PostProcessingEffect.ChromaticAberration:		SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.ChromaticAberration>(value);break;
					case PostProcessingEffect.DepthOfField:				SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.DepthOfField>(value);break;
					case PostProcessingEffect.FilmGrain:				SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.FilmGrain>(value);break;
					case PostProcessingEffect.LensDistortion:			SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.LensDistortion>(value);break;
					case PostProcessingEffect.MotionBlur:				SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.MotionBlur>(value);break;
					case PostProcessingEffect.PaniniProjection:			SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.PaniniProjection>(value);break;
					case PostProcessingEffect.Vignette:					SetPostProcessingEffectActive_URP<UnityEngine.Rendering.Universal.Vignette>(value);break;
					case PostProcessingEffect.AutoExposure:				/* Not Supported */;break;
					case PostProcessingEffect.ScreenSpaceReflections:	/* Not Supported */;break;
					case PostProcessingEffect.AmbientOcclusion:			/* Not Supported */;break;
				}
			}
#endif
		}

		/// <summary>
		/// Sets post processing effect. The effect must be in the post process profile defined in the UniversalSettingsRunner game object.
		/// </summary>
		public void SetPostProcessingEffect(PostProcessingEffect postProcessingEffect, bool value) {
			if (enablePostProcessing == false) return;

			SetPostProcessingEffect_Internal(postProcessingEffect, value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied post processing effect (on/off).
		/// </summary>
		public bool GetPostProcessingEffect(PostProcessingEffect postProcessingEffect) {
			return appliedSettings.postProcessingEffect[(int)postProcessingEffect];
		}


#if POSTPROCESSING_3_0_0_OR_NEWER
		private void SetPostProcessingEffectActive_Builtin<T>(bool value) where T : UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings {
			if(GraphicsSettings.defaultRenderPipeline == null) {
				foreach(UnityEngine.Rendering.PostProcessing.PostProcessProfile profile in postProcessProfiles) {
					if (profile == null) continue;

					T setting;
					if(profile.TryGetSettings<T>(out setting)) {
						setting.active = value;
					}
				}
			}
		}
#endif


#if URP_10_0_0_OR_NEWER
		private void SetPostProcessingEffectActive_URP<T>(bool value) where T : VolumeComponent {
			if(GraphicsSettings.defaultRenderPipeline != null) {
				foreach(VolumeProfile profile in volumeProfiles) {
					if (profile == null) continue;

					T setting;
					if(profile.TryGet<T>(out setting)) {
						setting.active = value;
					}
				}
			}
		}
#endif
		#endregion

		#region Renderer Feature
		internal void SetRendererFeature_Internal(int id, bool value) {
#if URP_10_0_0_OR_NEWER
			if (enableRenderFeature == false) return;
			if (id >= rendererFeatureAssets.Count) return;

			viewSettings.rendererFeatures[id] = value;
			appliedSettings.rendererFeatures[id] = value;

			rendererFeatureAssets[id].SetActive(value);
#endif
		}

		/// <summary>
		/// Sets renderer feature (URP only).
		/// </summary>
		/// <param name="index">0 is the first renderer feature, 2 is the second, and so on.</param>
		public void SetRendererFeature(int index, bool value) {
#if URP_10_0_0_OR_NEWER
			if(enableRenderFeature == false) return;
			if (index >= rendererFeatureAssets.Count) return;

			SetRendererFeature_Internal(index, value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
#endif
		}

		/// <summary>
		/// Returns the applied renderer feature (on/off).
		/// </summary>
		/// <param name="index">0 is the first renderer feature, 2 is the second, and so on.</param>
		public bool GetRendererFeature(int index) {
			return appliedSettings.rendererFeatures[index];
		}
		#endregion

		#region MasterVolume
		internal void SetMasterVolume_Internal(float value) {
			if (enableMasterVolume == false) return;

			viewSettings.masterVolume = value;
			appliedSettings.masterVolume = value;

			AudioListener.volume = value;
		}

		/// <summary>
		/// Sets master volume by changing the AudioListener.volume.
		/// </summary>
		/// <param name="value">Value in [0, 1] range.</param>
		public void SetMasterVolume(float value) {
			if (enableMasterVolume == false) return;

			SetMasterVolume_Internal(value);

			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied master volume.
		/// </summary>
		public float GetMasterVolume() {
			return appliedSettings.masterVolume;
		}
		#endregion

		#region Audio Mixer
		internal void SetAudioMixerVolume_Internal(int id, float value) {
			if (enableAudioMixerVolume == false) return;
			if (id >= audioMixerConfigs.Count) return;


			viewSettings.audioMixerVolume[id] = value;
			appliedSettings.audioMixerVolume[id] = value;

			if (audioMixerConfigs[id].audioMixer == null) return;

			float dB = Mathf.Log10(0.0001f + value) * 20f;
			audioMixerConfigs[id].audioMixer.SetFloat(audioMixerConfigs[id].volumeVariable, dB);
		}

		/// <summary>
		/// Sets audio mixer volume.
		/// </summary>
		/// <param name="id">Index from audio mixer list in the UniversalSettingsRunner game object.</param>
		/// <param name="value">Value in [0, 1] range.</param>
		public void SetAudioMixerVolume(int id, float value) {
			if(enableAudioMixerVolume == false) return;
			if(id >= audioMixerConfigs.Count) return;

			SetAudioMixerVolume_Internal(id, value);

			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied audio mixer volume.
		/// </summary>
		/// <param name="id">Index from audio mixer list in the UniversalSettingsRunner game object.</param>
		public float GetAudioMixerVolume(int id) {
			return appliedSettings.audioMixerVolume[id];
		}
		#endregion

		#region Custom Float
		internal void SetCustomFloat_Internal(int id, float value) {
			if (id >= SettingsProfile.MAX_CUSTOM_FLOAT) return;

			viewSettings.customFloat[id] = value;
			appliedSettings.customFloat[id] = value;
		}

		/// <summary>
		/// Sets custom float.
		/// </summary>
		/// <param name="id">Custom float index.</param>
		/// <param name="value">Value in [0, 1] range.</param>
		public void SetCustomFloat(int id, float value) {
			if (id >= SettingsProfile.MAX_CUSTOM_FLOAT) return;

			SetCustomFloat_Internal(id, value);

			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied custom float.
		/// </summary>
		/// <param name="id">Custom float index.</param>
		public float GetCustomFloat(int id) {
			return appliedSettings.customFloat[id];
		}
		#endregion

		#region Custom Boolean
		internal void SetCustomBoolean_Internal(int id, bool value) {
			if (id >= SettingsProfile.MAX_CUSTOM_BOOLEAN) return;

			viewSettings.customBoolean[id] = value;
			appliedSettings.customBoolean[id] = value;
		}

		/// <summary>
		/// Sets custom boolean.
		/// </summary>
		/// <param name="id">Custom boolean index.</param>
		public void SetCustomBoolean(int id, bool value) {
			if (id >= SettingsProfile.MAX_CUSTOM_BOOLEAN) return;

			SetCustomBoolean_Internal(id, value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied custom boolean.
		/// </summary>
		/// <param name="id">Custom boolean index.</param>
		public bool GetCustomBoolean(int id) {
			return appliedSettings.customBoolean[id];
		}
		#endregion

		#region Custom Integer
		internal void SetCustomInteger_Internal(int id, int value) {
			if (id >= SettingsProfile.MAX_CUSTOM_INTEGER) return;

			viewSettings.customInteger[id] = value;
			appliedSettings.customInteger[id] = value;
		}

		/// <summary>
		/// Sets custom integer.
		/// </summary>
		/// <param name="id">Custom integer index.</param>
		public void SetCustomInteger(int id, int value) {
			if (id >= SettingsProfile.MAX_CUSTOM_INTEGER) return;

			SetCustomInteger_Internal(id, value);

			UpdateUI();
			SavePlayerSettings(appliedSettings);
			CallApplySettingsCallback();
		}

		/// <summary>
		/// Returns the applied custom integer.
		/// </summary>
		/// <param name="id">Custom integer index.</param>
		public int GetCustomInteger(int id) {
			return appliedSettings.customInteger[id];
		}
		#endregion
		
	}
}
