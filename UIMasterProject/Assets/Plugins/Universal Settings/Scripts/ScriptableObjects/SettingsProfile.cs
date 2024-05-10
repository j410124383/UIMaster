using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalSettings {

    [CreateAssetMenu(menuName = "Universal Settings/Settings Profile", fileName = "Settings Profile")]
    [System.Serializable]
	public class SettingsProfile : ScriptableObject {

        public const int MAX_RENDERER_FEATURES = 30;
        public const int MAX_AUDIO_MIXERS = 10;
        public const int MAX_CUSTOM_BOOLEAN = 10;
        public const int MAX_CUSTOM_FLOAT = 10;
        public const int MAX_CUSTOM_INTEGER = 10;

        public int fpsIndex = 9999;
        public int resolutionIndex = 9999;
        public int refreshRateIndex = 0;
        public int antiAliasingIndex = 9999;
        public int shadowModeIndex = 9999;
        public int shadowDistanceIndex = 9999;
        public int shadowResolutionIndex = 9999;
        public int textureResolutionIndex = 0;

        public float brightness = 0.5f;

        public bool fullscreen = true;
        public bool vsync = false;

        public bool postProcessing = true;
        public bool[] postProcessingEffect;

        public bool[] rendererFeatures;

        public float masterVolume = 1f;
        public float[] audioMixerVolume;

        public bool[] customBoolean;
        public float[] customFloat;
        public int[] customInteger;

        public SettingsProfile() {
            UpdateStruct();
		}

        public void UpdateStruct() {
            int arraySize = Enum.GetNames(typeof(PostProcessingEffect)).Length;
            if (postProcessingEffect == null || postProcessingEffect.Length != arraySize) {
                postProcessingEffect = new bool[arraySize];
                for(int i = 0;i < arraySize; i++) {
                    postProcessingEffect[i] = true;
				}
			}

            if (rendererFeatures == null || rendererFeatures.Length != MAX_RENDERER_FEATURES) {
                rendererFeatures = new bool[MAX_RENDERER_FEATURES];
                for(int i = 0; i < MAX_RENDERER_FEATURES; i++) {
                    rendererFeatures[i] = true;
                }
            }

            if(audioMixerVolume == null || audioMixerVolume.Length != MAX_AUDIO_MIXERS) {
                audioMixerVolume = new float[MAX_AUDIO_MIXERS];
                for(int i = 0; i < MAX_AUDIO_MIXERS; i++) {
                    audioMixerVolume[i] = 1f;
                }
            }

            if(customBoolean == null || customBoolean.Length != MAX_CUSTOM_BOOLEAN) {
                customBoolean = new bool[MAX_CUSTOM_BOOLEAN];
            }

            if(customFloat == null || customFloat.Length != MAX_CUSTOM_FLOAT) {
                customFloat = new float[MAX_CUSTOM_FLOAT];
            }

            if(customInteger == null || customInteger.Length != MAX_CUSTOM_FLOAT) {
                customInteger = new int[MAX_CUSTOM_INTEGER];
            }
        }

		internal FullScreenMode GetFullScreenMode() {
            if (fullscreen) return FullScreenMode.ExclusiveFullScreen;
            return FullScreenMode.Windowed;
        }

		public bool CompareGraphicQuality(SettingsProfile other) {
            UpdateStruct();
            other.UpdateStruct();

			if (other.antiAliasingIndex != antiAliasingIndex) return false;
            if (other.shadowModeIndex != shadowModeIndex) return false;
            if (other.shadowDistanceIndex != shadowDistanceIndex) return false;
            if (other.shadowResolutionIndex != shadowResolutionIndex) return false;
            if (other.textureResolutionIndex != textureResolutionIndex) return false;

            if (other.postProcessing != postProcessing) return false;
            for(int i = 0;i < postProcessingEffect.Length; i++) {
                if (other.postProcessingEffect[i] != postProcessingEffect[i]) return false;
            }

            for(int i = 0;i < MAX_RENDERER_FEATURES; i++) {
                if (other.rendererFeatures[i] != rendererFeatures[i]) return false;
			}

            return true;
		}
	}
}