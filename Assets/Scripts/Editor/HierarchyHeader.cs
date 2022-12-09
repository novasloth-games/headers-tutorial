#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Novasloth {

    [InitializeOnLoad]
    public static class HierarchyHeader {

        private const string HEADER_STARTS_WITH = "//";
        private static Color HEADER_COLOR = Color.black;

        static HierarchyHeader () {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyHeader;
        }

        private static void OnHierarchyHeader (int instanceId, Rect rect) {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

            if (gameObject == null) return;

            if (gameObject.name.StartsWith(HEADER_STARTS_WITH, System.StringComparison.Ordinal)) {
                EditorGUI.DrawRect(rect, HEADER_COLOR);

                EditorGUI.DropShadowLabel(
                    rect,
                    gameObject.name.Replace(HEADER_STARTS_WITH, "").ToUpperInvariant()
                );
            }
        }
    }
}
#endif
