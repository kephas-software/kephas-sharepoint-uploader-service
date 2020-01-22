﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointDataContext.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint data context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Kephas.Composition;
    using Kephas.Data;
    using Kephas.Data.Behaviors;
    using Kephas.Data.Commands.Factory;
    using Kephas.Data.Store;
    using Kephas.Diagnostics.Contracts;
    using Kephas.SharePoint;
    using Kephas.SharePoint.Data.Linq;

    /// <summary>
    /// A SharePoint data context.
    /// </summary>
    [SupportedDataStoreKinds("SharePoint")]
    public class SharePointDataContext : DataContextBase
    {
        private readonly ISiteServiceProvider siteServiceProvider;
        private readonly ILibraryService libraryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDataContext"/> class.
        /// </summary>
        /// <param name="compositionContext">Context for the composition.</param>
        /// <param name="metadataCache">The metadata cache.</param>
        /// <param name="siteServiceProvider">The site service provider.</param>
        /// <param name="libraryService">The library service.</param>
        /// <param name="dataCommandProvider">Optional. the data command provider.</param>
        /// <param name="dataBehaviorProvider">Optional. the data behavior provider.</param>
        public SharePointDataContext(
            ICompositionContext compositionContext,
            ISharePointMetadataCache metadataCache,
            ISiteServiceProvider siteServiceProvider,
            ILibraryService libraryService,
            IDataCommandProvider dataCommandProvider = null,
            IDataBehaviorProvider dataBehaviorProvider = null)
            : base(compositionContext, dataCommandProvider, dataBehaviorProvider)
        {
            this.MetadataCache = metadataCache;
            this.siteServiceProvider = siteServiceProvider;
            this.libraryService = libraryService;
        }

        /// <summary>
        /// Gets the metadata cache.
        /// </summary>
        /// <value>
        /// The metadata cache.
        /// </value>
        public ISharePointMetadataCache MetadataCache { get; }

        /// <summary>
        /// Gets a query over the entity type for the given query operation context, if any is provided
        /// (core implementation).
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="queryOperationContext">Context for the query.</param>
        /// <returns>
        /// A query over the entity type.
        /// </returns>
        protected override IQueryable<T> QueryCore<T>(IQueryOperationContext queryOperationContext)
        {
            var listFullName = queryOperationContext.ListFullName();

            Requires.NotNull(listFullName, nameof(listFullName));

            var (siteName, _) = this.libraryService.GetLibraryPathFragments(listFullName);
            var siteService = string.IsNullOrEmpty(siteName)
                ? this.siteServiceProvider.GetDefaultSiteService()
                : this.siteServiceProvider.GetSiteService(siteName);

            var provider = new SharePointQueryProvider(queryOperationContext, this.libraryService, siteService);
            return provider.CreateQuery<T>(new List<T>().AsQueryable().Expression);
        }

        /// <summary>
        /// Initializes the service asynchronously.
        /// </summary>
        /// <param name="dataInitializationContext">The data initialization context.</param>
        protected override void Initialize(IDataInitializationContext dataInitializationContext)
        {
            base.Initialize(dataInitializationContext);
        }
    }
}
