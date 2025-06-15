// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEditor;
using UnityEngine;

namespace OpenAI.Editor
{
    [CustomPropertyDrawer(typeof(Voice))]
    public sealed class VoicePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var idProp = property.FindPropertyRelative("id");
            var currentId = idProp.stringValue;
            var selectedIndex = 0;

            for (var i = 0; i < Voice.All.Length; i++)
            {
                if (Voice.All[i] == currentId)
                {
                    selectedIndex = i;
                }
            }

            var newIndex = EditorGUI.Popup(position, label.text, selectedIndex, Voice.All);

            if (newIndex != selectedIndex)
            {
                idProp.stringValue = Voice.All[newIndex];
            }
        }
    }
}
