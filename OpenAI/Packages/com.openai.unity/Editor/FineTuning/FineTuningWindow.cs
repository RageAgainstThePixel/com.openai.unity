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

        private static readonly string[] tabTitles = { "Training Data", "Training Jobs", "Models" };

        private static readonly List<SerializedObject> fineTuningTrainingDataSets = new List<SerializedObject>();

        private static readonly List<Model> openAiModels = new List<Model>();

        private static readonly List<Model> organizationModels = new List<Model>();

        private static readonly List<FineTuneJob> fineTuneJobs = new List<FineTuneJob>();

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

        private static GUIStyle rightMiddleAlignedLabel;

        private static GUIStyle RightMiddleAlignedLabel => rightMiddleAlignedLabel ??= new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight };

        private static Vector2 scrollPosition = Vector2.zero;

        private static bool hasFetchedModels;

        private static bool isFetchingModels;

        private static GUIContent[] modelOptions;

        private static bool hasFetchedJobEvents;

        private static bool isFetchingJobEvents;

        [SerializeField]
        private int tab;

        [MenuItem("OpenAI/Fine Tuning")]
        private static void OpenWindow()
        {
            // Dock it next to the Scene View.
            var instance = GetWindow<FineTuningWindow>(typeof(SceneView));
            instance.Show();
            instance.titleContent = guiTitleContent;
            FetchAllModels();
        }

        private void OnEnable()
        {
            titleContent = guiTitleContent;
            minSize = new Vector2(512, 256);
        }

        private void OnFocus()
        {
            GatherTrainingDataSets();

            openAI ??= new OpenAIClient();

            if (!hasFetchedModels)
            {
                hasFetchedModels = true;
                FetchAllModels();
            }

            if (!hasFetchedJobEvents)
            {
                hasFetchedJobEvents = true;
                FetchAllTrainingJobs();
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

            tab = GUILayout.Toolbar(tab, tabTitles);

            EditorGUILayout.Space();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true));

            switch (tab)
            {
                case 0:
                    RenderTrainingDataSets();
                    break;
                case 1:
                    RenderTrainingJobQueue();
                    break;
                case 2:
                    RenderOrganizationModels();
                    break;
            }

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
                if (dataSet == null ||
                    dataSet.targetObject == null)
                {
                    continue;
                }

                dataSet.Update();
                var jobId = dataSet.FindProperty("id");
                if (jobId == null) { continue; }
                var baseModel = dataSet.FindProperty("baseModel");
                var modelSuffix = dataSet.FindProperty("modelSuffix");
                var trainingData = dataSet.FindProperty("trainingData");
                var isAdvanced = dataSet.FindProperty("advanced");

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

                    EditorGUI.BeginChangeCheck();
                    isAdvanced.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Use Advanced Settings", isAdvanced.tooltip), isAdvanced.boolValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        var batchSize = dataSet.FindProperty("batchSize");
                        batchSize.intValue = Mathf.RoundToInt(trainingData.arraySize * 0.2f);

                        if (batchSize.intValue == 0)
                        {
                            batchSize.intValue = 1;
                        }

                        if (batchSize.intValue > 256)
                        {
                            batchSize.intValue = 256;
                        }
                    }

                    if (isAdvanced.boolValue)
                    {
                        var epochs = dataSet.FindProperty("epochs");
                        var batchSize = dataSet.FindProperty("batchSize");
                        var learningRateMultiplier = dataSet.FindProperty("learningRateMultiplier");
                        var promptLossWeight = dataSet.FindProperty("promptLossWeight");

                        EditorGUI.indentLevel++;
                        isAdvanced.isExpanded = EditorGUILayout.Foldout(isAdvanced.isExpanded, "Advanced Model Training Options", true);

                        if (isAdvanced.isExpanded)
                        {
                            EditorGUI.indentLevel++;
                            var prevLabelWidth = EditorGUIUtility.labelWidth;
                            EditorGUIUtility.labelWidth = 256;
                            EditorGUILayout.PropertyField(epochs);
                            EditorGUILayout.PropertyField(batchSize);
                            EditorGUILayout.PropertyField(learningRateMultiplier);
                            EditorGUILayout.PropertyField(promptLossWeight);
                            EditorGUIUtility.labelWidth = prevLabelWidth;
                            EditorGUI.indentLevel--;
                        }

                        EditorGUI.indentLevel--;
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
                        EditorApplication.delayCall += () => TrainModel(dataSet);
                    }
                }

                EditorGUILayout.Space();
            }
        }

        private static async void TrainModel(SerializedObject serializedObject)
        {
            if (serializedObject.targetObject is not FineTuningTrainingDataSet fineTuneDataSet)
            {
                return;
            }

            var choice = EditorUtility.DisplayDialog(
                title: "Begin Training Model?",
                // TODO we might be able to estimate token and training cost.
                message: $"Would you like to begin training your custom {fineTuneDataSet.BaseModel} model?",
                ok: "Ok",
                cancel: "Cancel");

            if (!choice)
            {
                return;
            }

            try
            {
                var tempDir = Path.Combine($"{Application.temporaryCachePath}", "FineTuning");

                if (Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                var lines = fineTuneDataSet.TrainingData.Select(trainingData => trainingData.ToString()).ToList();
                var tempFilePath = Path.Combine(tempDir, $"{fineTuneDataSet.name}.jsonl");
                await File.WriteAllLinesAsync(tempFilePath, lines);
                var fileData = await openAI.FilesEndpoint.UploadFileAsync(tempFilePath, "fine-tune");
                File.Delete(tempFilePath);

                var jobRequest = new CreateFineTuneJobRequest(
                    trainingFileId: fileData.Id,
                    model: fineTuneDataSet.BaseModel,
                    suffix: fineTuneDataSet.ModelSuffix,
                    epochs: (uint)(fineTuneDataSet.Advanced ? fineTuneDataSet.Epochs : 4),
                    batchSize: fineTuneDataSet.Advanced ? fineTuneDataSet.BatchSize : null,
                    learningRateMultiplier: fineTuneDataSet.Advanced ? fineTuneDataSet.LearningRateMultiplier : null,
                    promptLossWeight: fineTuneDataSet.Advanced ? fineTuneDataSet.PromptLossWeight : 0.01f);
                var job = await openAI.FineTuningEndpoint.CreateFineTuneJobAsync(jobRequest);
                fineTuneDataSet.FineTuneJob = job;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private static void RenderTrainingJobQueue()
        {
            EditorGUI.indentLevel++;

            foreach (var job in fineTuneJobs)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(job.Id, EditorStyles.boldLabel, GUILayout.MaxWidth(Screen.width));

                if (!string.IsNullOrWhiteSpace(job.FineTunedModel))
                {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField(job.FineTunedModel, RightMiddleAlignedLabel, GUILayout.MaxWidth(Screen.width));
                    EditorGUILayout.Space(12);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Events:");

                var events = job.Events.OrderBy(e => e.CreatedAt);

                foreach (var jobEvent in events)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"[{jobEvent.Level}]", GUILayout.Width(72));
                    EditorGUILayout.LabelField($"{jobEvent.Message.Replace("\n", " ")}", GUILayout.MaxWidth(Screen.width), GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField($"{jobEvent.CreatedAt.ToLocalTime()}", GUILayout.Width(165));
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUI.indentLevel--;
        }

        private static async void FetchAllTrainingJobs()
        {
            if (isFetchingJobEvents) { return; }
            isFetchingJobEvents = true;

            try
            {
                var jobs = await openAI.FineTuningEndpoint.ListFineTuneJobsAsync();

                fineTuneJobs.Clear();

                foreach (var job in jobs)
                {
                    var jobDetails = await openAI.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(job);

                    if (jobDetails.Events.Any(IsStaleEvent))
                    {
                        continue;
                    }

                    fineTuneJobs.Add(jobDetails);

                    static bool IsStaleEvent(Event @event)
                    {
                        var eventTimeSpan = DateTimeOffset.Now - @event.CreatedAt;

                        if (eventTimeSpan >= TimeSpan.FromDays(7))
                        {
                            return true;
                        }

                        var message = @event.Message;

                        return message.Contains("cancelled");
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                isFetchingJobEvents = false;
            }
        }

        private static void RenderOrganizationModels()
        {
            EditorGUI.indentLevel++;

            foreach (var model in organizationModels)
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.Id))
                {
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(model, GUILayout.MaxWidth(Screen.width));
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(model.OwnedBy, RightMiddleAlignedLabel);
                EditorGUILayout.Space(12);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel--;
        }

        private static async void FetchAllModels()
        {
            if (isFetchingModels) { return; }
            isFetchingModels = true;

            try
            {
                var allModels = await openAI.ModelsEndpoint.GetModelsAsync();

                openAiModels.Clear();
                openAiModels.AddRange(allModels.Where(model =>
                {
                    var canFineTuneModel = model.Permissions?.Any(permission => permission.AllowFineTuning) ?? true;
                    return model.OwnedBy.Contains("openai") && canFineTuneModel;
                }));

                organizationModels.Clear();
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
    }
}
