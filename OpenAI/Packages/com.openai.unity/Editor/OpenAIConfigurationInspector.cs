// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEditor;
using UnityEngine;
using Utilities.Rest.Editor;

namespace OpenAI.Editor
{
    [CustomEditor(typeof(OpenAIConfiguration))]
    internal class OpenAIConfigurationInspector : BaseConfigurationInspector<OpenAIConfiguration>
    {
        private static bool triggerReload;

        private SerializedProperty apiKey;
        private SerializedProperty organizationId;
        private SerializedProperty useAzureOpenAI;
        private SerializedProperty proxyDomain;
        private SerializedProperty resourceName;
        private SerializedProperty deploymentId;
        private SerializedProperty useAzureActiveDirectory;
        private SerializedProperty apiVersion;

        #region Project Settings Window

        [SettingsProvider]
        private static SettingsProvider Preferences()
            => GetSettingsProvider(nameof(OpenAI), CheckReload);

        #endregion Project Settings Window

        #region Inspector Window

        private void OnEnable()
        {
            GetOrCreateInstance(target);

            try
            {
                apiKey = serializedObject.FindProperty(nameof(apiKey));
                organizationId = serializedObject.FindProperty(nameof(organizationId));
                useAzureOpenAI = serializedObject.FindProperty(nameof(useAzureOpenAI));
                proxyDomain = serializedObject.FindProperty(nameof(proxyDomain));
                resourceName = serializedObject.FindProperty(nameof(resourceName));
                deploymentId = serializedObject.FindProperty(nameof(deploymentId));
                useAzureActiveDirectory = serializedObject.FindProperty(nameof(useAzureActiveDirectory));
                apiVersion = serializedObject.FindProperty(nameof(apiVersion));
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        private void OnDisable() => CheckReload();

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
                triggerReload = true;
                apiVersion.stringValue = useAzureOpenAI.boolValue
                    ? OpenAISettingsInfo.DefaultAzureApiVersion
                    : OpenAISettingsInfo.DefaultOpenAIApiVersion;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(apiKey);

            if (!string.IsNullOrWhiteSpace(apiKey.stringValue))
            {
                if (!useAzureOpenAI.boolValue)
                {
                    if (!apiKey.stringValue.StartsWith(OpenAIAuthInfo.SecretKeyPrefix))
                    {
                        EditorGUILayout.HelpBox($"{nameof(apiKey)} must start with '{OpenAIAuthInfo.SecretKeyPrefix}' unless using Azure OpenAI", MessageType.Error);
                    }
                }
                else
                {
                    if (apiKey.stringValue.StartsWith(OpenAIAuthInfo.SecretKeyPrefix))
                    {
                        EditorGUILayout.HelpBox($"{nameof(apiKey)} must not start with '{OpenAIAuthInfo.SecretKeyPrefix}' when using Azure OpenAI", MessageType.Error);
                    }
                }
            }

            if (!useAzureOpenAI.boolValue)
            {
                EditorGUILayout.PropertyField(organizationId);

                if (!string.IsNullOrWhiteSpace(organizationId.stringValue))
                {
                    if (!organizationId.stringValue.StartsWith(OpenAIAuthInfo.OrganizationPrefix))
                    {
                        EditorGUILayout.HelpBox($"{nameof(organizationId)} must start with '{OpenAIAuthInfo.OrganizationPrefix}'", MessageType.Error);
                    }
                }

                EditorGUILayout.PropertyField(proxyDomain);
            }
            else
            {
                EditorGUILayout.PropertyField(resourceName);
                EditorGUILayout.PropertyField(deploymentId);
                var useAzureActiveDirectoryContent = new GUIContent(useAzureActiveDirectory.displayName, useAzureActiveDirectory.tooltip);
                useAzureActiveDirectory.boolValue = EditorGUILayout.ToggleLeft(useAzureActiveDirectoryContent, useAzureActiveDirectory.boolValue);
            }

            GUI.enabled = useAzureOpenAI.boolValue;
            EditorGUILayout.PropertyField(apiVersion);
            GUI.enabled = true;

            if (EditorGUI.EndChangeCheck())
            {
                triggerReload = true;
            }

            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }

        #endregion Inspector Window

        private static void CheckReload()
        {
            if (triggerReload)
            {
                triggerReload = false;
                EditorUtility.RequestScriptReload();
            }
        }
    }
}
