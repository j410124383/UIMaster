using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if TEXTMESHPRO_2_0_0_OR_NEWER
using DropdownUI = TMPro.TMP_Dropdown;
#else
using DropdownUI = UnityEngine.UI.Dropdown;
#endif

namespace UniversalSettings {

    [DisallowMultipleComponent]
    public abstract class SettingsComponentDropdown : SettingsComponent {

        protected DropdownUI tmpDropdown = null;
        protected Dropdown legacyDropdown = null;

        protected bool invertedIndex = false;

        protected virtual ref int SettingsValue() {
            throw new Exception("SettingsValue was not implemented!");
        }

        protected virtual bool AutoApplyValue() {
            throw new Exception("AutoApplyValue was not implemented!");
        }

        protected virtual void OnValueChanged(int value) {
            SettingsValue() = ProcessValue(value);
            if(AutoApplyValue()) {
                AutoApply();
            }
			else {
                universalSettings.RegisterSettingsChange();
			}
        }

        protected void CreateOptions(List<string> options) {
            tmpDropdown?.options.Clear();
            legacyDropdown?.options.Clear();

            foreach(string option in options) {
                tmpDropdown?.options.Add(new DropdownUI.OptionData(option));
                legacyDropdown?.options.Add(new Dropdown.OptionData(option));
            }

            tmpDropdown?.SetValueWithoutNotify(0);
            legacyDropdown?.SetValueWithoutNotify(0);
        }
        
        protected void SetDropdownActive(bool value) {
            if(tmpDropdown) tmpDropdown.interactable = value;
            if(legacyDropdown) legacyDropdown.interactable = value;
        }

        internal override void Initialize(UniversalSettingsRunner universalSettings) {
            base.Initialize(universalSettings);

            tmpDropdown = GetComponent<DropdownUI>();
            if(tmpDropdown == null) {
                legacyDropdown = GetComponent<Dropdown>();
            }

            tmpDropdown?.onValueChanged.AddListener(OnValueChanged);
            legacyDropdown?.onValueChanged.AddListener(OnValueChanged);

            Setup();
        }

        internal override void UpdateComponent(SettingsProfile settings) {
            int processedValue = ProcessValue(SettingsValue());

            tmpDropdown?.SetValueWithoutNotify(processedValue);
            legacyDropdown?.SetValueWithoutNotify(processedValue);

            tmpDropdown?.RefreshShownValue();
            legacyDropdown?.RefreshShownValue();
        }

        protected void AddOption(string option) {
            tmpDropdown?.options.Add(new DropdownUI.OptionData(option));
            legacyDropdown?.options.Add(new Dropdown.OptionData(option));
        }

        protected void RemoveOption(int index) {
            tmpDropdown?.options.RemoveAt(index);
            legacyDropdown?.options.RemoveAt(index);
        }

        protected int GetOptionsCount() {
            if (tmpDropdown) return tmpDropdown.options.Count;
            return legacyDropdown.options.Count;
		}

        private int ProcessValue(int value) {
            int processedValue;
            int maxOptionValue = GetOptionsCount() - 1;

            if(invertedIndex) {
                processedValue = Math.Max(0, maxOptionValue - value);
			}
			else {
                processedValue = Math.Min(value, maxOptionValue);
			}

            return processedValue;
		}

		private void OnDestroy() {
            if (tmpDropdown) tmpDropdown.onValueChanged.RemoveListener(OnValueChanged);
            if (legacyDropdown) legacyDropdown.onValueChanged.RemoveListener(OnValueChanged);
		}

#if UNITY_EDITOR
        private void Reset() {
            DropdownUI _tmpDropdown = GetComponent<DropdownUI>();
            Dropdown _legacyDropdown = GetComponent<Dropdown>();

            if(_tmpDropdown == null && _legacyDropdown == null) {
                EditorUtility.DisplayDialog("Component not found", "Text Mesh Pro Dropdown or Legacy Dropdown component not found!", "Ok");
                DestroyImmediate(this);
            }
        }
#endif
    }
}
