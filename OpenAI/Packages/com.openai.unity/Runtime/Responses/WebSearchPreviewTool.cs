// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Responses
{
    /// <summary>
    /// This tool searches the web for relevant results to use in a response. 
    /// </summary>
    public sealed class WebSearchPreviewTool : ITool
    {
        public static implicit operator Tool(WebSearchPreviewTool webSearchPreviewTool) => new(webSearchPreviewTool as ITool);

        public WebSearchPreviewTool(SearchContextSize searchContextSize = 0, UserLocation userLocation = null)
        {
            SearchContextSize = searchContextSize;
            UserLocation = userLocation;
        }

        [JsonProperty("type")]
        public string Type => "web_search_preview";

        /// <summary>
        /// High level guidance for the amount of context window space to use for the search. One of low, medium, or high. medium is the default.
        /// </summary>
        [JsonProperty("search_context_size")]
        public SearchContextSize SearchContextSize { get; private set; }

        /// <summary>
        /// The user's location.
        /// </summary>
        [JsonProperty("user_location")]
        public UserLocation UserLocation { get; private set; }
    }
}
