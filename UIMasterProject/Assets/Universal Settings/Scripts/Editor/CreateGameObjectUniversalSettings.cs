using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalSettings.Editor {
    public class CreateGameObjectUniversalSettings {

        [MenuItem("GameObject/Universal Settings Runner", false, 10)]
        public static void CreateGameObject() {
            GameObject gameObject = new GameObject("Universal Settings Runner");

            gameObject.transform.SetParent(null);
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.AddComponent<UniversalSettingsRunner>();

            Selection.activeGameObject = gameObject;
        }
    }
}