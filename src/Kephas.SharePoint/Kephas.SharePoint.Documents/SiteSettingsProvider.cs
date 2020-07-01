﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteSettingsProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the site settings provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Collections.Generic;
    using System.Linq;

    using Kephas.Configuration;

    /// <summary>
    /// A site settings provider.
    /// </summary>
    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private SharePointSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteSettingsProvider"/> class.
        /// </summary>
        /// <param name="sharepointConfiguration">The SharePoint configuration.</param>
        public SiteSettingsProvider(IConfiguration<SharePointSettings> sharepointConfiguration)
        {
            this.settings = sharepointConfiguration.Settings;
        }

        /// <summary>
        /// Gets the site account settings.
        /// </summary>
        /// <returns>
        /// An enumeration of site account name and settings tuples.
        /// </returns>
        public IEnumerable<(string name, SiteAccountSettings settings)> GetAccountSettings()
            => this.settings.Accounts.Select(s => (s.Key, s.Value));

        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>
        /// An enumeration of site name and settings tuples.
        /// </returns>
        public IEnumerable<(string name, SiteSettings settings)> GetSiteSettings()
            => this.settings.Sites.Select(s => (s.Key, s.Value));
    }
}
