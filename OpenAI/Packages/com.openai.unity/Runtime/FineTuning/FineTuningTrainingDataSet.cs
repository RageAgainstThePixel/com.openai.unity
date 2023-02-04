// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI.FineTuning
{
    [CreateAssetMenu(fileName = "FineTuningTrainingDataSet", menuName = "OpenAI/Fine Tuning/new training data set...", order = 99)]
    public class FineTuningTrainingDataSet : ScriptableObject
    {
        private void Awake()
        {
            if (trainingData.Count == 0)
            {
                trainingData.Add(new FineTuningTrainingData("<prompt text>", "<ideal generated text>"));
            }

            id = "pending";
            status = "not started";
            fineTuneJob = null;
        }

        [SerializeField]
        [HideInInspector]
        private string id = "pending";

        public string Id => id;

        [SerializeField]
        [HideInInspector]
        private string status = "not started";

        public string Status
        {
            get => status;
            internal set => status = value;
        }

        [SerializeField]
        [HideInInspector]
        private string baseModel = Models.Model.Curie;

        public string BaseModel => baseModel;

        [SerializeField]
        [HideInInspector]
        [Tooltip("You can add a suffix of up to 40 characters to your fine-tuned model name.")]
        private string modelSuffix;

        public string ModelSuffix => modelSuffix;

        [SerializeField]
        [Tooltip("When enabled this will use the advanced parameter training options when training.")]
        private bool advanced;

        public bool Advanced => advanced;

        [SerializeField]
        [Range(1, 4)]
        [Tooltip("The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.")]
        private int epochs = 4;

        public int Epochs => epochs;

        [SerializeField]
        [Range(1, 256)]
        [Tooltip("The batch size to use for training." +
                 " The batch size is the number of training examples used to train a single forward and backward pass.\r\n\r\n" +
                 "By default, the batch size will be dynamically configured to be ~0.2% of the number of examples in the training set, " +
                 "capped at 256 - in general, we've found that larger batch sizes tend to work better for larger datasets.")]
        private int batchSize = 1;

        public int BatchSize => batchSize;

        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("The learning rate multiplier to use for training. " +
                 "The fine-tuning learning rate is the original learning rate used for pretraining multiplied by this value.\r\n\r\n" +
                 "By default, the learning rate multiplier is the 0.05, 0.1, or 0.2 depending on final batch_size " +
                 "(larger learning rates tend to perform better with larger batch sizes). " +
                 "We recommend experimenting with values in the range 0.02 to 0.2 to see what produces the best results.")]
        private float learningRateMultiplier = 0.2f;

        public float LearningRateMultiplier => learningRateMultiplier;

        [SerializeField]
        [Range(0.01f, 1f)]
        [Tooltip("The weight to use for loss on the prompt tokens. " +
                 "This controls how much the model tries to learn to generate the prompt " +
                 "(as compared to the completion which always has a weight of 1.0), " +
                 "and can add a stabilizing effect to training when completions are short.\r\n\r\n" +
                 "If prompts are extremely long (relative to completions), " +
                 "it may make sense to reduce this weight so as to avoid over-prioritizing learning the prompt.")]
        private float promptLossWeight = 0.1f;

        public float PromptLossWeight => promptLossWeight;

        [SerializeField]
        private List<FineTuningTrainingData> trainingData = new List<FineTuningTrainingData>();

        public IReadOnlyList<FineTuningTrainingData> TrainingData => trainingData;

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
    }
}
