using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Extensions;

namespace OpenAI.Demo.Chat
{
    public class ChatDemo : MonoBehaviour
    {
        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private RectTransform contentArea;

        private OpenAIClient openAI;

        private readonly List<Message> chatMessages = new List<Message>();

        private void OnValidate()
        {
            inputField.Validate();
            contentArea.Validate();
            submitButton.Validate();
        }

        private void Awake()
        {
            OnValidate();
            openAI = new OpenAIClient();
            chatMessages.Add(new Message(Role.System, "You are a helpful assistant."));
            inputField.onSubmit.AddListener(SubmitChat);
            submitButton.onClick.AddListener(SubmitChat);
        }

        private void SubmitChat(string _) => SubmitChat();

        private static bool isChatPending;

        private async void SubmitChat()
        {
            if (isChatPending) { return; }
            isChatPending = true;

            inputField.interactable = false;
            submitButton.interactable = false;
            chatMessages.Add(new Message(Role.User, inputField.text));
            var userMessageContent = AddNewTextMessageContent();
            userMessageContent.text = $"User: {inputField.text}";
            inputField.text = string.Empty;

            var assistantMessageContent = AddNewTextMessageContent();
            assistantMessageContent.text = "Assistant: ";

            try
            {
                await openAI.ChatEndpoint.StreamCompletionAsync(
                    new ChatRequest(chatMessages, Model.GPT3_5_Turbo),
                    response =>
                    {
                        if (response.FirstChoice?.Delta != null)
                        {
                            assistantMessageContent.text += response.ToString();
                        }
                    });
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                inputField.interactable = true;
                submitButton.interactable = true;
                isChatPending = false;
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
