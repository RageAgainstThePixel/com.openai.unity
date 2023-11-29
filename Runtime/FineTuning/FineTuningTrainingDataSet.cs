// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        private string baseModel = Model.GPT3_5_Turbo.ToString();

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
        private bool autoEpochs = true;

        public bool AutoEpochs
        {
            get => autoEpochs;
            set => autoEpochs = value;
        }

        [SerializeField]
        [Tooltip("The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.")]
        private int epochs;

        public int Epochs
        {
            get => epochs;
            set
            {
                epochs = value;
                autoEpochs = epochs > 0;
            }
        }

        [SerializeField]
        private bool autoBatchSize = true;

        public bool AutoBatchSize
        {
            get => autoBatchSize;
            set => autoBatchSize = value;
        }

        [SerializeField]
        private int batchSize;

        public int BatchSize
        {
            get => batchSize;
            set
            {
                batchSize = value;
                autoBatchSize = batchSize > 0;
            }
        }

        [SerializeField]
        private bool autoLearningRateMultiplier = true;

        public bool AutoLearningRateMultiplier
        {
            get => autoLearningRateMultiplier;
            set => autoLearningRateMultiplier = value;
        }

        [SerializeField]
        private int learningRateMultiplier;

        public int LearningRateMultiplier
        {
            get => learningRateMultiplier;
            set
            {
                learningRateMultiplier = value;
                autoLearningRateMultiplier = learningRateMultiplier > 0;
            }
        }

        [SerializeField]
        private List<Conversation> conversationTrainingData = new List<Conversation>();

        public List<Conversation> ConversationTrainingData
        {
            get => conversationTrainingData;
            set => conversationTrainingData = value;
        }

        [NonSerialized]
        private FineTuneJobResponse fineTuneJob;

        internal FineTuneJobResponse FineTuneJob
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
