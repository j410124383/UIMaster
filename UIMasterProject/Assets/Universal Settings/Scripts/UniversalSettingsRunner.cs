using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UniversalSettings {

	[AddComponentMenu("Universal Settings/Universal Settings Runner")]
	[DisallowMultipleComponent]
	public sealed partial class UniversalSettingsRunner : MonoBehaviour {

		[Serializable]
		public class AudioMixerConfig {
			public AudioMixer audioMixer;
			public string volumeVariable;
		}

		[SerializeField] private SettingsProfile defaultSettings;
		[SerializeField] private List<SettingsProfile> qualitySettings = new List<SettingsProfile>();
		[SerializeField] private List<int> fpsOptions = new List<int>() { 30, 60, 120, 240, -1 };
		[SerializeField] private List<AudioMixerConfig> audioMixerConfigs = new List<AudioMixerConfig>();
#if POSTPROCESSING_3_0_0_OR_NEWER
		[SerializeField] private List<UnityEngine.Rendering.PostProcessing.PostProcessProfile> postProcessProfiles = new List<UnityEngine.Rendering.PostProcessing.PostProcessProfile>();
#endif
#if URP_10_0_0_OR_NEWER
		[SerializeField] private List<VolumeProfile> volumeProfiles = new List<VolumeProfile>();
#endif

		public bool enableBrightness = true;
		public bool enableFps = true;
		public bool enableFullscreen = true;
		public bool enableResolution = true;
		public bool enableRefreshRate = true;
		public bool enableVsync = true;
		public bool enableAntiAliasing = true;
		public bool enableShadow = true;
		public bool enableShadowDistance = true;
		public bool enableShadowResolution = true;
		public bool enableTextureResolution = true;
		public bool enablePostProcessing = true;
		public bool enableRenderFeature = true;
		public bool enableMasterVolume = true;
		public bool enableAudioMixerVolume = true;

		public static UniversalSettingsRunner Instance { private set; get; } = null;
		private static RenderPipelineAsset DefaultRenderPipeline = null;

		public bool Initialized { private set; get; } = false;
		public Resolution[] SupportedScreenResolution { private set; get; } = null;
		public List<int>[] SupportedRefreshRate { private set; get; } = null;
		internal bool IsDirty { private set; get; } = false;

		private int supportedRefreshRateFallback = 60;

		private List<SettingsComponent> settingsComponents = new List<SettingsComponent>();
		private List<SettingsButton> settingsButtons = new List<SettingsButton>();

		private SettingsProfile appliedSettings = null;
		internal SettingsProfile viewSettings = null;

#if URP_10_0_0_OR_NEWER
		private List<UnityEngine.Rendering.Universal.ScriptableRendererFeature> rendererFeatureAssets = new List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>();
#endif

		/// <summary>
		/// Is called when any setting is applied.
		/// </summary>
		public event UnityAction onApplySettings = null;

		private void Awake() {
			if (Instance != null) {
				Destroy(this);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);

			Initialize();
		}

		private void Initialize() {
			Initialized = false;

			CheckRequisites();

			CloneDefaultRenderPipeline();

			CreateTemporarySettingsProfiles();

			ComputeResolutions();
			UpdateDropdownOptions();

			UpdateRendererFeatureAssetsList();

			SetSettings(LoadPlayerSettings());

			RegisterSceneButtons();
			RegisterSceneComponents();

			Initialized = true;
		}

		private void Start() {
			for(int i = 0; i < SettingsProfile.MAX_AUDIO_MIXERS; i++) {
				SetAudioMixerVolume_Internal(i, appliedSettings.audioMixerVolume[i]);
			}
		}

		private void OnEnable() {
			StartCoroutine(UpdateSettings());
		}

		private IEnumerator UpdateSettings() {
			yield return new WaitUntil(() => { return Initialized; });

			while(true) {

				while(updatingDisplay) {
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();

					updatingDisplay = false;
				}

				if(FixExternalChanges()) {
					SavePlayerSettings(appliedSettings);
					UpdateUI();
				}

				yield return new WaitForSeconds(1f);
			}
		}

		private bool FixExternalChanges() {
			bool change = false;

			ClearDisplayBuffer();

			change |= FixResolution();
			change |= FixFullscreen();

			if(change) {
				ApplyDisplayBuffer();
			}

			return change;
		}

		private bool FixFullscreen() {
			if (enableFullscreen == false) return false;

			if (Screen.fullScreenMode != appliedSettings.GetFullScreenMode()) {

				SetFullscreen_Internal(Screen.fullScreen);

				return true;
			}
			return false;
		}

		private bool FixResolution() {
			if (enableResolution == false) return false;

			int resolutionIndex = SupportedScreenResolution.Length;
			for(int i = 0;i < SupportedScreenResolution.Length; i++) { 
				if(IsCurrentResolution(SupportedScreenResolution[i])) {
					resolutionIndex = i;
					break;
				}
			}

			if (resolutionIndex != appliedSettings.resolutionIndex) {

				SetResolution_Internal(resolutionIndex);

				return true;
			}
			return false;
		}

		private void CheckRequisites() {
			if (defaultSettings == null) {
				defaultSettings = ScriptableObject.CreateInstance<SettingsProfile>();

				Debug.LogError("Default settings cannot be empty because it's used to set default values for all properties.", gameObject);
			}
		}

		private void UpdateRendererFeatureAssetsList() {
#if URP_10_0_0_OR_NEWER
			rendererFeatureAssets = GetRendererFeatures();
#endif
		}

		private int GetRefreshRateFromIndex(int refreshRateIndex) {
			int refreshRate = supportedRefreshRateFallback;

			if(appliedSettings.resolutionIndex < SupportedRefreshRate.Length) {
				if(refreshRateIndex < SupportedRefreshRate[appliedSettings.resolutionIndex].Count) {
					int processedIndex = SupportedRefreshRate[appliedSettings.resolutionIndex].Count - 1 - refreshRateIndex;
					refreshRate = SupportedRefreshRate[appliedSettings.resolutionIndex][processedIndex];
				}
			}

			return refreshRate;
		}

		private bool IsCurrentResolution(Resolution resolution) {
			return resolution.width == Screen.width && resolution.height == Screen.height;
		}

		private void CloneDefaultRenderPipeline() {
			if (GraphicsSettings.defaultRenderPipeline != null) {
				DefaultRenderPipeline = GraphicsSettings.defaultRenderPipeline;
				GraphicsSettings.defaultRenderPipeline = Instantiate(GraphicsSettings.defaultRenderPipeline);
			}
		}

		private void CreateTemporarySettingsProfiles() {
			defaultSettings.UpdateStruct();

			appliedSettings = Instantiate(defaultSettings);
			viewSettings = Instantiate(defaultSettings);
		}

		private void RegisterSceneButtons() {
			SettingsButton[] buttons = FindObjectsOfType<SettingsButton>();
			foreach(SettingsButton button in buttons) {
				RegisterButton(button);
			}
		}

		internal void RegisterButton(SettingsButton button) {
			settingsButtons.Add(button);

			button.Initialize(this);
			button.UpdateButton(viewSettings);
		}

		private void UpdateButtons() {
			foreach(SettingsButton button in settingsButtons) {
				if (button == null) continue;

				button.UpdateButton(viewSettings);
			}
		}

		private void RegisterSceneComponents() {
			SettingsComponent[] components = FindObjectsOfType<SettingsComponent>();
			foreach(SettingsComponent component in components) {
				RegisterComponent(component);
			}
		}

		internal void RegisterComponent(SettingsComponent component) {
			settingsComponents.Add(component);

			component.Initialize(this);
			component.UpdateComponent(viewSettings);
		}

		private void UpdateComponents() {
			foreach(SettingsComponent component in settingsComponents) {
				if (component == null) continue;

				component.UpdateComponent(viewSettings);
			}
		}

		internal void RegisterSettingsChange() {
			if (Initialized == false) return;

			IsDirty = true;
			
			UpdateUI();	
		}

		private void UpdateUI() {
			UpdateComponents();
			UpdateButtons();
		}

		private void ComputeResolutions() {
			Dictionary<string, Resolution> resolutionsDict = new Dictionary<string, Resolution>();
			Dictionary<string, List<int>> refreshRateDict = new Dictionary<string, List<int>>();

			foreach(Resolution resolution in Screen.resolutions) {
				string key = $"{resolution.width}x{resolution.height}";

				if(!resolutionsDict.ContainsKey(key)) {
					resolutionsDict.Add(key, resolution);
				}

				if(!refreshRateDict.ContainsKey(key)) {
					refreshRateDict.Add(key, new List<int>());
				}

				refreshRateDict[key].Add(resolution.refreshRate);
			}

			SupportedScreenResolution = new Resolution[resolutionsDict.Count];
			SupportedRefreshRate = new List<int>[resolutionsDict.Count];

			int index = 0;
			foreach(var element in resolutionsDict) {
				SupportedScreenResolution[index] = element.Value;
				SupportedRefreshRate[index] = refreshRateDict[element.Key];

				index++;
			}

			supportedRefreshRateFallback = Screen.currentResolution.refreshRate;
		}

		/// <summary>
		/// Return the current settings that have not been applied.
		/// </summary>
		public SettingsProfile GetNotAppliedSettings() {
			return viewSettings;
		}

		/// <summary>
		/// Returns the applied settings.
		/// </summary>
		public SettingsProfile GetAppliedSettings() {
			return appliedSettings;
		}

		/// <summary>
		/// Returns a list of preset settings.
		/// </summary>
		public List<SettingsProfile> GetQualitySettings() {
			return qualitySettings;
		}

#if URP_10_0_0_OR_NEWER
		static public List<UnityEngine.Rendering.Universal.ScriptableRendererFeature> GetRendererFeatures() {
			var urp = GraphicsSettings.defaultRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
			if(urp == null) return new List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>();

			var propertyInfo = urp.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
			var scriptableRenderData = (UnityEngine.Rendering.Universal.ScriptableRendererData[])propertyInfo.GetValue(urp);

			List<UnityEngine.Rendering.Universal.ScriptableRendererFeature> result = new List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>();
			if(scriptableRenderData != null && scriptableRenderData.Length > 0) {
				foreach(var renderData in scriptableRenderData) {
					foreach(var rendererFeature in renderData.rendererFeatures) {
						result.Add(rendererFeature);
					}
				}
			}
			return result;
		}
#endif

#if UNITY_EDITOR
		private void OnValidate() {
			ComputeResolutions();
			UpdateDropdownOptions();
		}
#endif
	}
}
