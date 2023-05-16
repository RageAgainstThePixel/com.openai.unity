// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.FineTuning;
using OpenAI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Progress = Utilities.WebRequestRest.Progress;

namespace OpenAI.Editor.FineTuning
{
    public class FineTuningWindow : EditorWindow
    {
        private static readonly GUIContent guiTitleContent = new GUIContent("OpenAI Fine Tuning");

        private static readonly string[] tabTitles = { "Training Data", "Training Jobs", "Models" };

        private static readonly List<SerializedObject> fineTuningTrainingDataSets = new List<SerializedObject>();

        private static readonly List<Model> organizationModels = new List<Model>();

        private static readonly ConcurrentDictionary<string, FineTuneJob> fineTuneJobs = new ConcurrentDictionary<string, FineTuneJob>();

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

            try
            {
                openAI ??= new OpenAIClient();
            }
            catch (Exception)
            {
                // Ignored
                return;
            }

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

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("OpenAI Model Fine Tuning", BoldCenteredHeaderStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (openAI is not { HasValidAuthentication: true })
            {

                EditorGUILayout.HelpBox($"No valid {nameof(OpenAIConfiguration)} was found. This tool requires that you set your API key.", MessageType.Error);
                EditorGUILayout.EndVertical();
                return;
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
                if (dataSet is not { targetObject: FineTuningTrainingDataSet })
                {
                    continue;
                }

                dataSet.Update();
                var jobId = dataSet.FindProperty("id");
                if (jobId == null) { continue; }
                var jobStatus = dataSet.FindProperty("status");
                var baseModel = dataSet.FindProperty("baseModel");
                var modelSuffix = dataSet.FindProperty("modelSuffix");
                var trainingData = dataSet.FindProperty("trainingData");
                var isAdvanced = dataSet.FindProperty("advanced");

                var prevLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = Screen.width;
                jobId.isExpanded = EditorGUILayout.Foldout(jobId.isExpanded, $"Fine tune job: {jobId.stringValue}", true);
                EditorGUIUtility.labelWidth = prevLabelWidth;

                if (jobId.isExpanded)
                {
                    EditorGUI.indentLevel++;

                    var modelIndex = -1;

                    for (var i = 0; i < modelOptions.Length; i++)
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
                            prevLabelWidth = EditorGUIUtility.labelWidth;
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

                if (openAI != null)
                {
                    if (jobStatus.stringValue.Contains("not started") ||
                        jobStatus.stringValue.Contains("cancelled") ||
                        jobStatus.stringValue.Contains("succeeded"))
                    {
                        FineTuneJob fineTuneJob = null;

                        if (jobStatus.stringValue.Contains("succeeded"))
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"Status: {jobStatus.stringValue}", GUILayout.MaxWidth(128));
                            GUILayout.FlexibleSpace();

                            if (fineTuneJobs.TryGetValue(jobId.stringValue, out fineTuneJob))
                            {
                                EditorGUILayout.LabelField(fineTuneJob.FineTunedModel, RightMiddleAlignedLabel, GUILayout.MaxWidth(Screen.width));
                            }

                            GUILayout.Space(10);
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.indentLevel--;
                        }

                        EditorGUILayout.BeginHorizontal();

                        if (GUILayout.Button("Train new Model"))
                        {
                            EditorApplication.delayCall += () => TrainModel(dataSet);
                        }

                        var trainingFile = fineTuneJob?.ResultFiles?.FirstOrDefault();

                        if (trainingFile != null &&
                            GUILayout.Button("Download training results"))
                        {
                            EditorApplication.delayCall += async () =>
                            {
                                try
                                {
                                    void ProgressHandler(Progress report) => EditorUtility.DisplayProgressBar($"Downloading {trainingFile}", $"Downloading {trainingFile} @ {report.Speed} {report.Unit}/s", report.Percentage * 0.01f);
                                    var downloadPath = await openAI.FilesEndpoint.DownloadFileAsync(trainingFile, new Progress<Progress>(ProgressHandler)).ConfigureAwait(true);
                                    EditorUtility.ClearProgressBar();
                                    EditorUtility.RevealInFinder(downloadPath);
                                }
                                catch (Exception e)
                                {
                                    EditorUtility.ClearProgressBar();
                                    Debug.LogError(e);
                                }
                            };
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        RenderJobStatus(dataSet);
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

                if (!Directory.Exists(tempDir))
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

                FetchAllTrainingJobs();
                StreamJobEvents(fineTuneDataSet);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private static async void StreamJobEvents(FineTuningTrainingDataSet dataSet)
        {
            if (dataSet.FineTuneJob == null)
            {
                return;
            }

            try
            {
                await openAI.FineTuningEndpoint.StreamFineTuneEventsAsync(dataSet.FineTuneJob, async _ =>
                {
                    dataSet.FineTuneJob = await openAI.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(dataSet.FineTuneJob);

                    if (fineTuneJobs.TryGetValue(dataSet.FineTuneJob.Id, out var job))
                    {
                        fineTuneJobs.TryUpdate(dataSet.FineTuneJob.Id, dataSet.FineTuneJob, job);
                    }

                    if (dataSet.FineTuneJob.Status.Equals("succeeded"))
                    {
                        FetchAllModels();
                    }
                });
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case TaskCanceledException:
                    case OperationCanceledException:
                        // Ignored
                        break;
                    default:
                        Debug.LogError(e);
                        break;
                }
            }
        }

        private static void RenderJobStatus(SerializedObject serializedObject)
        {
            if (serializedObject.targetObject is not FineTuningTrainingDataSet fineTuneDataSet)
            {
                return;
            }

            if (fineTuneDataSet.FineTuneJob == null)
            {
                if (fineTuneJobs.TryGetValue(fineTuneDataSet.Id, out var job))
                {
                    fineTuneDataSet.FineTuneJob = job;
                }
                else
                {
                    return;
                }
            }

            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Status: {fineTuneDataSet.FineTuneJob.Status}");

            var canCancel = fineTuneDataSet.Status.Equals("pending") || fineTuneDataSet.Status.Equals("running");

            if (canCancel && GUILayout.Button("Cancel Training"))
            {
                EditorApplication.delayCall += async () =>
                {
                    var choice = EditorUtility.DisplayDialog(
                        title: "Cancel Training Model?",
                        message: $"Are you sure you want to cancel {fineTuneDataSet.Id}?",
                        ok: "Ok",
                        cancel: "Cancel");

                    if (!choice)
                    {
                        return;
                    }

                    fineTuneDataSet.Status = "cancelled";

                    try
                    {
                        await openAI.FineTuningEndpoint.CancelFineTuneJobAsync(fineTuneDataSet.Id);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                };
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private static void RenderTrainingJobQueue()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = !isFetchingJobEvents;

            if (GUILayout.Button("Refresh"))
            {
                EditorApplication.delayCall += FetchAllTrainingJobs;
            }

            EditorGUILayout.Space(10);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            var jobs = fineTuneJobs.Values.OrderByDescending(job => job.CreatedAt).ToList();

            foreach (var job in jobs)
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
                EditorGUILayout.LabelField($"Status: {job.Status}");
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

                EditorGUILayout.Space(10);
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
                jobs = jobs.OrderByDescending(job => job.UpdatedAt).ToList();

                fineTuneJobs.Clear();
                await Task.WhenAll(jobs.Select(SyncJobDataAsync));

                static async Task SyncJobDataAsync(FineTuneJob job)
                {
                    var jobIsCancelled = job.Status.Contains("cancelled");

                    if (jobIsCancelled || IsStale(job.UpdatedAt))
                    {
                        return;
                    }

                    var jobDetails = await openAI.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(job);

                    fineTuneJobs.TryAdd(jobDetails.Id, jobDetails);

                    static bool IsStale(DateTime dateTime)
                    {
                        var timeSpan = DateTimeOffset.Now - dateTime;
                        return timeSpan >= TimeSpan.FromDays(7);
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
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = !isFetchingModels;

            if (GUILayout.Button("Refresh"))
            {
                EditorApplication.delayCall += FetchAllModels;
            }

            EditorGUILayout.Space(10);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
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

                if (GUILayout.Button("Delete"))
                {
                    EditorApplication.delayCall += async () =>
                    {
                        var choice = EditorUtility.DisplayDialog(
                            title: "Delete Fine Tuned Model?",
                            message: $"Are you sure you want to delete {model}?\n\nYou can only perform this action if you are the organization owner.",
                            ok: "Ok",
                            cancel: "Cancel");

                        if (!choice)
                        {
                            return;
                        }

                        try
                        {
                            var wasDeleted = await openAI.ModelsEndpoint.DeleteFineTuneModelAsync(model);

                            var message = wasDeleted
                                ? $"{model} was successfully deleted"
                                : "Your model was **NOT** deleted.";

                            EditorUtility.DisplayDialog("Delete Fine Tuned Model Result", message, "Ok");
                        }
                        catch (UnauthorizedAccessException)
                        {
                            EditorUtility.DisplayDialog("Unauthorized", "You do not have permissions to delete models for this organization.", "Ok");
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    };
                }
                EditorGUILayout.Space(12);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }

            EditorGUI.indentLevel--;
        }

        private static async void FetchAllModels()
        {
            if (isFetchingModels || openAI == null) { return; }
            isFetchingModels = true;

            try
            {
                var allModels = (await openAI.ModelsEndpoint.GetModelsAsync()).OrderBy(model => model.Id).ToList();
                var modelOptionList = new List<string>
                {
                    "ada",
                    "babbage",
                    "curie",
                    "davinci"
                };

                foreach (var model in allModels)
                {
                    var canFineTuneModel = model.Permissions?.Any(permission => permission.AllowFineTuning) ?? false;

                    if (canFineTuneModel)
                    {
                        modelOptionList.Add(model.Id);
                    }
                }

                organizationModels.Clear();
                organizationModels.AddRange(allModels.Where(model => !model.OwnedBy.Contains("openai") && !model.OwnedBy.Contains("system")));
                modelOptionList.AddRange(organizationModels.Select(model => model.Id));

                modelOptions = new GUIContent[modelOptionList.Count];

                for (var i = 0; i < modelOptionList.Count; i++)
                {
                    modelOptions[i] = new GUIContent(modelOptionList[i]);
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
