using UnityEngine;

namespace CombatLogExporter
{
    public class GUIHelper
    {
        public static void Toggle(ref bool value, string text, string tooltip)
        {
            GUILayoutOption[] emptyOptions = new GUILayoutOption[0];
            GUILayout.BeginHorizontal(emptyOptions);
            GUIContent val = (tooltip == null) ? new GUIContent(text) : new GUIContent(text, tooltip);
            value = GUILayout.Toggle(value, val, new GUILayoutOption[1] {
                GUILayout.ExpandWidth(false)
            });
            GUILayout.EndHorizontal();
        }

        public static void Label(string text)
        {
            GUILayout.Label(text, new GUILayoutOption[] { } );
        }

        public static void TextField(ref string value, GUIStyle style = null, params GUILayoutOption[] options)
        {
            value = GUILayout.TextField(value, new GUILayoutOption[] { });
        }
    }
}
