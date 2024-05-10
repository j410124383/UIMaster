using UniversalSettings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {
    [CustomEditor(typeof(UniversalSettingsRunner))]
    public class UniversalSettingsRunnerCustomEditor : UnityEditor.Editor {

        private const float headerHeight = 30f;
        private const float chevronSize = 8f;
        private Color sectionColor = new Color(0.1f, 0.1f, .1f);

        private static bool settingsProfileFoldout = true;
        private SerializedProperty defaultSettingsProp;
        private SerializedProperty qualitySettingsProp;

        private static bool fpsFoldout = true;
        private SerializedProperty fpsOptionsProp;

        private static bool audioMixerFoldout = true;
        private SerializedProperty audioMixerConfigsProp;

        private static bool postProcessingFoldout = true;
        private SerializedProperty postProcessProfilesProp;
        private SerializedProperty volumeProfilesProp;

        private static bool enablePropertiesFoldout = true;
        private SerializedProperty enableBrightnessProp;
        private SerializedProperty enableFpsProp;
        private SerializedProperty enableFullscreenProp;
        private SerializedProperty enableResolutionProp;
        private SerializedProperty enableRefreshRateProp;
        private SerializedProperty enableVsyncProp;
        private SerializedProperty enableAntiAliasingProp;
        private SerializedProperty enableShadowProp;
        private SerializedProperty enableShadowDistanceProp;
        private SerializedProperty enableShadowResolutionProp;
        private SerializedProperty enableTextureResolutionProp;
        private SerializedProperty enablePostProcessingProp;
        private SerializedProperty enableRenderFeatureProp;
        private SerializedProperty enableMasterVolumeProp;
        private SerializedProperty enableAudioMixerVolumeProp;

        private Texture2D chevronTexture;
        private GUIStyle sectionStyle = null;   


        private void OnEnable() {
            fpsOptionsProp = serializedObject.FindProperty("fpsOptions");

            defaultSettingsProp = serializedObject.FindProperty("defaultSettings");
            qualitySettingsProp = serializedObject.FindProperty("qualitySettings");

            audioMixerConfigsProp = serializedObject.FindProperty("audioMixerConfigs");

            postProcessProfilesProp = serializedObject.FindProperty("postProcessProfiles");
            volumeProfilesProp = serializedObject.FindProperty("volumeProfiles");

            enableBrightnessProp = serializedObject.FindProperty("enableBrightness");
            enableFpsProp = serializedObject.FindProperty("enableFps");
            enableFullscreenProp = serializedObject.FindProperty("enableFullscreen");
            enableResolutionProp = serializedObject.FindProperty("enableResolution");
            enableRefreshRateProp = serializedObject.FindProperty("enableRefreshRate");
            enableVsyncProp = serializedObject.FindProperty("enableVsync");
            enableAntiAliasingProp = serializedObject.FindProperty("enableAntiAliasing");
            enableShadowProp = serializedObject.FindProperty("enableShadow");
            enableShadowDistanceProp = serializedObject.FindProperty("enableShadowDistance");
            enableShadowResolutionProp = serializedObject.FindProperty("enableShadowResolution");
            enableTextureResolutionProp = serializedObject.FindProperty("enableTextureResolution");
            enablePostProcessingProp = serializedObject.FindProperty("enablePostProcessing");
            enableRenderFeatureProp = serializedObject.FindProperty("enableRenderFeature");
            enableMasterVolumeProp = serializedObject.FindProperty("enableMasterVolume");
            enableAudioMixerVolumeProp = serializedObject.FindProperty("enableAudioMixerVolume");

            chevronTexture = Resources.Load<Texture2D>("chevron");
        }

        public override void OnInspectorGUI() {
            if (sectionStyle == null) {
                sectionStyle = new GUIStyle(GUI.skin.box);
                sectionStyle.alignment = TextAnchor.MiddleCenter;
                sectionStyle.fontStyle = FontStyle.Bold;
                sectionStyle.normal.textColor = Color.white;
                sectionStyle.richText = true;
			}



            serializedObject.Update();

            EditorGUI.BeginChangeCheck();



            DrawSettingsSection();
            DrawFpsSection();
            DrawAudioMixerSection();
            DrawPostProcessingSection();
            DrawEnablePropertiesSection();



            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawSectionHeader(string title, ref bool foldout) {
            Rect rect = EditorGUILayout.GetControlRect();
            rect.width = EditorGUIUtility.currentViewWidth;
            rect.height = headerHeight;
            rect.x = 0;

			foldout = EditorGUI.Foldout(rect, foldout, "", true);
            EditorGUI.DrawRect(rect, sectionColor);
            EditorGUI.LabelField(rect, title, sectionStyle);

            float pos = headerHeight / 2f - chevronSize / 2f;
            if(foldout) EditorGUI.DrawTextureTransparent(new Rect(pos, rect.y+pos+chevronSize, chevronSize, -chevronSize), chevronTexture);
			else        EditorGUI.DrawTextureTransparent(new Rect(pos, rect.y+pos, chevronSize, chevronSize), chevronTexture);

            GUILayout.Space(headerHeight-20f+1f);
        }

        private void DrawSettingsSection() {
            DrawSectionHeader("Settings Profiles", ref settingsProfileFoldout);

            if(settingsProfileFoldout) {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(defaultSettingsProp);
                if (defaultSettingsProp.objectReferenceValue == null) {
                    EditorGUILayout.HelpBox("Default settings cannot be empty because it's used to set default values for all properties. If you don't have a settings profile yet, click on the button below to create one.", MessageType.Error);
			    }

                EditorGUILayout.PropertyField(qualitySettingsProp, true);

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("The Quality Settings list is used by the Quality Settings component to select the settings profile during gameplay.", MessageType.None);
                EditorGUILayout.Space();

                if (GUILayout.Button("Create new Settings Profile asset")) {
                    if (defaultSettingsProp.objectReferenceValue == null) {
					    defaultSettingsProp.objectReferenceValue = CreateSettingsProfileAsset();   
                    }
                    else {
						CreateSettingsProfileAsset();
					}
                }

                EditorGUILayout.Space();
            }
        }

        private void DrawFpsSection() {
            DrawSectionHeader("Frames Per Second", ref fpsFoldout);

            if(fpsFoldout) {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(fpsOptionsProp);

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("The Fps Options list is used by the Fps component.", MessageType.None);
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Use -1 if you want an unlimited frames per seconds option", MessageType.Info);

                EditorGUILayout.Space();
			}
        }

        private void DrawAudioMixerSection() {
            DrawSectionHeader("Audio Mixer", ref audioMixerFoldout);

            if(audioMixerFoldout) {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(audioMixerConfigsProp, true);

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("The Audio Mixer Configs list is used by the Audio Mixer Volume component. To make the component work, you need to expose the volume variable in the Audio Mixer.", MessageType.None);

                EditorGUILayout.Space();
			}
        }

        private void DrawPostProcessingSection() {
            DrawSectionHeader("Post Processing", ref postProcessingFoldout);

            if(postProcessingFoldout) {
                EditorGUILayout.Space();

                // Post-processing profiles
#if POSTPROCESSING_3_0_0_OR_NEWER
                EditorGUILayout.PropertyField(postProcessProfilesProp, true);
#endif

                // Volume profiles
#if URP_10_0_0_OR_NEWER
                EditorGUILayout.PropertyField(volumeProfilesProp, true);
#endif

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("The Post Processing Effect component will override the profiles listed in order to enable or disable certain effects.", MessageType.None);

                EditorGUILayout.Space();
            }
        }

        private void DrawEnablePropertiesSection() {
            DrawSectionHeader("Enabled Properties", ref enablePropertiesFoldout);

            if(enablePropertiesFoldout) {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(enableBrightnessProp, new GUIContent("Brightness"), GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(enableFpsProp, new GUIContent("Fps"));
                EditorGUILayout.PropertyField(enableFullscreenProp, new GUIContent("Fullscreen"));
                EditorGUILayout.PropertyField(enableResolutionProp, new GUIContent("Resolution"));
                EditorGUILayout.PropertyField(enableRefreshRateProp, new GUIContent("RefreshRate"));
                EditorGUILayout.PropertyField(enableVsyncProp, new GUIContent("Vsync"));
                EditorGUILayout.PropertyField(enableAntiAliasingProp, new GUIContent("AntiAliasing"));
                EditorGUILayout.PropertyField(enableShadowProp, new GUIContent("Shadow"));
                EditorGUILayout.PropertyField(enableShadowDistanceProp, new GUIContent("ShadowDistance"));
                EditorGUILayout.PropertyField(enableShadowResolutionProp, new GUIContent("ShadowResolution"));
                EditorGUILayout.PropertyField(enableTextureResolutionProp, new GUIContent("TextureResolution"));
                EditorGUILayout.PropertyField(enablePostProcessingProp, new GUIContent("PostProcessing"));
                EditorGUILayout.PropertyField(enableRenderFeatureProp, new GUIContent("RenderFeature (URP)"));
                EditorGUILayout.PropertyField(enableMasterVolumeProp, new GUIContent("MasterVolume"));
                EditorGUILayout.PropertyField(enableAudioMixerVolumeProp, new GUIContent("AudioMixerVolume"));

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Only enable the properties that you plan to use, or you may encounter unexpected behavior.", MessageType.Info);
                EditorGUILayout.Space();
            }
        }

        private SettingsProfile CreateSettingsProfileAsset() {
            string baseName = "SettingsProfile";
            int index = 0;
            string assetName = baseName + ".asset";

            while(AssetDatabase.LoadAssetAtPath("Assets/" + assetName, typeof(SettingsProfile))) {
                index++;
                assetName = baseName + " " + index + ".asset";
            }

            SettingsProfile newSettingsProfile = ScriptableObject.CreateInstance<SettingsProfile>();
            AssetDatabase.CreateAsset(newSettingsProfile, "Assets/" + assetName);
            AssetDatabase.SaveAssets();

            Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/" + assetName, typeof(SettingsProfile));

            return newSettingsProfile;
		}

    }
}
