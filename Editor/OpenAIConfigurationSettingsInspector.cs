// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using UnityEditor;

namespace OpenAI.Editor
{
    [CustomEditor(typeof(OpenAIConfigurationSettings))]
    internal class OpenAIConfigurationSettingsInspector : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var update = false;
            var currentPath = AssetDatabase.GetAssetPath(target);

            if (string.IsNullOrWhiteSpace(currentPath))
            {
                return;
            }

            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
                update = true;
            }

            if (!currentPath.Contains("Resources"))
            {
                File.Move(Path.GetFullPath(currentPath), Path.GetFullPath($"Assets/Resources/{target.name}.asset"));
                File.Move(Path.GetFullPath($"{currentPath}.meta"), Path.GetFullPath($"Assets/Resources/{target.name}.asset.meta"));
                update = true;
            }

            if (update)
            {
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    EditorGUIUtility.PingObject(target);
                };
            }
        }
    }
}
