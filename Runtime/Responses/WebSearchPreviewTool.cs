// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// This tool searches the web for relevant results to use in a response. 
    /// </summary>
    [Preserve]
    public sealed class WebSearchPreviewTool : ITool
    {
        [Preserve]
        public static implicit operator Tool(WebSearchPreviewTool webSearchPreviewTool) => new(webSearchPreviewTool as ITool);

        [Preserve]
        public WebSearchPreviewTool(SearchContextSize searchContextSize = 0, UserLocation userLocation = null)
        {
            SearchContextSize = searchContextSize;
            UserLocation = userLocation;
        }

        [Preserve]
        [JsonConstructor]
        internal WebSearchPreviewTool(
            [JsonProperty("type")] string type,
            [JsonProperty("search_context_size")] SearchContextSize searchContextSize,
            [JsonProperty("user_location")] UserLocation userLocation)
        {
            Type = type;
            SearchContextSize = searchContextSize;
            UserLocation = userLocation;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "web_search_preview";

        /// <summary>
        /// High level guidance for the amount of context window space to use for the search. One of low, medium, or high. medium is the default.
        /// </summary>
        [Preserve]
        [JsonProperty("search_context_size")]
        public SearchContextSize SearchContextSize { get; }

        /// <summary>
        /// The user's location.
        /// </summary>
        [Preserve]
        [JsonProperty("user_location")]
        public UserLocation UserLocation { get; }
    }
}
