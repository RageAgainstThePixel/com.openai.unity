// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Audio;
using OpenAI.Images;
using OpenAI.Models;
using OpenAI.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Async;
using Utilities.Audio;
using Utilities.Encoding.Wav;
using Utilities.Extensions;
using Utilities.WebRequestRest.Interfaces;
using Debug = UnityEngine.Debug;
using Message = OpenAI.Responses.Message;

namespace OpenAI.Samples.Responses
{
    [RequireComponent(typeof(StreamAudioSource))]
    public class ResponsesBehaviour : MonoBehaviour
    {
        [SerializeField]
        private OpenAIConfiguration configuration;

        [SerializeField]
        private bool enableDebug;

        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private Button recordButton;

        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private RectTransform contentArea;

        [SerializeField]
        private ScrollRect scrollView;

        [SerializeField]
        private StreamAudioSource streamAudioSource;

        [SerializeField]
        private Voice voice;

        [SerializeField]
        [TextArea(3, 10)]
        private string systemPrompt = "You are a helpful assistant.\n- If an image is requested then use \"![Image](output.jpg)\" to display it.\n- When performing function calls, use the defaults unless explicitly told to use a specific value.\n- Images should always be generated in base64.";

        private OpenAIClient openAI;
        private readonly List<IResponseItem> conversation = new();
        private Tool imageTool;
        private string toolChoice = "auto";
        private readonly List<Tool> assistantTools = new();

#if !UNITY_2022_3_OR_NEWER
        private readonly CancellationTokenSource lifetimeCts = new();

        // ReSharper disable once InconsistentNaming
        private CancellationToken destroyCancellationToken => lifetimeCts.Token;
#endif

        private void OnValidate()
        {
            submitButton.Validate();
            recordButton.Validate();
            inputField.Validate();
            contentArea.Validate();

            if (streamAudioSource == null)
            {
                streamAudioSource = GetComponent<StreamAudioSource>();
            }
        }

        private void Awake()
        {
            OnValidate();
            openAI = new OpenAIClient(configuration)
            {
                EnableDebug = enableDebug
            };
            RecordingManager.EnableDebug = enableDebug;
            imageTool = Tool.GetOrCreateTool(openAI.ImagesEndPoint, nameof(ImagesEndpoint.GenerateImageAsync));
            assistantTools.Add(imageTool);
            conversation.Add(new Message(Role.Developer, systemPrompt));
            inputField.onSubmit.AddListener(SubmitChat);
            submitButton.onClick.AddListener(SubmitChat);
            recordButton.onClick.AddListener(ToggleRecording);
        }

#if !UNITY_2022_3_OR_NEWER
        private void OnDestroy()
        {
            lifetimeCts.Cancel();
        }
#endif

        private void SubmitChat(string _) => SubmitChat();

        private static bool isChatPending;

        private async void SubmitChat()
        {
            if (isChatPending || string.IsNullOrWhiteSpace(inputField.text)) { return; }
            isChatPending = true;

            inputField.ReleaseSelection();
            inputField.interactable = false;
            submitButton.interactable = false;
            conversation.Add(new Message(Role.User, inputField.text));
            var userMessageContent = AddNewTextMessageContent(Role.User);
            userMessageContent.text = $"User: {inputField.text}";
            inputField.text = string.Empty;

            try
            {
                var request = new CreateResponseRequest(input: conversation, tools: assistantTools, toolChoice: toolChoice, model: Model.GPT4_1_Nano);
                async Task StreamEventHandler(string eventName, IServerSentEvent sseEvent)
                {
                    switch (sseEvent)
                    {
                        case Message messageItem:
                            conversation.Add(messageItem);
                            toolChoice = "auto";
                            var assistantMessageContent = AddNewTextMessageContent(Role.Assistant);
                            var message = messageItem.ToString().Replace("![Image](output.jpg)", string.Empty);
                            assistantMessageContent.text = $"Assistant: {message}";
                            scrollView.verticalNormalizedPosition = 0f;
                            await GenerateSpeechAsync(message, destroyCancellationToken);
                            break;
                        case FunctionToolCall functionToolCall:
                            conversation.Add(functionToolCall);
                            toolChoice = "none";
                            var output = await ProcessToolCallAsync(functionToolCall, destroyCancellationToken);
                            conversation.Add(output);
                            break;
                    }
                }

                await openAI.ResponsesEndpoint.CreateModelResponseAsync(request, StreamEventHandler, cancellationToken: destroyCancellationToken);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case TaskCanceledException:
                    case OperationCanceledException:
                        break;
                    default:
                        Debug.LogError(e);
                        break;
                }
            }
            finally
            {
                if (destroyCancellationToken is { IsCancellationRequested: false })
                {
                    inputField.interactable = true;
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                    submitButton.interactable = true;
                }

                isChatPending = false;
            }
        }

