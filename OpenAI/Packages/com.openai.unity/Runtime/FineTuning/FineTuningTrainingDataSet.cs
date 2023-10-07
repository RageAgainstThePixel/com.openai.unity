// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace OpenAI.FineTuning
{
    [CreateAssetMenu(fileName = "FineTuningTrainingDataSet", menuName = "OpenAI/Fine Tuning/new training data set...", order = 99)]
    public sealed class FineTuningTrainingDataSet : ScriptableObject
    {
        private void Awake()
        {
            id = "ft-";
            status = JobStatus.NotStarted;
            fineTuneJob = null;
        }

        [SerializeField]
        [HideInInspector]
        private string id = "ft-";

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
        [Tooltip("When enabled this will use the advanced parameter training options when training.")]
        private bool advanced;

        [Obsolete("removed")]
        public bool Advanced => advanced;

        [SerializeField]
        [Range(1, 4)]
        [Tooltip(
            "The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.")]
        private int epochs = 4;

        public int Epochs
        {
            get => epochs;
            set => epochs = value;
        }

        [SerializeField]
        [Range(1, 256)]
        [Tooltip("The batch size to use for training." +
                 " The batch size is the number of training examples used to train a single forward and backward pass.\r\n\r\n" +
                 "By default, the batch size will be dynamically configured to be ~0.2% of the number of examples in the training set, " +
                 "capped at 256 - in general, we've found that larger batch sizes tend to work better for larger datasets.")]
        private int batchSize = 1;

        [Obsolete("removed")]
        public int BatchSize
        {
            get => batchSize;
            set => batchSize = value;
        }

        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("The learning rate multiplier to use for training. " +
                 "The fine-tuning learning rate is the original learning rate used for pretraining multiplied by this value.\r\n\r\n" +
                 "By default, the learning rate multiplier is the 0.05, 0.1, or 0.2 depending on final batch_size " +
                 "(larger learning rates tend to perform better with larger batch sizes). " +
                 "We recommend experimenting with values in the range 0.02 to 0.2 to see what produces the best results.")]
        private float learningRateMultiplier = 0.2f;

        [Obsolete("removed")]
        public float LearningRateMultiplier
        {
            get => learningRateMultiplier;
            set => learningRateMultiplier = value;
        }

        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("The weight to use for loss on the prompt tokens. " +
                 "This controls how much the model tries to learn to generate the prompt " +
                 "(as compared to the completion which always has a weight of 1.0), " +
                 "and can add a stabilizing effect to training when completions are short.\r\n\r\n" +
                 "If prompts are extremely long (relative to completions), " +
                 "it may make sense to reduce this weight so as to avoid over-prioritizing learning the prompt.")]
        private float promptLossWeight = 0.1f;

        [Obsolete("removed")]
        public float PromptLossWeight
        {
            get => promptLossWeight;
            set => promptLossWeight = value;
        }

        [SerializeField]
        [FormerlySerializedAs("trainingData")]
        private List<FineTuningTrainingData> legacyTrainingData = new List<FineTuningTrainingData>();

        [Obsolete("Use LegacyTrainingData")]
        public IReadOnlyList<FineTuningTrainingData> TrainingData => legacyTrainingData;

        public List<FineTuningTrainingData> LegacyTrainingData
        {
            get => legacyTrainingData;
            set => legacyTrainingData = value;
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

        public IReadOnlyList<string> LegacyTrainingToJsonl()
        {
            var result = new List<string>(LegacyTrainingData.Count);
            result.AddRange(LegacyTrainingData.Select(trainingData => trainingData.ToString()));
            return result;
        }

        public IReadOnlyList<string> ConversationTrainingToJsonl()
        {
            var result = new List<string>(LegacyTrainingData.Count);
            result.AddRange(ConversationTrainingData.Select(trainingData => trainingData.ToString()));
            return result;
        }
    }
}
