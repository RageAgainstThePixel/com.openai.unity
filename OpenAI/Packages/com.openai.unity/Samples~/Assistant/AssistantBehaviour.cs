// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Assistants;
using OpenAI.Audio;
using OpenAI.Images;
using OpenAI.Models;
using OpenAI.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace OpenAI.Samples.Assistant
{
    [RequireComponent(typeof(StreamAudioSource))]
    public class AssistantBehaviour : MonoBehaviour
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
        [Obsolete]
        private SpeechVoice voice;

        [SerializeField]
        [TextArea(3, 10)]
        private string systemPrompt = "You are a helpful assistant.\n- If an image is requested then use \"![Image](output.jpg)\" to display it.\n- When performing function calls, use the defaults unless explicitly told to use a specific value.\n- Images should always be generated in base64.";

        private OpenAIClient openAI;
        private ThreadResponse thread;
        private AssistantResponse assistant;

#if !UNITY_2022_3_OR_NEWER
        private readonly CancellationTokenSource lifetimeCts = new();

        // ReSharper disable once InconsistentNaming
        private CancellationToken destroyCancellationToken => lifetimeCts.Token;
#endif

        private void OnValidate()
        {
            inputField.Validate();
            contentArea.Validate();
            submitButton.Validate();
            recordButton.Validate();

            if (streamAudioSource == null)
            {
                streamAudioSource = GetComponent<StreamAudioSource>();
            }
        }

        private async void Awake()
        {
            OnValidate();
            openAI = new OpenAIClient(configuration)
            {
                EnableDebug = enableDebug
            };
            RecordingManager.EnableDebug = enableDebug;

            try
            {
                assistant = await openAI.AssistantsEndpoint.CreateAssistantAsync(
                    new CreateAssistantRequest(
                        model: Model.GPT4o,
                        name: "OpenAI Sample Assistant",
                        description: "An assistant sample example for Unity",
                        instructions: systemPrompt,
                        tools: new List<Tool>
                        {
                            Tool.GetOrCreateTool(openAI.ImagesEndPoint, nameof(ImagesEndpoint.GenerateImageAsync))
                        }),
                        destroyCancellationToken);
                thread = await openAI.ThreadsEndpoint.CreateThreadAsync(new(assistant), destroyCancellationToken);
                inputField.onSubmit.AddListener(SubmitChat);
                submitButton.onClick.AddListener(SubmitChat);
                recordButton.onClick.AddListener(ToggleRecording);

                do
                {
                    await Task.Yield();
                } while (!destroyCancellationToken.IsCancellationRequested);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case TaskCanceledException:
                    case OperationCanceledException:
                    case ObjectDisposedException:
                        // ignored
                        break;
                    default:
                        Debug.LogError(e);
                        break;

                }
            }
            finally
            {
                try
                {
                    if (assistant != null)
                    {
                        var deleteAssistantResult = await assistant.DeleteAsync(deleteToolResources: thread == null, CancellationToken.None);

                        if (!deleteAssistantResult)
                        {
                            Debug.LogError("Failed to delete sample assistant!");
                        }
                    }

                    if (thread != null)
                    {
                        var deleteThreadResult = await thread.DeleteAsync(deleteToolResources: true, CancellationToken.None);

                        if (!deleteThreadResult)
                        {
                            Debug.LogError("Failed to delete sample thread!");
                        }
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
        }

        private void OnDestroy()
        {
#if !UNITY_2022_3_OR_NEWER
            lifetimeCts.Cancel();
#endif
        }

        private void SubmitChat(string _) => SubmitChat();

        private static bool isChatPending;

        private async void SubmitChat()
        {
            if (isChatPending || string.IsNullOrWhiteSpace(inputField.text)) { return; }
            isChatPending = true;

            inputField.ReleaseSelection();
            inputField.interactable = false;
            submitButton.interactable = false;
            recordButton.interactable = false;
            var userMessage = new Message(inputField.text);
            var userMessageContent = AddNewTextMessageContent(Role.User);
            userMessageContent.text = $"User: {inputField.text}";
            inputField.text = string.Empty;
            var assistantMessageContent = AddNewTextMessageContent(Role.Assistant);
            assistantMessageContent.text = "Assistant: ";

            try
            {
                await thread.CreateMessageAsync(userMessage, destroyCancellationToken);

                async Task StreamEventHandler(IServerSentEvent @event)
                {
                    try
                    {
                        switch (@event)
                        {
                            case MessageResponse message:
                                switch (message.Status)
                                {
                                    case MessageStatus.InProgress:
                                        if (message.Role == Role.Assistant)
                                        {
                                            assistantMessageContent.text += message.PrintContent();
                                            scrollView.verticalNormalizedPosition = 0f;
                                        }
                                        break;
                                    case MessageStatus.Completed:
                                        if (message.Role == Role.Assistant)
                                        {
                                            await GenerateSpeechAsync(message.PrintContent(), destroyCancellationToken);
                                            scrollView.verticalNormalizedPosition = 0f;
                                        }
                                        break;
                                }
                                break;
                            case RunResponse run:
                                if (run.Status == RunStatus.RequiresAction)
                                {
                                    await ProcessToolCalls(run);
                                }

                                break;
                            case Error errorResponse:
                                throw errorResponse.Exception ?? new Exception(errorResponse.Message);
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

                var run = await thread.CreateRunAsync(assistant, StreamEventHandler, destroyCancellationToken);
                await run.WaitForStatusChangeAsync(timeout: 60, cancellationToken: destroyCancellationToken);
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
                    submitButton.interactable = true;
                    recordButton.interactable = true;
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                }

                isChatPending = false;
            }

            async Task ProcessToolCalls(RunResponse run)
            {
                Debug.Log(nameof(ProcessToolCalls));
                var toolCalls = run.RequiredAction.SubmitToolOutputs.ToolCalls;
                var toolOutputs = await Task.WhenAll(toolCalls.Select(toolCall => ProcessToolCall(toolCall))).ConfigureAwait(true);
                await run.SubmitToolOutputsAsync(new SubmitToolOutputsRequest(toolOutputs), cancellationToken: destroyCancellationToken);
            }

            async Task<ToolOutput> ProcessToolCall(ToolCall toolCall)
            {
                string result;

                try
                {
                    var imageResults = await assistant.InvokeToolCallAsync<IReadOnlyList<ImageResult>>(toolCall, destroyCancellationToken);

                    foreach (var imageResult in imageResults)
                    {
                        AddNewImageContent(imageResult);
                    }

                    result = "{\"result\":\"completed\"}";
                }
                catch (Exception e)
                {
                    result = $"{{\"result\":\"{e.Message}\"}}";
                }

                return new ToolOutput(toolCall.Id, result);
            }
        }

        private static bool isGeneratingSpeech;

        private async Task GenerateSpeechAsync(string text, CancellationToken cancellationToken)
        {
            if (isGeneratingSpeech)
            {
                throw new InvalidOperationException("Speech generation is already in progress!");
            }

            if (enableDebug)
            {
                Debug.Log($"{nameof(GenerateSpeechAsync)}: {text}");
            }

            isGeneratingSpeech = true;
            try
            {
                text = text.Replace("![Image](output.jpg)", string.Empty);
                if (string.IsNullOrWhiteSpace(text)) { return; }
#pragma warning disable CS0612 // Type or member is obsolete
                var request = new SpeechRequest(text, Model.TTS_1, voice, SpeechResponseFormat.PCM);
#pragma warning restore CS0612 // Type or member is obsolete
                var speechClip = await openAI.AudioEndpoint.GetSpeechAsync(request, partialClip =>
                {
                    streamAudioSource.BufferCallback(partialClip.AudioSamples);
                }, cancellationToken);
                await Awaiters.DelayAsync(TimeSpan.FromSeconds(speechClip.Length), cancellationToken).ConfigureAwait(true);
                ((AudioSource)streamAudioSource).clip = speechClip.AudioClip;

                if (enableDebug)
                {
                    Debug.Log(speechClip.CachePath);
                }
            }
            finally
            {
                isGeneratingSpeech = false;
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
                inputField.interactable = false;
                recordButton.interactable = false;
                submitButton.interactable = false;
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
                if (!isChatPending)
                {
                    inputField.interactable = true;
                    recordButton.interactable = true;
                    submitButton.interactable = true;
                }

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
    }
}
