﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListItemSourceMetadata.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Sources.Composition
{
    using System.Collections.Generic;

    using Kephas.Services.Composition;
    using Kephas.SharePoint.Sources.AttributedModel;

    /// <summary>
    /// Metadata for list item source.
    /// </summary>
    public class ListItemSourceMetadata : AppServiceMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListItemSourceMetadata"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public ListItemSourceMetadata(IDictionary<string, object?> metadata)
            : base(metadata)
        {
            if (metadata == null)
            {
                return;
            }

            this.RedirectPattern = this.GetMetadataValue<RedirectPatternAttribute, string>(metadata);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItemSourceMetadata" /> class.
        /// </summary>
        /// <param name="redirectPattern">The redirect pattern.</param>
        /// <param name="processingPriority">Optional. The processing priority.</param>
        /// <param name="overridePriority">Optional. The override priority.</param>
        public ListItemSourceMetadata(string? redirectPattern, int processingPriority = 0, int overridePriority = 0)
            : base(processingPriority, overridePriority)
        {
            this.RedirectPattern = redirectPattern;
        }

        /// <summary>
        /// Gets the supported redirect pattern.
        /// </summary>
        public string? RedirectPattern { get; }
    }
}