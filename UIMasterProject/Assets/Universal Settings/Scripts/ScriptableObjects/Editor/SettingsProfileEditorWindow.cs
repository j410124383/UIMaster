using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace UniversalSettings.Editor {

    public class SettingsProfileEditorWindow : EditorWindow {

        private GUIStyle sectionStyle = null;
        private Color sectionColor = new Color(0.1f, 0.1f, .1f);

        private SettingsProfile settingsProfile = null;
        private SerializedObject serializedObject = null;
        private UniversalSettingsRunner universalSettings = null;

        private SerializedProperty fpsIndexProp;
        private SerializedProperty fullscreenProp;
        private SerializedProperty vsyncProp;

        private SerializedProperty antiAliasingIndexProp;
        private SerializedProperty shadowDistanceIndexProp; 
        private SerializedProperty shadowIndexProp;
        private SerializedProperty shadowResolutionIndexProp;
        private SerializedProperty textureResolutionIndexProp;

        private SerializedProperty masterVolumeProp;
        private SerializedProperty audioMixerVolumeProp;

        private SerializedProperty brightnessProp;
        private SerializedProperty postProcessingProp;
        private SerializedProperty postProcessingEffectProp;
        private SerializedProperty rendererFeaturesProp;

        private SerializedProperty customBooleanProp;
        private SerializedProperty customFloatProp;
        private SerializedProperty customIntegerProp;

        private string[] fpsOptions;
        private string[] aaOptions;
        private string[] shadowDistanceOptions;
        private string[] shadowOptions;
        private string[] shadowResolutionOptions;
        private string[] textureResolutionOptions;

        private string[] rendererFeaturesNames;

        private Vector2 scrollView = Vector2.zero;
        
        private static bool displayFold = true;
        private static bool graphicFold = true;
        private static bool soundFold = true;
        private static bool effectFold = true;
        private static bool customBooleanFold = false;
        private static bool customFloatFold = false;
        private static bool customIntegerFold = false;


        public static void Open(SettingsProfile settingsProfile) {
            UniversalSettingsRunner universalSettingsRunner = GameObject.FindObjectOfType<UniversalSettingsRunner>();

            if(universalSettingsRunner == null) {
                if (EditorUtility.DisplayDialog("UniversalSettingsRunner not found", "The UniversalSettingsRunner was not found in the current scene. Would you like to add it?", "Create Universal Settings Runner", "Cancel")) {
                    CreateGameObjectUniversalSettings.CreateGameObject();
                }
                return;
            }

            settingsProfile.UpdateStruct();

			SettingsProfileEditorWindow window = GetWindow<SettingsProfileEditorWindow>("Settings Profile Editor");

            window.minSize = new Vector2(400, 500);
			window.serializedObject = new SerializedObject(settingsProfile);
            window.settingsProfile = settingsProfile;
            window.universalSettings = universalSettingsRunner;

            window.universalSettings.UpdateDropdownOptions();
            window.ComputeOptions();
            window.ComputeProperties();

        }

        private void ComputeOptions() {
            fpsOptions = universalSettings.GetDropdownFpsOptions().ToArray();

            aaOptions = universalSettings.GetDropdownAntiAliasingOptions().ToArray();
            shadowDistanceOptions = universalSettings.GetDropdownShadowDistanceOptions().ToArray();
            shadowOptions = universalSettings.GetDropdownShadowModeOptions().ToArray();
            shadowResolutionOptions = universalSettings.GetDropdownShadowResolutionOptions().ToArray();
            textureResolutionOptions = universalSettings.GetDropdownTextureResolutionOptions().ToArray();

#if URP_10_0_0_OR_NEWER
			var rendererFeatures = UniversalSettingsRunner.GetRendererFeatures();
            rendererFeaturesNames = new string[rendererFeatures.Count];
            for(int i = 0;i < rendererFeaturesNames.Length; i++) {
                rendererFeaturesNames[i] = rendererFeatures[i].name;
			}
#else
			rendererFeaturesNames = new string[0];
#endif
		}

		private void ComputeProperties() {
            fpsIndexProp = serializedObject.FindProperty("fpsIndex");
            fullscreenProp = serializedObject.FindProperty("fullscreen");
            vsyncProp = serializedObject.FindProperty("vsync");

            antiAliasingIndexProp = serializedObject.FindProperty("antiAliasingIndex");
            shadowDistanceIndexProp = serializedObject.FindProperty("shadowDistanceIndex");
            shadowIndexProp = serializedObject.FindProperty("shadowModeIndex");
            shadowResolutionIndexProp = serializedObject.FindProperty("shadowResolutionIndex");
            textureResolutionIndexProp = serializedObject.FindProperty("textureResolutionIndex");

            masterVolumeProp = serializedObject.FindProperty("masterVolume");
            audioMixerVolumeProp = serializedObject.FindProperty("audioMixerVolume");

            brightnessProp = serializedObject.FindProperty("brightness");
            postProcessingProp = serializedObject.FindProperty("postProcessing");
            postProcessingEffectProp = serializedObject.FindProperty("postProcessingEffect");
            rendererFeaturesProp = serializedObject.FindProperty("rendererFeatures");

            customBooleanProp = serializedObject.FindProperty("customBoolean");
            customFloatProp = serializedObject.FindProperty("customFloat");
            customIntegerProp = serializedObject.FindProperty("customInteger");

        }


		private void OnGUI() {
            if(sectionStyle == null) {
                sectionStyle = new GUIStyle(GUI.skin.box);
                sectionStyle.alignment = TextAnchor.MiddleLeft;
                sectionStyle.fontStyle = FontStyle.Bold;
                sectionStyle.normal.textColor = Color.white;
                sectionStyle.richText = true;
            }


            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            DrawHeader();
            DrawContent();

            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
			}
        }

        private void DrawContent() {
            scrollView = EditorGUILayout.BeginScrollView(scrollView);

            DrawDisplaySettings();
            DrawGraphicSettings();
            DrawSoundSettings();
            DrawEffectSettings();
            DrawCustomFloatSettings();
            DrawCustomBooleanSettings();
            DrawCustomIntegerSettings();

            EditorGUILayout.EndScrollView();
        }

		private void DrawHeader() {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, 30f), sectionColor);
            EditorGUI.LabelField(new Rect(10, 0, position.width, 30f), settingsProfile.name, sectionStyle);

            GUILayout.Space(30f);
        }

		private void DrawDisplaySettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            displayFold = EditorGUILayout.Foldout(displayFold, "Display", true);

            if(displayFold) {
                EditorGUI.indentLevel++;


                fpsIndexProp.intValue = EditorGUILayout.Popup("Frames per second", Math.Min(fpsIndexProp.intValue, fpsOptions.Length-1), fpsOptions);
                EditorGUILayout.PropertyField(fullscreenProp);
                EditorGUILayout.PropertyField(vsyncProp);



                EditorGUI.indentLevel--;
			}

            EditorGUILayout.EndVertical();
        }

        private void DrawGraphicSettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            graphicFold = EditorGUILayout.Foldout(graphicFold, "Graphic", true);

            if(graphicFold) {
                EditorGUI.indentLevel++;


                antiAliasingIndexProp.intValue = EditorGUILayout.Popup("Anti Aliasing", Math.Min(antiAliasingIndexProp.intValue, aaOptions.Length - 1), aaOptions);
                shadowIndexProp.intValue = EditorGUILayout.Popup("Shadow", Math.Min(shadowIndexProp.intValue, shadowOptions.Length - 1), shadowOptions);
                shadowDistanceIndexProp.intValue = EditorGUILayout.Popup("Shadow Distance", Math.Min(shadowDistanceIndexProp.intValue, shadowDistanceOptions.Length - 1), shadowDistanceOptions);
                shadowResolutionIndexProp.intValue = EditorGUILayout.Popup("Shadow Resolution", Math.Min(shadowResolutionIndexProp.intValue, shadowResolutionOptions.Length - 1), shadowResolutionOptions);
                textureResolutionIndexProp.intValue = EditorGUILayout.Popup("Texture Resolution", Math.Min(textureResolutionIndexProp.intValue, textureResolutionOptions.Length - 1), textureResolutionOptions);



                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawSoundSettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            soundFold = EditorGUILayout.Foldout(soundFold, "Sound", true);

            if(soundFold) {
                EditorGUI.indentLevel++;

                EditorGUILayout.Slider(masterVolumeProp, 0, 1f, "Master Volume");
                for(int i = 0;i < SettingsProfile.MAX_AUDIO_MIXERS; i++) {
                    EditorGUILayout.Slider(audioMixerVolumeProp.GetArrayElementAtIndex(i), 0, 1f, $"Audio Mixer Volume #{i}");
				}

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawEffectSettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            effectFold = EditorGUILayout.Foldout(effectFold, "Effect", true);

            if(effectFold) {
                EditorGUI.indentLevel++;


                for(int i = 0;i < rendererFeaturesNames.Length; i++) {
                    rendererFeaturesProp.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.Toggle(rendererFeaturesNames[i], rendererFeaturesProp.GetArrayElementAtIndex(i).boolValue);
                }
                EditorGUILayout.Slider(brightnessProp, 0, 1f, "Brightness");
                EditorGUILayout.PropertyField(postProcessingProp);
                for(int i = 0; i < Enum.GetNames(typeof(PostProcessingEffect)).Length; i++) {
                    EditorGUILayout.PropertyField(postProcessingEffectProp.GetArrayElementAtIndex(i), new GUIContent($"{(PostProcessingEffect)i}"));
				}


                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawCustomBooleanSettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            customBooleanFold = EditorGUILayout.Foldout(customBooleanFold, "Custom Boolean", true);

            if(customBooleanFold) {
                EditorGUI.indentLevel++;


                for(int i = 0;i < SettingsProfile.MAX_CUSTOM_BOOLEAN; i++) {
                    EditorGUILayout.PropertyField(customBooleanProp.GetArrayElementAtIndex(i), new GUIContent($"Custom Boolean #{i}"));
                }


                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawCustomFloatSettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            customFloatFold = EditorGUILayout.Foldout(customFloatFold, "Custom Float", true);

            if(customFloatFold) {
                EditorGUI.indentLevel++;


                for(int i = 0; i < SettingsProfile.MAX_CUSTOM_FLOAT; i++) {
                    EditorGUILayout.Slider(customFloatProp.GetArrayElementAtIndex(i), 0, 1f, $"Custom Float #{i}");
                }


                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawCustomIntegerSettings() {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            customIntegerFold = EditorGUILayout.Foldout(customIntegerFold, "Custom Integer", true);

            if(customIntegerFold) {
                EditorGUI.indentLevel++;


                for(int i = 0; i < SettingsProfile.MAX_CUSTOM_INTEGER; i++) {
                    EditorGUILayout.PropertyField(customIntegerProp.GetArrayElementAtIndex(i), new GUIContent($"Custom Integer #{i}"));
                }


                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

    }
}
