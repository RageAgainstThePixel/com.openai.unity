// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.FineTuning;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Editor.FineTuning
{
    public class FineTuningWindow : EditorWindow
    {
        private static readonly GUIContent guiTitleContent = new GUIContent("OpenAI Fine Tuning");

        private static readonly List<SerializedObject> fineTuningTrainingDataSets = new List<SerializedObject>();

        private static readonly List<Model> openAiModels = new List<Model>();

        private static readonly List<Model> organizationModels = new List<Model>();

        private static OpenAIClient openAI;

        private static GUIStyle boldCenteredHeaderStyle;

        private static GUIStyle BoldCenteredHeaderStyle
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

        private static Vector2 scrollPosition = Vector2.zero;

        private static GUIContent[] modelOptions;

        private static bool isFetchingModels;

        [MenuItem("OpenAI/Fine Tuning")]
        private static void OpenWindow()
        {
            // Dock it next to the Scene View.
            var instance = GetWindow<FineTuningWindow>(typeof(SceneView));
            instance.Show();
            instance.titleContent = guiTitleContent;
            FetchModels();
        }

        private static async void FetchModels()
        {
            if (isFetchingModels) { return; }

            isFetchingModels = true;

            try
            {
                openAI = new OpenAIClient();
                var allModels = await openAI.ModelsEndpoint.GetModelsAsync().ConfigureAwait(false);
                openAiModels.Clear();
                openAiModels.AddRange(allModels.Where(model => model.OwnedBy.Contains("openai")));
                organizationModels.AddRange(allModels.Where(model => !model.OwnedBy.Contains("openai") && !model.OwnedBy.Contains("system")));

                modelOptions = new GUIContent[openAiModels.Count];

                for (var i = 0; i < openAiModels.Count; i++)
                {
                    modelOptions[i] = new GUIContent(openAiModels[i]);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // no access is granted yet.
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                isFetchingModels = false;
            }
        }

        private void OnEnable()
        {
            titleContent = guiTitleContent;
            minSize = new Vector2(512, 256);
        }

        private void OnFocus()
        {
            GatherTrainingDataSets();

            if (openAI == null || modelOptions == null)
            {
                FetchModels();
            }
        }

        private void OnLostFocus()
        {
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("OpenAI Model Fine Tuning", BoldCenteredHeaderStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (openAI != null &&
                string.IsNullOrWhiteSpace(openAI.OpenAIAuthentication.Organization))
            {
                EditorGUILayout.HelpBox($"No Organization has been identified in {nameof(OpenAIAuthentication)}. This tool requires that you set it in your configuration.", MessageType.Error);
            }

            EditorGUILayout.Space();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true));

            RenderTrainingDataSets();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private static void GatherTrainingDataSets()
        {
            var instances = AssetDatabase.FindAssets($"t:{nameof(FineTuningTrainingDataSet)}");

            fineTuningTrainingDataSets.Clear();

            foreach (var instance in instances)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(instance);
                var asset = AssetDatabase.LoadAssetAtPath<FineTuningTrainingDataSet>(assetPath);
                fineTuningTrainingDataSets.Add(new SerializedObject(asset));
            }
        }

        private static void RenderTrainingDataSets()
        {
            if (modelOptions == null || isFetchingModels) { return; }

            foreach (var dataSet in fineTuningTrainingDataSets)
            {
                if (dataSet == null) { continue; }
                dataSet.Update();
                var jobId = dataSet.FindProperty("id");
                if (jobId == null) { continue; }
                var baseModel = dataSet.FindProperty("baseModel");
                var modelSuffix = dataSet.FindProperty("modelSuffix");
                var trainingData = dataSet.FindProperty("trainingData");

                jobId.isExpanded = EditorGUILayout.Foldout(jobId.isExpanded, $"Fine tune job id: {jobId.stringValue}", true);

                if (jobId.isExpanded)
                {
                    EditorGUI.indentLevel++;

                    int modelIndex = -1;

                    for (int i = 0; i < modelOptions.Length; i++)
                    {
                        if (modelOptions[i].text == baseModel.stringValue)
                        {
                            modelIndex = i;
                            break;
                        }
                    }

                    EditorGUI.BeginChangeCheck();
                    modelIndex = EditorGUILayout.Popup(new GUIContent(baseModel.displayName, baseModel.tooltip), modelIndex, modelOptions);

                    if (EditorGUI.EndChangeCheck())
                    {
                        baseModel.stringValue = modelOptions[modelIndex].text;
                    }

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(modelSuffix);

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Model suffix characters can only be 40 characters in length
                        const int maxSuffixCharacters = 40;
                        if (modelSuffix.stringValue.Length > maxSuffixCharacters)
                        {
                            modelSuffix.stringValue = modelSuffix.stringValue[..maxSuffixCharacters];
                        }
                    }

                    EditorGUILayout.PropertyField(trainingData);
                    dataSet.ApplyModifiedProperties();

                    EditorGUI.indentLevel--;
                }

                if (openAI != null &&
                    jobId.stringValue.Contains("pending"))
                {
                    if (GUILayout.Button("Begin Training Model"))
                    {
                        TrainModel(dataSet);
                    }
                }
                else
                {
                    if (dataSet.targetObject is FineTuningTrainingDataSet { FineTuneJob: { } fineTuneJob })
                    {
                        EditorGUILayout.LabelField($"Training Status: {fineTuneJob.Status}");
                    }
                }
            }
        }

        private static async void TrainModel(SerializedObject serializedObject)
        {
            try
            {
                if (serializedObject.targetObject is not FineTuningTrainingDataSet fineTuneDataSet)
                {
                    return;
                }

                var tempDir = Path.Combine($"{Application.temporaryCachePath}", "FineTuning");

                if (Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                var lines = fineTuneDataSet.TrainingData.Select(trainingData => trainingData.ToString()).ToList();
                var tempFilePath = Path.Combine(tempDir, $"{fineTuneDataSet.name}.jsonl");
                await File.WriteAllLinesAsync(tempFilePath, lines);
                var fileData = await openAI.FilesEndpoint.UploadFileAsync(tempFilePath, "fine-tune").ConfigureAwait(true);
                File.Delete(tempFilePath);

                var jobRequest = new CreateFineTuneJobRequest(
                    trainingFileId: fileData.Id,
                    model: fineTuneDataSet.BaseModel,
                    suffix: fineTuneDataSet.ModelSuffix);
                var job = await openAI.FineTuningEndpoint.CreateFineTuneJobAsync(jobRequest).ConfigureAwait(true);
                fineTuneDataSet.FineTuneJob = job;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
