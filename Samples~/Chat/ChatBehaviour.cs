// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Extensions;

namespace OpenAI.Samples.Chat
{
    public class ChatBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool enableDebug;

        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private RectTransform contentArea;

        [SerializeField]
        private ScrollRect scrollView;

        [SerializeField]
        private AudioSource audioSource;

        private OpenAIClient openAI;

        private readonly List<Message> chatMessages = new List<Message>();

        private CancellationTokenSource lifetimeCancellationTokenSource;

        private void OnValidate()
        {
            inputField.Validate();
            contentArea.Validate();
            submitButton.Validate();
            audioSource.Validate();
        }

        private void Awake()
        {
            OnValidate();
            lifetimeCancellationTokenSource = new CancellationTokenSource();
            openAI = new OpenAIClient();
            chatMessages.Add(new Message(Role.System, "You are a helpful assistant."));
            inputField.onSubmit.AddListener(SubmitChat);
            submitButton.onClick.AddListener(SubmitChat);
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
            var userMessage = new Message(Role.User, inputField.text);
            chatMessages.Add(userMessage);
            var userMessageContent = AddNewTextMessageContent();
            userMessageContent.text = $"User: {inputField.text}";
            inputField.text = string.Empty;
            var assistantMessageContent = AddNewTextMessageContent();
            assistantMessageContent.text = "Assistant: ";

            try
            {
                var request = new ChatRequest(chatMessages, Model.GPT3_5_Turbo);
                openAI.ChatEndpoint.EnableDebug = enableDebug;
                var response = await openAI.ChatEndpoint.StreamCompletionAsync(request, resultHandler: deltaResponse =>
                {
                    if (deltaResponse?.FirstChoice?.Delta == null) { return; }
                    assistantMessageContent.text += deltaResponse.FirstChoice.Delta.ToString();
                    scrollView.verticalNormalizedPosition = 0f;
                }, lifetimeCancellationTokenSource.Token);
                GenerateSpeech(response);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
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
        }

        private async void GenerateSpeech(ChatResponse response)
        {
            try
            {
                var request = new SpeechRequest(response.FirstChoice.ToString(), Model.TTS_1);
                openAI.AudioEndpoint.EnableDebug = enableDebug;
                var (clipPath, clip) = await openAI.AudioEndpoint.CreateSpeechAsync(request, lifetimeCancellationTokenSource.Token);
                audioSource.PlayOneShot(clip);
                Debug.Log(clipPath);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private TextMeshProUGUI AddNewTextMessageContent()
        {
            var textObject = new GameObject($"Message_{contentArea.childCount + 1}");
            textObject.transform.SetParent(contentArea, false);
            var textMesh = textObject.AddComponent<TextMeshProUGUI>();
            textMesh.fontSize = 24;
            textMesh.enableWordWrapping = true;
            return textMesh;
        }
    }
}
