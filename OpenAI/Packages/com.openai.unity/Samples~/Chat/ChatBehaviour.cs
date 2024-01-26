// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Audio;
using Utilities.Encoding.Wav;
using Utilities.Extensions;
using Utilities.WebRequestRest;

namespace OpenAI.Samples.Chat
{
    public class ChatBehaviour : MonoBehaviour
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
        private AudioSource audioSource;

        [SerializeField]
        [TextArea(3, 10)]
        private string systemPrompt = "You are a helpful assistant.\n- If an image is requested then use \"![Image](output.jpg)\" to display it.";

        private OpenAIClient openAI;

        private readonly Conversation conversation = new();

        private CancellationTokenSource lifetimeCancellationTokenSource;

        private readonly List<Tool> assistantTools = new()
        {
            new Function(
                nameof(GenerateImageAsync),
                "Generates an image based on the user's request.",
                new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["prompt"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "A text description of the desired image(s). The maximum length is 1000 characters for dall-e-2 and 4000 characters for dall-e-3."
                        },
                        ["model"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "The model to use for image generation.",
                            ["enum"] = new JArray { "dall-e-2", "dall-e-3" },
                            ["default"] = "dall-e-2"
                        },
                        ["size"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2. Must be one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3 models.",
                            ["enum"] = new JArray{ "256x256", "512x512", "1024x1024", "1792x1024", "1024x1792" },
                            ["default"] = "512x512"
                        },
                        ["response_format"] = new JObject
                        {
                            ["type"] = "string",
                            ["enum"] = new JArray { "b64_json" } // hard coded for webgl
                        }
                    },
                    ["required"] = new JArray { "prompt", "model", "response_format" }
                })
        };

        private void OnValidate()
        {
            inputField.Validate();
            contentArea.Validate();
            submitButton.Validate();
            recordButton.Validate();
            audioSource.Validate();
        }

        private void Awake()
        {
            OnValidate();
            lifetimeCancellationTokenSource = new CancellationTokenSource();
            openAI = new OpenAIClient(configuration)
            {
                EnableDebug = enableDebug
            };
            conversation.AppendMessage(new Message(Role.System, systemPrompt));
            inputField.onSubmit.AddListener(SubmitChat);
            submitButton.onClick.AddListener(SubmitChat);
            recordButton.onClick.AddListener(ToggleRecording);
        }

        private void OnDestroy()
        {
            lifetimeCancellationTokenSource.Cancel();
            lifetimeCancellationTokenSource.Dispose();
            lifetimeCancellationTokenSource = null;
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
            conversation.AppendMessage(new Message(Role.User, inputField.text));
            var userMessageContent = AddNewTextMessageContent(Role.User);
            userMessageContent.text = $"User: {inputField.text}";
            inputField.text = string.Empty;
            var assistantMessageContent = AddNewTextMessageContent(Role.Assistant);
            assistantMessageContent.text = "Assistant: ";

            try
            {
                var request = new ChatRequest(conversation.Messages, tools: assistantTools, toolChoice: "auto");
                var response = await openAI.ChatEndpoint.StreamCompletionAsync(request, resultHandler: deltaResponse =>
                {
                    if (deltaResponse?.FirstChoice?.Delta == null) { return; }
                    assistantMessageContent.text += deltaResponse.FirstChoice.Delta.ToString();
                    scrollView.verticalNormalizedPosition = 0f;
                }, lifetimeCancellationTokenSource.Token);

                conversation.AppendMessage(response.FirstChoice.Message);

                if (response.FirstChoice.FinishReason == "tool_calls")
                {
                    response = await ProcessToolCallAsync(response);
                    assistantMessageContent.text += response.ToString().Replace("![Image](output.jpg)", string.Empty);
                }

                GenerateSpeech(response);
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
                if (lifetimeCancellationTokenSource is { IsCancellationRequested: false })
                {
                    inputField.interactable = true;
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                    submitButton.interactable = true;
                }

                isChatPending = false;
            }

            async Task<ChatResponse> ProcessToolCallAsync(ChatResponse response)
            {
                var toolCall = response.FirstChoice.Message.ToolCalls.FirstOrDefault();

                if (enableDebug)
                {
                    Debug.Log($"{response.FirstChoice.Message.Role}: {toolCall?.Function?.Name} | Finish Reason: {response.FirstChoice.FinishReason}");
                    Debug.Log($"{toolCall?.Function?.Arguments}");
                }

                if (toolCall == null || toolCall.Function?.Name != nameof(GenerateImageAsync))
                {
                    throw new Exception($"Failed to find a valid tool call!\n{response}");
                }

                ChatResponse toolCallResponse;

                try
                {
                    var imageGenerationRequest = JsonConvert.DeserializeObject<ImageGenerationRequest>(toolCall.Function.Arguments.ToString());
                    var imageResult = await GenerateImageAsync(imageGenerationRequest);
                    AddNewImageContent(imageResult);
                    conversation.AppendMessage(new Message(toolCall, "{\"result\":\"completed\"}"));
                    var toolCallRequest = new ChatRequest(conversation.Messages, tools: assistantTools, toolChoice: "auto");
                    toolCallResponse = await openAI.ChatEndpoint.GetCompletionAsync(toolCallRequest);
                    conversation.AppendMessage(toolCallResponse.FirstChoice.Message);
                }
                catch (RestException restEx)
                {
                    Debug.LogError(restEx);
                    conversation.AppendMessage(new Message(toolCall, restEx.Response.Body));
                    var toolCallRequest = new ChatRequest(conversation.Messages, tools: assistantTools, toolChoice: "auto");
                    toolCallResponse = await openAI.ChatEndpoint.GetCompletionAsync(toolCallRequest);
                    conversation.AppendMessage(toolCallResponse.FirstChoice.Message);
                }

                if (toolCallResponse.FirstChoice.FinishReason == "tool_calls")
                {
                    return await ProcessToolCallAsync(toolCallResponse);
                }

                return toolCallResponse;
            }
        }

        private async void GenerateSpeech(string text)
        {
            text = text.Replace("![Image](output.jpg)", string.Empty);
            var request = new SpeechRequest(text, Model.TTS_1);
            var (clipPath, clip) = await openAI.AudioEndpoint.CreateSpeechAsync(request, lifetimeCancellationTokenSource.Token);
            audioSource.PlayOneShot(clip);

            if (enableDebug)
            {
                Debug.Log(clipPath);
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

        private async Task<ImageResult> GenerateImageAsync(ImageGenerationRequest request)
        {
            var results = await openAI.ImagesEndPoint.GenerateImageAsync(request);
            return results.FirstOrDefault();
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
                var userInput = await openAI.AudioEndpoint.CreateTranscriptionAsync(request, lifetimeCancellationTokenSource.Token);

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
