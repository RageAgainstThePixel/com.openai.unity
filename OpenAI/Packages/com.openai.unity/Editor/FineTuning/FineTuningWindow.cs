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
using Utilities.Extensions.Editor;
using Progress = Utilities.WebRequestRest.Progress;

namespace OpenAI.Editor.FineTuning
{
    public class FineTuningWindow : EditorWindow
    {
        private const int EndWidth = 10;
        private const float WideColumnWidth = 128f;
        private const float DefaultColumnWidth = 96f;

        private static readonly GUIContent guiTitleContent = new GUIContent("OpenAI Fine Tuning");

        private static readonly string[] tabTitles = { "Training Data", "Training Jobs", "Models" };

        private static readonly GUIContent deleteContent = new GUIContent("Delete");

        private static readonly GUIContent refreshContent = new GUIContent("Refresh");

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

        private static readonly GUILayoutOption[] defaultColumnWidthOption =
        {
            GUILayout.Width(DefaultColumnWidth)
        };

        private static readonly GUILayoutOption[] wideColumnWidthOption =
        {
            GUILayout.Width(WideColumnWidth)
        };

        private static readonly GUILayoutOption[] expandWidthOption =
        {
            GUILayout.ExpandWidth(true)
        };

        private static readonly GUILayoutOption[] squareWidthOption =
        {
            GUILayout.Width(24)
        };

        private static Vector2 scrollPosition = Vector2.zero;

        private static bool hasFetchedModels;

        private static bool isFetchingModels;

        private static GUIContent[] modelOptions;

        private static bool hasFetchedJobEvents;

        private static bool isFetchingJobs;

        [SerializeField]
        private int tab;

        [MenuItem("Window/Dashboard/OpenAI/Fine Tuning")]
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
                FetchTrainingJobs();
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

            if (GUILayout.Button("Create new Training Data Set"))
            {
                EditorApplication.delayCall += () =>
                {
                    var fineTuningDirectory = $"{Application.dataPath}/OpenAI/Editor/FineTuningJobs";

                    if (!Directory.Exists(fineTuningDirectory))
                    {
                        Directory.CreateDirectory(fineTuningDirectory);
                    }

                    var newTrainingSetDataInstance = CreateInstance<FineTuningTrainingDataSet>().CreateAsset(fineTuningDirectory);
                    fineTuningTrainingDataSets.Add(new SerializedObject(newTrainingSetDataInstance));

                    EditorApplication.delayCall += AssetDatabase.Refresh;
                };
            }

            EditorGUILayout.Space();

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
                var autoEpochs = dataSet.FindProperty("autoEpochs");
                var epochs = dataSet.FindProperty("epochs");
                var trainingData = dataSet.FindProperty("conversationTrainingData");

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

                    EditorGUILayout.PropertyField(autoEpochs);

                    if (!autoEpochs.boolValue)
                    {
                        EditorGUILayout.PropertyField(epochs);
                    }

                    EditorGUILayout.PropertyField(trainingData);
                    dataSet.ApplyModifiedProperties();

                    EditorGUI.indentLevel--;
                }

                if (openAI != null)
                {
                    if (jobStatus.intValue is
                        (int)JobStatus.NotStarted or
                        (int)JobStatus.Cancelled or
                        (int)JobStatus.Succeeded)
                    {
                        FineTuneJob fineTuneJob = null;

                        if (jobStatus.intValue == (int)JobStatus.Succeeded)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"Status: {(JobStatus)jobStatus.intValue}", GUILayout.MaxWidth(128));
                            GUILayout.FlexibleSpace();

                            if (fineTuneJobs.TryGetValue(jobId.stringValue, out fineTuneJob))
                            {
                                EditorGUILayout.LabelField(fineTuneJob.FineTunedModel, RightMiddleAlignedLabel, GUILayout.MaxWidth(Screen.width));
                            }

                            GUILayout.Space(EndWidth);
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.indentLevel--;
                        }

                        EditorGUILayout.BeginHorizontal();
                        GUI.enabled = trainingData.arraySize > 10;

