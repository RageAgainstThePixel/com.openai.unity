// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Editor
{
    [CustomEditor(typeof(OpenAIConfigurationSettings))]
    internal class OpenAIConfigurationSettingsInspector : UnityEditor.Editor
    {
        private SerializedProperty apiKey;
        private SerializedProperty organizationId;
        private SerializedProperty useAzureOpenAI;
        private SerializedProperty proxyDomain;
        private SerializedProperty resourceName;
        private SerializedProperty deploymentId;
        private SerializedProperty apiVersion;

        #region Project Settings Window

        [SettingsProvider]
        private static SettingsProvider Preferences()
        {
            return new SettingsProvider("Project/OpenAI", SettingsScope.Project, new[] { "OpenAI" })
            {
                label = "OpenAI",
                guiHandler = OnPreferencesGui,
                keywords = new[] { "OpenAI" }
            };
        }

        private static void OnPreferencesGui(string searchContext)
        {
            if (EditorApplication.isPlaying ||
                EditorApplication.isCompiling)
            {
                return;
            }

            var instance = GetOrCreateInstance();
            var instanceEditor = CreateEditor(instance);
            instanceEditor.OnInspectorGUI();
        }

        #endregion Project Settings Window

        #region Inspector Window

        private void OnEnable()
        {
            GetOrCreateInstance(target);
            apiKey = serializedObject.FindProperty(nameof(apiKey));
            organizationId = serializedObject.FindProperty(nameof(organizationId));
            useAzureOpenAI = serializedObject.FindProperty(nameof(useAzureOpenAI));
            proxyDomain = serializedObject.FindProperty(nameof(proxyDomain));
            resourceName = serializedObject.FindProperty(nameof(resourceName));
            deploymentId = serializedObject.FindProperty(nameof(deploymentId));
            apiVersion = serializedObject.FindProperty(nameof(apiVersion));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            EditorGUI.BeginChangeCheck();
            var useAzureOpenAIContent = new GUIContent(useAzureOpenAI.displayName, useAzureOpenAI.tooltip);
            useAzureOpenAI.boolValue = EditorGUILayout.ToggleLeft(useAzureOpenAIContent, useAzureOpenAI.boolValue);

            if (EditorGUI.EndChangeCheck())
            {
                apiVersion.stringValue = useAzureOpenAI.boolValue ?
                    OpenAIClientSettings.DefaultAzureApiVersion :
                    OpenAIClientSettings.DefaultOpenAIApiVersion;
            }

            EditorGUILayout.PropertyField(apiKey);

            if (!string.IsNullOrWhiteSpace(apiKey.stringValue))
            {
                if (!useAzureOpenAI.boolValue)
                {
                    if (!apiKey.stringValue.StartsWith(AuthInfo.SecretKeyPrefix))
                    {
                        EditorGUILayout.HelpBox($"{nameof(apiKey)} must start with '{AuthInfo.SecretKeyPrefix}' unless using Azure OpenAI", MessageType.Error);
                    }
                }
                else
                {
                    if (apiKey.stringValue.StartsWith(AuthInfo.SecretKeyPrefix))
                    {
                        EditorGUILayout.HelpBox($"{nameof(apiKey)} must not start with '{AuthInfo.SecretKeyPrefix}' when using Azure OpenAI", MessageType.Error);
                    }
                }
            }

            if (!useAzureOpenAI.boolValue)
            {
                EditorGUILayout.PropertyField(organizationId);

                if (!string.IsNullOrWhiteSpace(organizationId.stringValue))
                {
                    if (!organizationId.stringValue.StartsWith(AuthInfo.OrganizationPrefix))
                    {
                        EditorGUILayout.HelpBox($"{nameof(organizationId)} must start with '{AuthInfo.OrganizationPrefix}'", MessageType.Error);
                    }
                }

                EditorGUILayout.PropertyField(proxyDomain);
            }
            else
            {
                EditorGUILayout.PropertyField(resourceName);
                EditorGUILayout.PropertyField(deploymentId);
            }

            GUI.enabled = useAzureOpenAI.boolValue;
            EditorGUILayout.PropertyField(apiVersion);
            GUI.enabled = true;

            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }

        #endregion Inspector Window

        private static OpenAIConfigurationSettings GetOrCreateInstance(Object target = null)
        {
            var update = false;
            OpenAIConfigurationSettings instance;

            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
                update = true;
            }

            if (target != null)
            {
                instance = target as OpenAIConfigurationSettings;

                var currentPath = AssetDatabase.GetAssetPath(instance);

                if (string.IsNullOrWhiteSpace(currentPath))
                {
                    return instance;
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
            }
            else
            {
                var instances = AssetDatabase.FindAssets($"t:{nameof(OpenAIConfigurationSettings)}");

                if (instances.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(instances[0]);
                    instance = AssetDatabase.LoadAssetAtPath<OpenAIConfigurationSettings>(path);
                }
                else
                {
                    instance = CreateInstance<OpenAIConfigurationSettings>();
                    AssetDatabase.CreateAsset(instance, $"Assets/Resources/{nameof(OpenAIConfigurationSettings)}.asset");
                    update = true;
                }
            }

            if (update)
            {
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    EditorGUIUtility.PingObject(instance);
                };
            }

            return instance;
        }
    }
}
