// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Files;
using OpenAI.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Assistants
{
    public static class AssistantExtensions
    {
        /// <summary>
        /// Modify the assistant.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="request"><see cref="CreateAssistantRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantResponse"/>.</returns>
        public static async Task<AssistantResponse> ModifyAsync(this AssistantResponse assistant, CreateAssistantRequest request, CancellationToken cancellationToken = default)
            => await assistant.Client.AssistantsEndpoint.ModifyAssistantAsync(
                assistantId: assistant.Id,
                request: request ?? new CreateAssistantRequest(assistant),
                cancellationToken: cancellationToken);

        /// <summary>
        /// Get the latest status of the assistant.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantResponse"/>.</returns>
        public static async Task<AssistantResponse> UpdateAsync(this AssistantResponse assistant, CancellationToken cancellationToken = default)
            => await assistant.Client.AssistantsEndpoint.RetrieveAssistantAsync(assistant, cancellationToken);

        /// <summary>
        /// Delete the assistant.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="deleteToolResources">Optional, should tool resources, such as vector stores be deleted when this assistant is deleted?</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the <see cref="assistant"/> was successfully deleted.</returns>
        public static async Task<bool> DeleteAsync(this AssistantResponse assistant, bool deleteToolResources = false, CancellationToken cancellationToken = default)
        {
            if (deleteToolResources)
            {
                assistant = await assistant.UpdateAsync(cancellationToken);
            }

            var deleteTasks = new List<Task<bool>> { assistant.Client.AssistantsEndpoint.DeleteAssistantAsync(assistant.Id, cancellationToken) };

            if (deleteToolResources && assistant.ToolResources?.FileSearch?.VectorStoreIds is { Count: > 0 })
            {
                deleteTasks.AddRange(
                    from vectorStoreId in assistant.ToolResources?.FileSearch?.VectorStoreIds
                    where !string.IsNullOrWhiteSpace(vectorStoreId)
                    select assistant.Client.VectorStoresEndpoint.DeleteVectorStoreAsync(vectorStoreId, cancellationToken));
            }

            await Task.WhenAll(deleteTasks);
            return deleteTasks.TrueForAll(task => task.Result);
        }

        [Obsolete("use new overload with Func<IServerSentEvent, Task> instead.")]
        public static async Task<RunResponse> CreateThreadAndRunAsync(this AssistantResponse assistant, CreateThreadRequest request, Action<IServerSentEvent> streamEventHandler, CancellationToken cancellationToken = default)
            => await CreateThreadAndRunAsync(assistant, request, streamEventHandler == null ? null : async serverSentEvent =>
            {
                streamEventHandler.Invoke(serverSentEvent);
                await Task.CompletedTask;
            }, cancellationToken);

        /// <summary>
        /// Create a thread and run it.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="request">Optional, <see cref="CreateThreadRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public static async Task<RunResponse> CreateThreadAndRunAsync(this AssistantResponse assistant, CreateThreadRequest request = null, Func<IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
        {
            var threadRunRequest = new CreateThreadAndRunRequest(
                assistant.Id,
                assistant.Model,
                assistant.Instructions,
                assistant.Tools,
                assistant.ToolResources,
                assistant.Metadata,
                assistant.Temperature,
                assistant.TopP,
                jsonSchema: assistant.ResponseFormatObject?.JsonSchema,
                responseFormat: assistant.ResponseFormat,
                createThreadRequest: request);
            return await assistant.Client.ThreadsEndpoint.CreateThreadAndRunAsync(threadRunRequest, streamEventHandler, cancellationToken);
        }

        #region Tools

        /// <summary>
        /// Invoke the assistant's tool function using the <see cref="ToolCall"/>.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="toolCall"><see cref="ToolCall"/>.</param>
        /// <returns>Tool output result as <see cref="string"/>.</returns>
        /// <remarks>Only call this directly on your <see cref="ToolCall"/> if you know the method is synchronous.</remarks>
        public static string InvokeToolCall(this AssistantResponse assistant, ToolCall toolCall)
        {
            if (!toolCall.IsFunction)
            {
                throw new InvalidOperationException($"Cannot invoke built in tool {toolCall.Type}");
            }

            var tool = assistant.Tools.FirstOrDefault(tool => tool.IsFunction && tool.Function.Name == toolCall.FunctionCall.Name) ??
                throw new InvalidOperationException($"Failed to find a valid tool for [{toolCall.Id}] {toolCall.FunctionCall.Name}");
            tool.Function.Arguments = toolCall.FunctionCall.Arguments;
            return tool.InvokeFunction();
        }

        /// <summary>
        /// Invoke the assistant's tool function using the <see cref="ToolCall"/>.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="toolCall"><see cref="ToolCall"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>Tool output result as <see cref="string"/>.</returns>
        public static async Task<string> InvokeToolCallAsync(this AssistantResponse assistant, ToolCall toolCall, CancellationToken cancellationToken = default)
        {
            if (!toolCall.IsFunction)
            {
                throw new InvalidOperationException($"Cannot invoke built in tool {toolCall.Type}");
            }

            var tool = assistant.Tools.FirstOrDefault(tool => tool.Type == "function" && tool.Function.Name == toolCall.FunctionCall.Name) ??
                throw new InvalidOperationException($"Failed to find a valid tool for [{toolCall.Id}] {toolCall.FunctionCall.Name}");
            tool.Function.Arguments = toolCall.FunctionCall.Arguments;
            return await tool.InvokeFunctionAsync(cancellationToken);
        }

        /// <summary>
        /// Calls the tool's function, with the provided arguments from the toolCall and returns the output.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="toolCall"><see cref="ToolCall"/>.</param>
        /// <returns><see cref="ToolOutput"/>.</returns>
        /// <remarks>Only call this directly on your <see cref="ToolCall"/> if you know the method is synchronous.</remarks>
        public static ToolOutput GetToolOutput(this AssistantResponse assistant, ToolCall toolCall)
            => new(toolCall.Id, assistant.InvokeToolCall(toolCall));

        /// <summary>
        /// Calls each tool's function, with the provided arguments from the toolCalls and returns the outputs.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="toolCalls">A collection of <see cref="ToolCall"/>s.</param>
        /// <returns>A collection of <see cref="ToolOutput"/>s.</returns>
        [Obsolete("Use GetToolOutputsAsync instead.")]
        public static IReadOnlyList<ToolOutput> GetToolOutputs(this AssistantResponse assistant, IEnumerable<ToolCall> toolCalls)
            => toolCalls.Select(assistant.GetToolOutput).ToList();

        /// <summary>
        /// Calls the tool's function, with the provided arguments from the toolCall and returns the output.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="toolCall"><see cref="ToolCall"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ToolOutput"/>.</returns>
        public static async Task<ToolOutput> GetToolOutputAsync(this AssistantResponse assistant, ToolCall toolCall, CancellationToken cancellationToken = default)
        {
            var output = await assistant.InvokeToolCallAsync(toolCall, cancellationToken);
            return new ToolOutput(toolCall.Id, output);
        }

        /// <summary>
        /// Calls each tool's function, with the provided arguments from the toolCalls and returns the outputs.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="toolCalls">A collection of <see cref="ToolCall"/>s.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A collection of <see cref="ToolOutput"/>s.</returns>
        public static async Task<IReadOnlyList<ToolOutput>> GetToolOutputsAsync(this AssistantResponse assistant, IEnumerable<ToolCall> toolCalls, CancellationToken cancellationToken = default)
            => await Task.WhenAll(toolCalls.Select(async toolCall => await assistant.GetToolOutputAsync(toolCall, cancellationToken))).ConfigureAwait(true);

        /// <summary>
        /// Calls each tool's function, with the provided arguments from the toolCalls and returns the outputs.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="run">The <see cref="RunResponse"/> to complete the tool calls for.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A collection of <see cref="ToolOutput"/>s.</returns>
        public static async Task<IReadOnlyList<ToolOutput>> GetToolOutputsAsync(this AssistantResponse assistant, RunResponse run, CancellationToken cancellationToken = default)
            => await GetToolOutputsAsync(assistant, run.RequiredAction.SubmitToolOutputs.ToolCalls, cancellationToken);

        #endregion Tools

        #region Files (Obsolete)

        /// <summary>
        /// Returns a list of assistant files.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{AssistantFile}"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<ListResponse<AssistantFileResponse>> ListFilesAsync(this AssistantResponse assistant, ListQuery query = null, CancellationToken cancellationToken = default)
            => await assistant.Client.AssistantsEndpoint.ListFilesAsync(assistant.Id, query, cancellationToken);

        /// <summary>
        /// Attach a file to the  <see cref="assistant"/>.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="file">
        /// A <see cref="FileResponse"/> (with purpose="assistants") that the assistant should use.
        /// Useful for tools like retrieval and code_interpreter that can access files.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<AssistantFileResponse> AttachFileAsync(this AssistantResponse assistant, FileResponse file, CancellationToken cancellationToken = default)
            => await assistant.Client.AssistantsEndpoint.AttachFileAsync(assistant.Id, file, cancellationToken);

        /// <summary>
        /// Uploads a new file at the specified <see cref="filePath"/> and attaches it to the <see cref="assistant"/>.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="filePath">The local file path to upload.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<AssistantFileResponse> UploadFileAsync(this AssistantResponse assistant, string filePath, CancellationToken cancellationToken = default)
        {
            var file = await assistant.Client.FilesEndpoint.UploadFileAsync(new FileUploadRequest(filePath, FilePurpose.Assistants), uploadProgress: null, cancellationToken);
            return await assistant.AttachFileAsync(file, cancellationToken);
        }

        /// <summary>
        /// Uploads a new file at the specified path and attaches it to the assistant.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="stream">The file contents to upload.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<AssistantFileResponse> UploadFileAsync(this AssistantResponse assistant, Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            var file = await assistant.Client.FilesEndpoint.UploadFileAsync(new FileUploadRequest(stream, fileName, FilePurpose.Assistants), uploadProgress: null, cancellationToken);
            return await assistant.AttachFileAsync(file, cancellationToken);
        }

        /// <summary>
        /// Retrieves the <see cref="AssistantFileResponse"/>.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="fileId">The ID of the file we're getting.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<AssistantFileResponse> RetrieveFileAsync(this AssistantResponse assistant, string fileId, CancellationToken cancellationToken = default)
            => await assistant.Client.AssistantsEndpoint.RetrieveFileAsync(assistant.Id, fileId, cancellationToken);

        /// <summary>
        /// Remove the file from the assistant it is attached to.
        /// </summary>
        /// <remarks>
        /// Note that removing an AssistantFile does not delete the original File object,
        /// it simply removes the association between that File and the Assistant.
        /// To delete a File, use <see cref="DeleteFileAsync(AssistantFileResponse,CancellationToken)"/>.
        /// </remarks>
        /// <param name="file"><see cref="AssistantResponse"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if file was removed.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<bool> RemoveFileAsync(this AssistantFileResponse file, CancellationToken cancellationToken = default)
            => await file.Client.AssistantsEndpoint.RemoveFileAsync(file.AssistantId, file.Id, cancellationToken);

        /// <summary>
        /// Remove the file from the assistant it is attached to.
        /// </summary>
        /// <remarks>
        /// Note that removing an AssistantFile does not delete the original File object,
        /// it simply removes the association between that File and the Assistant.
        /// To delete a File, use <see cref="DeleteFileAsync(AssistantFileResponse,CancellationToken)"/>.
        /// </remarks>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="fileId">The ID of the file to remove.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if file was removed.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<bool> RemoveFileAsync(this AssistantResponse assistant, string fileId, CancellationToken cancellationToken = default)
            => await assistant.Client.AssistantsEndpoint.RemoveFileAsync(assistant.Id, fileId, cancellationToken);

        /// <summary>
        /// Removes and Deletes a file from the assistant.
        /// </summary>
        /// <param name="file"><see cref="AssistantResponse"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the file was successfully removed from the assistant and deleted.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<bool> DeleteFileAsync(this AssistantFileResponse file, CancellationToken cancellationToken = default)
        {
            var isRemoved = await file.RemoveFileAsync(cancellationToken);
            return isRemoved && await file.Client.FilesEndpoint.DeleteFileAsync(file.Id, cancellationToken);
        }

        /// <summary>
        /// Removes and Deletes a file from the <see cref="assistant"/>.
        /// </summary>
        /// <param name="assistant"><see cref="AssistantResponse"/>.</param>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the file was successfully removed from the assistant and deleted.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public static async Task<bool> DeleteFileAsync(this AssistantResponse assistant, string fileId, CancellationToken cancellationToken = default)
        {
            var isRemoved = await assistant.Client.AssistantsEndpoint.RemoveFileAsync(assistant.Id, fileId, cancellationToken);
            if (!isRemoved) { return false; }
            return await assistant.Client.FilesEndpoint.DeleteFileAsync(fileId, cancellationToken);
        }

        #endregion Files (Obsolete)
    }
}
