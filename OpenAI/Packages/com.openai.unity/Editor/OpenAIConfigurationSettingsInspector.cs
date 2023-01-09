// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Editor
{
    [CustomEditor(typeof(OpenAIConfigurationSettings))]
    internal class OpenAIConfigurationSettingsInspector : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var update = false;
            var instance = target as OpenAIConfigurationSettings;
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
                var newPath = $"Assets/Resources/{instance!.name}.asset";

                if (!File.Exists(newPath))
                {
                    File.Move(Path.GetFullPath(currentPath), Path.GetFullPath(newPath));
                    File.Move(Path.GetFullPath($"{currentPath}.meta"), Path.GetFullPath($"{newPath}.meta"));
                }
                else
                {
                    AssetDatabase.DeleteAsset(currentPath);
                    var instances = AssetDatabase.FindAssets($"t:{nameof(OpenAIConfigurationSettings)}");
                    var path = AssetDatabase.GUIDToAssetPath(instances[0]);
                    instance = AssetDatabase.LoadAssetAtPath<OpenAIConfigurationSettings>(path);
                }

                update = true;
            }

            if (update)
            {
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    EditorGUIUtility.PingObject(instance);
                };
            }
        }
    }
}
