﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITokenizer.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ITokenizer interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Text
{
    using System;
    using System.Collections.Generic;

    using Kephas.Services;

    /// <summary>
    /// Interface for tokenizer.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ITokenizer
    {
        /// <summary>
        /// Splits the provided text into tokens and returns them.
        /// </summary>
        /// <param name="text">The text to be tokenized.</param>
        /// <param name="optionsConfig">Optional. The options configuration.</param>
        /// <returns>
        /// An enumeration of tokens.
        /// </returns>
        public IEnumerable<string> Tokenize(string text, Action<ITokenizerContext> optionsConfig = null);
    }
}
