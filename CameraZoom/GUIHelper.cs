using UnityEngine;

namespace ShipMorale
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

        public static bool IntField(ref int value, GUIStyle style = null, params GUILayoutOption[] options)
        {
            string strValue = value.ToString();
            strValue = GUILayout.TextField(strValue, new GUILayoutOption[] { });
            bool valid = int.TryParse(strValue, out value);
            return valid && 1 <= value && value <= 100;
        }
    }
}
