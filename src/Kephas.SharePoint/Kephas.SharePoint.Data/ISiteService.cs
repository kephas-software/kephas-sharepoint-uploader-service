﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISiteService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISiteService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;
    using Microsoft.SharePoint.Client;

    using MsListItem = Microsoft.SharePoint.Client.ListItem;

    /// <summary>
    /// Interface for site service.
    /// </summary>
    [AppServiceContract]
    public interface ISiteService
    {
        /// <summary>
        /// Gets the identifier of the site.
        /// </summary>
        /// <value>
        /// The identifier of the site.
        /// </value>
        Guid SiteId { get; }

        /// <summary>
        /// Gets the site name.
        /// </summary>
        /// <value>
        /// The site name.
        /// </value>
        string SiteName { get; }

        /// <summary>
        /// Gets URL of the site.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        Uri SiteUrl { get; }

        /// <summary>
        /// Gets a context for the client.
        /// </summary>
        /// <value>
        /// The client context.
        /// </value>
        ClientContext ClientContext { get; }

        /// <summary>
        /// Gets the list by name asynchronously.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// An asynchronous result that yields the list.
        /// </returns>
        Task<List> GetListAsync(string listName, BaseType listType = BaseType.None);

        /// <summary>
        /// Gets the list by name.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// The list.
        /// </returns>
        List GetList(string listName, BaseType listType = BaseType.None);

        /// <summary>
        /// Gets the list by ID.
        /// </summary>
        /// <param name="listId">The list identity.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// An asynchronous result that yields the list.
        /// </returns>
        Task<List> GetListAsync(Guid listId, BaseType listType = BaseType.None);

        /// <summary>
        /// Gets the list by ID.
        /// </summary>
        /// <param name="listId">The list identity.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// The list.
        /// </returns>
        List GetList(Guid listId, BaseType listType = BaseType.None);

        /// <summary>
        /// Gets the list items asynchronously.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list items.
        /// </returns>
        Task<IEnumerable<MsListItem>> GetListItemsAsync(List list, CamlQuery query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The list items.
        /// </returns>
        IEnumerable<MsListItem> GetListItems(List list, CamlQuery query);
    }
}
