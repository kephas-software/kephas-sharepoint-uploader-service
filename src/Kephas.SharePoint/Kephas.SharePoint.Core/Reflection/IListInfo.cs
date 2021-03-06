﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListInfo.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IListInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using System.Collections.Generic;

    using Kephas.Data;
    using Kephas.Reflection;

    /// <summary>
    /// Interface for list type information.
    /// </summary>
    public interface IListInfo : ITypeInfo, IIdentifiable
    {
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        new IEnumerable<IListPropertyInfo> Properties { get; }
    }
}
