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
        }

        [SerializeField]
        [HideInInspector]
        private string id = "pending";

        public string Id
        {
            get => id;
            internal set => id = value;
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
        private List<FineTuningTrainingData> trainingData = new List<FineTuningTrainingData>();
        public IReadOnlyList<FineTuningTrainingData> TrainingData => trainingData;

        [NonSerialized]
        private FineTuneJob fineTuneJob;

        internal FineTuneJob FineTuneJob
        {
            get => fineTuneJob;
            set
            {
                id = fineTuneJob.Id;
                fineTuneJob = value;
            }
        }
    }
}
