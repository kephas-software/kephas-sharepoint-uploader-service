﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the default settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// A default settings.
    /// </summary>
    public class DefaultSettings
    {
        /// <summary>
        /// Gets or sets the synchronise identifier field.
        /// </summary>
        /// <value>
        /// The synchronise identifier field.
        /// </value>
        public string SyncIdField { get; set; } = "SyncID";

        /// <summary>
        /// Gets or sets the synchronise tool field.
        /// </summary>
        /// <value>
        /// The synchronise tool field.
        /// </value>
        public string SyncToolField { get; set; } = "SyncTool";

        /// <summary>
        /// Gets or sets the content field.
        /// </summary>
        /// <value>
        /// The content field.
        /// </value>
        public string TextContentField { get; set; } = "TextContent";

        /// <summary>
        /// Gets or sets the original name field.
        /// </summary>
        /// <value>
        /// The original name field.
        /// </value>
        public string OriginalNameField { get; set; } = "OriginalName";

        /// <summary>
        /// Gets or sets the parent identifier field.
        /// </summary>
        /// <value>
        /// The parent identifier field.
        /// </value>
        public string ParentIdField { get; set; } = "ParentID";

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        /// <value>
        /// The library.
        /// </value>
        public string Library { get; set; } = "Unsorted";

        /// <summary>
        /// Gets or sets the maximum size of the file.
        /// </summary>
        /// <value>
        /// The maximum size of the file.
        /// </value>
        public int MaxFileSize { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IDictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();
    }
}