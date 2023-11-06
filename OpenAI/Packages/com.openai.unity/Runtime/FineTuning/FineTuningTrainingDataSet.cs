﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OpenAI.FineTuning
{
    [CreateAssetMenu(fileName = "FineTuningTrainingDataSet", menuName = "OpenAI/Fine Tuning/new training data set...", order = 99)]
    public sealed class FineTuningTrainingDataSet : ScriptableObject
    {
        private void Awake()
        {
            id = "ftjob-";
            status = JobStatus.NotStarted;
            fineTuneJob = null;
        }

        [SerializeField]
        [HideInInspector]
        private string id = "ftjob-";

        public string Id => id;

        [SerializeField]
        [HideInInspector]
        private JobStatus status = JobStatus.NotStarted;

        public JobStatus Status
        {
            get => status;
            internal set => status = value;
        }

        [SerializeField]
        [HideInInspector]
        private string baseModel = "gpt-3.5-turbo";

        public Model BaseModel
        {
            get => baseModel;
            set => baseModel = value;
        }

        [SerializeField]
        [HideInInspector]
        [Tooltip("You can add a suffix of up to 40 characters to your fine-tuned model name.")]
        private string modelSuffix;

        public string ModelSuffix
        {
            get => modelSuffix;
            set => modelSuffix = value;
        }

        [SerializeField]
        [Range(1, 4)]
        [Tooltip("The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.")]
        private int epochs = 4;

        public int Epochs
        {
            get => epochs;
            set => epochs = value;
        }

        [SerializeField]
        private List<Conversation> conversationTrainingData = new List<Conversation>();

        public List<Conversation> ConversationTrainingData
        {
            get => conversationTrainingData;
            set => conversationTrainingData = value;
        }

        [NonSerialized]
        private FineTuneJob fineTuneJob;

        internal FineTuneJob FineTuneJob
        {
            get => fineTuneJob;
            set
            {
                id = value.Id;
                status = value.Status;
                fineTuneJob = value;

            }
        }

        public IEnumerable<string> ConversationTrainingToJsonl()
        {
            var result = new List<string>(ConversationTrainingData.Count);
            result.AddRange(ConversationTrainingData.Select(trainingData => trainingData.ToString()));
            return result;
        }
    }
}