                        if (GUILayout.Button("Train New Model"))
                        {
                            EditorApplication.delayCall += () => TrainModel(dataSet);
                        }

                        GUI.enabled = true;

                        var trainingFile = fineTuneJob?.ResultFiles?.FirstOrDefault();

                        if (trainingFile != null &&
                            GUILayout.Button("Download training results"))
                        {
                            EditorApplication.delayCall += async () =>
                            {
                                try
                                {
                                    void ProgressHandler(Progress report) => EditorUtility.DisplayProgressBar($"Downloading {trainingFile}", $"Downloading {trainingFile} @ {report.Speed} {report.Unit}/s", report.Percentage * 0.01f);
                                    var downloadPath = await openAI.FilesEndpoint.DownloadFileAsync(trainingFile, new Progress<Progress>(ProgressHandler));
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

                var lines = fineTuneDataSet.ConversationTrainingData.Select(trainingData => trainingData.ToString()).ToList();
                var tempFilePath = Path.Combine(tempDir, $"{fineTuneDataSet.name}.jsonl");
                await File.WriteAllLinesAsync(tempFilePath, lines);
                var fileData = await openAI.FilesEndpoint.UploadFileAsync(tempFilePath, "fine-tune");
                File.Delete(tempFilePath);

                var jobRequest = new CreateFineTuneJobRequest(
                    model: fineTuneDataSet.BaseModel,
                    trainingFileId: fileData.Id,
                    suffix: fineTuneDataSet.ModelSuffix,
                    hyperParameters: new HyperParameters(
                        epochs: fineTuneDataSet.AutoEpochs ? null : fineTuneDataSet.Epochs,
                        batchSize: fineTuneDataSet.AutoBatchSize ? null : fineTuneDataSet.BatchSize,
                        learningRateMultiplier: fineTuneDataSet.AutoLearningRateMultiplier ? null : fineTuneDataSet.LearningRateMultiplier));
                var job = await openAI.FineTuningEndpoint.CreateJobAsync(jobRequest);
                fineTuneDataSet.FineTuneJob = job;
                FetchTrainingJobs();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
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

            var canCancel = fineTuneDataSet.Status is > JobStatus.NotStarted and < JobStatus.Succeeded;

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

                    fineTuneDataSet.Status = JobStatus.Cancelled;

                    try
                    {
                        await openAI.FineTuningEndpoint.CancelJobAsync(fineTuneDataSet.Id);
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
            EditorGUI.BeginChangeCheck();
            trainingJobCount = EditorGUILayout.IntField("Jobs per page", trainingJobCount);

            if (EditorGUI.EndChangeCheck())
            {
                if (trainingJobCount > 100)
                {
                    trainingJobCount = 100;
                }

                if (trainingJobCount < 1)
                {
                    trainingJobCount = 1;
                }
            }

            GUILayout.FlexibleSpace();

            GUI.enabled = !isFetchingJobs;
            if (fineTuneJobList is not null && trainingJobIds.Count > 0 && GUILayout.Button("Prev Page", defaultColumnWidthOption))
            {
                EditorApplication.delayCall += () =>
                {
                    if (trainingJobIds.TryPeek(out var prevPageId))
                    {
                        FetchTrainingJobs(prevPageId);
                    }
                };
            }

            if (fineTuneJobList is { HasMore: true } && GUILayout.Button("Next Page", defaultColumnWidthOption))
            {
                EditorApplication.delayCall += () => FetchTrainingJobs(fineTuneJobList.Jobs.LastOrDefault());
            }

            if (GUILayout.Button(refreshContent, defaultColumnWidthOption))
            {
                EditorApplication.delayCall += () => FetchTrainingJobs();
            }

            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            var jobs = fineTuneJobs.Values.OrderByDescending(job => job.CreatedAt).ToList();

            foreach (var job in jobs)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(job.Id, EditorStyles.boldLabel, GUILayout.MaxWidth(Screen.width), GUILayout.ExpandWidth(true));
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(job.Status.ToString(), RightMiddleAlignedLabel);
                EditorGUILayout.Space(EndWidth);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"Base Model: {job.Model}");
                EditorGUILayout.LabelField($"Created At: {job.CreatedAt}");

                if (!string.IsNullOrWhiteSpace(job.FineTunedModel))
                {
                    EditorGUILayout.LabelField("Fine Tuned Model:");

                    EditorGUI.indentLevel++;
                    EditorGUILayout.SelectableLabel(job.FineTunedModel);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.LabelField($"Trained Tokens: {job.TrainedTokens}");

                if (job.HyperParameters != null)
                {
                    if (job.HyperParameters.Epochs != null)
                    {
                        EditorGUILayout.LabelField($"Epochs: {job.HyperParameters.Epochs}");
                    }

                    if (job.HyperParameters.BatchSize != null)
                    {
                        EditorGUILayout.LabelField($"BatchSize: {job.HyperParameters.BatchSize}");
                    }

                    if (job.HyperParameters.LearningRateMultiplier != null)
                    {
                        EditorGUILayout.LabelField($"Learning Rate Multiplier: {job.HyperParameters.LearningRateMultiplier}");
                    }
                }

                EditorGUILayout.LabelField("Events:");

                var events = job.Events.OrderBy(e => e.CreatedAt);

                foreach (var jobEvent in events)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"[{jobEvent.Level}]", GUILayout.Width(80f));
                    EditorGUILayout.LabelField($"{jobEvent.Message.Replace("\n", " ")}", GUILayout.MaxWidth(Screen.width), GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField($"{jobEvent.CreatedAt.ToLocalTime()}", GUILayout.Width(173f));
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space(EndWidth);
                EditorGUILayout.EndVertical();
                EditorGUILayoutExtensions.Divider();
            }

            EditorGUI.indentLevel--;
        }

        private static FineTuneJobList fineTuneJobList;
        private static int trainingJobCount = 25;
        private static readonly Stack<string> trainingJobIds = new Stack<string>();

        private static async void FetchTrainingJobs(string trainingJobId = null)
        {
            if (isFetchingJobs) { return; }
            isFetchingJobs = true;

            try
            {
                if (string.IsNullOrWhiteSpace(trainingJobId) &&
                    trainingJobIds.Count > 0 &&
                    trainingJobIds.TryPeek(out var prevPageId))
                {
                    trainingJobId = prevPageId;
                }
                else
                {
                    if (trainingJobIds.TryPeek(out prevPageId) && prevPageId == trainingJobId)
                    {
                        trainingJobIds.Pop();
                        trainingJobId = trainingJobIds.Count > 0 && trainingJobIds.TryPeek(out prevPageId) ? prevPageId : null;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(trainingJobId))
                        {
                            trainingJobIds.Push(trainingJobId);
                        }
                    }
                }

                fineTuneJobList = null;
                var list = await openAI.FineTuningEndpoint.ListJobsAsync(limit: trainingJobCount, after: trainingJobId);
                fineTuneJobs.Clear();
                await Task.WhenAll(list.Jobs.Select(SyncJobDataAsync));
                fineTuneJobList = list;

                static async Task SyncJobDataAsync(FineTuneJob job)
                {
                    var jobDetails = await openAI.FineTuningEndpoint.GetJobInfoAsync(job);
                    fineTuneJobs.TryAdd(jobDetails.Id, jobDetails);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                isFetchingJobs = false;
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

            EditorGUILayout.Space(EndWidth);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(EndWidth);
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
                EditorGUILayout.Space(EndWidth);

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
                            EditorUtility.DisplayDialog("Unauthorized", "You have insufficient permissions for this operation. You need to be this role: Owner.", "Ok");
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                        finally
                        {
                            FetchAllModels();
                        }
                    };
                }
                EditorGUILayout.Space(EndWidth);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(EndWidth);
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
                    "gpt-3.5-turbo",
                    "babbage-002",
                    "davinci-002"
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
