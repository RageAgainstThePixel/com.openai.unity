// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEditor;
using UnityEngine;

namespace OpenAI.Editor
{
    public class FineTuningWindow : EditorWindow
    {
        private static readonly GUIContent guiTitleContent = new GUIContent("OpenAI Fine Tuning");

        private static GUIStyle boldCenteredHeaderStyle = null;

        public static GUIStyle BoldCenteredHeaderStyle
        {
            get
            {
                if (boldCenteredHeaderStyle == null)
                {
                    var editorStyle = EditorGUIUtility.isProSkin ? EditorStyles.whiteLargeLabel : EditorStyles.largeLabel;

                    if (editorStyle != null)
                    {
                        boldCenteredHeaderStyle = new GUIStyle(editorStyle)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontSize = 18,
                            padding = new RectOffset(0, 0, -8, -8)
                        };
                    }
                }

                return boldCenteredHeaderStyle;
            }
        }

        [MenuItem("OpenAI/Fine Tuning")]
        private static void OpenWindow()
        {
            // Dock it next to the Scene View.
            var instance = GetWindow<FineTuningWindow>(typeof(SceneView));
            instance.Show();
            instance.titleContent = guiTitleContent;
        }

        private void OnEnable()
        {
            titleContent = guiTitleContent;
            minSize = new Vector2(512, 256);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("OpenAI Fine Tuning", BoldCenteredHeaderStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
        }
    }
}