        private async Task<FunctionToolCallOutput> ProcessToolCallAsync(FunctionToolCall toolCall, CancellationToken cancellationToken)
        {
            try
            {
                if (toolCall.Name == imageTool.Function.Name)
                {
                    var output = await toolCall.InvokeFunctionAsync<IReadOnlyList<ImageResult>>(cancellationToken: cancellationToken);

                    foreach (var imageResult in output.OutputResult)
                    {
                        AddNewImageContent(imageResult);
                    }

                    scrollView.verticalNormalizedPosition = 0f;
                    return output;
                }

                return await toolCall.InvokeFunctionAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return new FunctionToolCallOutput(toolCall.CallId, JsonConvert.SerializeObject(new { error = new Error(e) }));
            }
        }

        private async Task GenerateSpeechAsync(string text, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) { return; }

                var request = new SpeechRequest(input: text, model: Model.TTS_1, voice: voice, responseFormat: SpeechResponseFormat.PCM);
                var stopwatch = Stopwatch.StartNew();
                var speechClip = await openAI.AudioEndpoint.GetSpeechAsync(request, partialClip =>
                {
                    streamAudioSource.BufferCallback(partialClip.AudioSamples);
                }, cancellationToken);
                var playbackTime = speechClip.Length - (float)stopwatch.Elapsed.TotalSeconds + 0.1f;

                await Awaiters.DelayAsync(TimeSpan.FromSeconds(playbackTime), cancellationToken).ConfigureAwait(true);
                ((AudioSource)streamAudioSource).clip = speechClip.AudioClip;

                if (enableDebug)
                {
                    Debug.Log(speechClip.CachePath);
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case TaskCanceledException:
                    case OperationCanceledException:
                        break;
                    default:
                        Debug.LogError(e);
                        break;
                }
            }
        }

        private TextMeshProUGUI AddNewTextMessageContent(Role role)
        {
            var textObject = new GameObject($"{contentArea.childCount + 1}_{role}");
            textObject.transform.SetParent(contentArea, false);
            var textMesh = textObject.AddComponent<TextMeshProUGUI>();
            textMesh.fontSize = 24;
#if UNITY_2023_1_OR_NEWER
            textMesh.textWrappingMode = TextWrappingModes.Normal;
#else
            textMesh.enableWordWrapping = true;
#endif
            return textMesh;
        }

        private void AddNewImageContent(Texture2D texture)
        {
            var imageObject = new GameObject($"{contentArea.childCount + 1}_Image");
            imageObject.transform.SetParent(contentArea, false);
            var rawImage = imageObject.AddComponent<RawImage>();
            rawImage.texture = texture;
            var layoutElement = imageObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = texture.height / 4f;
            layoutElement.preferredWidth = texture.width / 4f;
            var aspectRatioFitter = imageObject.AddComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            aspectRatioFitter.aspectRatio = texture.width / (float)texture.height;
        }

        private void ToggleRecording()
        {
            RecordingManager.EnableDebug = enableDebug;

            if (RecordingManager.IsRecording)
            {
                RecordingManager.EndRecording();
            }
            else
            {
                inputField.interactable = false;
                // ReSharper disable once MethodSupportsCancellation
                RecordingManager.StartRecording<WavEncoder>(callback: ProcessRecording);
            }
        }

        private async void ProcessRecording(Tuple<string, AudioClip> recording)
        {
            var (path, clip) = recording;

            if (enableDebug)
            {
                Debug.Log(path);
            }

            try
            {
                recordButton.interactable = false;
                var request = new AudioTranscriptionRequest(clip, temperature: 0.1f, language: "en");
                var userInput = await openAI.AudioEndpoint.CreateTranscriptionTextAsync(request, destroyCancellationToken);

                if (enableDebug)
                {
                    Debug.Log(userInput);
                }

                inputField.text = userInput;
                SubmitChat();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                inputField.interactable = true;
            }
            finally
            {
                recordButton.interactable = true;
            }
        }
    }
}
