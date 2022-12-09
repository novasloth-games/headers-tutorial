#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Novasloth {
    public static class SetupHeaders {

        [MenuItem("Novasloth/Setup")]
        public static void SetupSceneObjects () {
            CreateGameObjects(
                "SYSTEMS",
                "CAMERAS",
                "GUI",
                "LIGHTS",
                "OBJECTS",
                ""
            );
        }

        private static void CreateGameObjects (params string[] names) {
            foreach (string name in names) {
                GameObject gameObject = new GameObject($"//{name}");
                gameObject.transform.position = Vector3.zero;
            }
        }
    }
}
#endif