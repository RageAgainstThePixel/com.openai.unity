// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Audio;
using OpenAI.Models;
using OpenAI.Responses;
using System;
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
        private string systemPrompt = "You are a helpful assistant.\n- If an image is requested then use \"![Image](output.jpg)\" to display it.\n- When performing tool calls, tell the user what you're doing before calling, and always use the defaults unless explicitly told to use a specific value.";

        private OpenAIClient openAI;
        private Conversation conversation;

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

        private async void Awake()
        {
            OnValidate();

            openAI = new OpenAIClient(configuration)
            {
                EnableDebug = enableDebug
            };

            RecordingManager.EnableDebug = enableDebug;
            assistantTools.Add(new ImageGenerationTool());
            inputField.onSubmit.AddListener(SubmitChat);
            submitButton.onClick.AddListener(SubmitChat);
            recordButton.onClick.AddListener(ToggleRecording);

            try
            {
                conversation = await openAI.ConversationsEndpoint.CreateConversationAsync(
                    new CreateConversationRequest(new Message(Role.Developer, systemPrompt)),
                    destroyCancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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
            var userInput = inputField.text;
            var userMessageContent = AddNewTextMessageContent(Role.User);
            userMessageContent.text = $"User: {userInput}";
            inputField.text = string.Empty;

            try
            {
                var request = new CreateResponseRequest(textInput: userInput, conversationId: conversation, tools: assistantTools, model: Model.GPT5_Nano);
                async Task StreamEventHandler(string eventName, IServerSentEvent sseEvent)
                {
                    switch (sseEvent)
                    {
                        case Message messageItem:
                            var assistantMessageContent = AddNewTextMessageContent(Role.Assistant);
                            var message = messageItem.ToString().Replace("![Image](output.jpg)", string.Empty);
                            assistantMessageContent.text = $"Assistant: {message}";
                            scrollView.verticalNormalizedPosition = 0f;
                            await GenerateSpeechAsync(message, destroyCancellationToken);
                            break;
                        case ImageGenerationCall imageGenerationCall:
                            if (!string.IsNullOrWhiteSpace(imageGenerationCall.Result))
                            {
                                var image = await imageGenerationCall.LoadTextureAsync(enableDebug, destroyCancellationToken);
                                AddNewImageContent(image);
                                scrollView.verticalNormalizedPosition = 0f;
                            }
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

        private async Task GenerateSpeechAsync(string text, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) { return; }
                var request = new SpeechRequest(input: text, model: Model.TTS_1, voice: voice, responseFormat: SpeechResponseFormat.PCM);
                var stopwatch = Stopwatch.StartNew();
                using var speechClip = await openAI.AudioEndpoint.GetSpeechAsync(
                    request,
                    async partialClip => await streamAudioSource.SampleCallbackAsync(partialClip.AudioSamples),
                    cancellationToken)
                    .ConfigureAwait(true);
                var playbackTime = speechClip.Length - (float)stopwatch.Elapsed.TotalSeconds + 0.1f;

                if (playbackTime > 0)
                {
                    await Awaiters.DelayAsync(playbackTime, cancellationToken).ConfigureAwait(true);
                }

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
