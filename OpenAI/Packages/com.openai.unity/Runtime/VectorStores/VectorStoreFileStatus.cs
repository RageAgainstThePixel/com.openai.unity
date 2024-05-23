using System.Runtime.Serialization;

namespace OpenAI.VectorStores
{
    public enum VectorStoreFileStatus
    {
        NotStarted = 0,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "failed")]
        Expired
    }
}