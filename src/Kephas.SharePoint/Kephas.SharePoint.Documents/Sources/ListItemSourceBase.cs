﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListItemSourceBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the ListItemSourceBase class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Sources
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Configuration;
    using Kephas.ExceptionHandling;
    using Kephas.Interaction;
    using Kephas.Logging;
    using Kephas.Messaging;
    using Kephas.Messaging.Messages;
    using Kephas.Operations;
    using Kephas.Services;
    using Kephas.SharePoint;
    using Kephas.SharePoint.Configuration;
    using Kephas.SharePoint.Messaging;
    using Kephas.Threading.Tasks;
    using Kephas.Workflow;
    using Kephas.Workflow.Interaction;

    /// <summary>
    /// A list item source base.
    /// </summary>
    /// <typeparam name="TSettings">Type of the settings.</typeparam>
    public abstract class ListItemSourceBase<TSettings> : Loggable, IListItemSource
        where TSettings : class, new()
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly IEventHub eventHub;
        private IEventSubscription retrySubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItemSourceBase{TSettings}"/> class.
        /// </summary>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="eventHub">The event hub.</param>
        /// <param name="configuration">The source configuration.</param>
        /// <param name="defaultsProvider">The defaults provider.</param>
        protected ListItemSourceBase(
            IMessageProcessor messageProcessor,
            IEventHub eventHub,
            IConfiguration<TSettings> configuration,
            IDefaultSettingsProvider defaultsProvider)
        {
            this.Settings = configuration.Settings;
            this.Defaults = defaultsProvider.Defaults;
            this.messageProcessor = messageProcessor;
            this.eventHub = eventHub;
        }

        /// <summary>
        /// Gets options for controlling the operation.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        protected TSettings Settings { get; }

        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <value>
        /// The defaults.
        /// </value>
        protected DefaultSettings Defaults { get; }

        /// <summary>
        /// Gets a context for the application.
        /// </summary>
        /// <value>
        /// The application context.
        /// </value>
        protected IContext? AppContext { get; private set; }

        /// <summary>
        /// Initializes the source asynchronously.
        /// </summary>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task InitializeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
            this.AppContext = context;
            this.retrySubscription = this.eventHub.Subscribe<RetryActivitySignal>(
                (e, ctx, token) => e.ActivityContext?[InteractionHelper.SourceArgName] == null
                                    || Equals(this.GetServiceName(), e.ActivityContext[InteractionHelper.SourceArgName])
                                        ? this.RetryUploadFailedItemsAsync(e.ActivityContext, token)
                                        : Task.CompletedTask);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finalize the source asynchronously.
        /// </summary>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task FinalizeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
            this.retrySubscription?.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Uploads the pending items asynchronously.
        /// </summary>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An asynchronous result that yields an operation result indicating whether there is more work to do.
        /// </returns>
        public abstract Task<IOperationResult<bool>> UploadPendingItemsAsync(
            IContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retries to upload the failed items asynchronously.
        /// </summary>
        /// <param name="retryContext">Context for the retry.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task<IOperationResult> RetryUploadFailedItemsAsync(
            IActivityContext? retryContext,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IOperationResult>(
                new OperationResult()
                    .Complete(TimeSpan.Zero, OperationState.NotStarted));
        }

        /// <summary>
        /// Handles the provided item, preparing it for upload
        /// and eventually uploading it.
        /// </summary>
        /// <remarks>
        /// This method is typically called when redirecting from one source to another.
        /// Once a document was redirected, should not be redirected anymore.
        /// </remarks>
        /// <param name="listItem">The list item to handle.</param>
        /// <param name="context">Optional. The handling context.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An asynchronous result that yields an operation result.
        /// </returns>
        public virtual Task<IOperationResult<bool>> HandleItemAsync(
            ListItem listItem,
            IContext? context = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IOperationResult<bool>>(
                new OperationResult<bool>(false)
                    .Complete(TimeSpan.Zero));
        }

        /// <summary>
        /// Gets service name.
        /// </summary>
        /// <returns>
        /// The service name.
        /// </returns>
        protected virtual string? GetServiceName()
        {
            return this.GetType().GetCustomAttribute<ServiceNameAttribute>()?.Value;
        }

        /// <summary>
        /// Uploads a list item asynchronous.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the upload document.
        /// </returns>
        protected virtual async Task<IOperationResult> UploadListItemAsync(ListItem listItem, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = (UploadResponseMessage)(await this.messageProcessor.ProcessAsync(listItem, ctx => ctx.Impersonate(this.AppContext), cancellationToken).PreserveThreadContext())!;
                return response.Result != null
                    ? new OperationResult<UploadResponseMessage>(response)
                        .MergeAll(response.Result)
                        .OperationState(response.Result.OperationState)
                    : new OperationResult<UploadResponseMessage>(response)
                        .MergeMessage(response.Message)
                        .OperationState(
                            response.Severity <= SeverityLevel.Error
                                ? OperationState.Failed
                                : response.Severity == SeverityLevel.Warning
                                    ? OperationState.Warning
                                    : OperationState.Completed);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, $"Error while uploading '{listItem}'");
                return new OperationResult<UploadResponseMessage>(
                        new UploadResponseMessage
                        {
                            Message = ex.Message,
                            Severity = SeverityLevel.Error,
                        })
                    .Fail(ex);
            }
        }

        /// <summary>
        /// Gets the file names for the target and archive.
        /// </summary>
        /// <param name="filePath">Full pathname of the file.</param>
        /// <param name="syncId">Synchronization identifier for the document.</param>
        /// <param name="preserveOriginalName">True to preserve original name.</param>
        /// <returns>
        /// The file names.
        /// </returns>
        protected virtual (string targetFileName, string archiveFileName) GetFileNames(string filePath, long syncId, bool preserveOriginalName)
        {
            var fileName = Path.GetFileName(filePath);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var fileExtension = fileName.Substring(fileNameWithoutExtension.Length);
            var archiveFileName = $"{fileNameWithoutExtension}.{syncId:x}{fileExtension}";

            return (preserveOriginalName ? fileName : archiveFileName, archiveFileName);
        }
    }
}
