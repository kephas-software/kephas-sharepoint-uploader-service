﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenizerTest.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the tokenizer test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Core.Tests.Text
{
    using System.Linq;

    using Kephas.SharePoint.Text;
    using NUnit.Framework;

    [TestFixture]
    public class TokenizerTest : SharePointTestBase
    {
        [Test]
        public void Composition()
        {
            var container = this.CreateContainer();
            var tokenizer = container.GetExport<ITokenizer>();

            Assert.IsInstanceOf<Tokenizer>(tokenizer);
        }

        [TestCase("hi there", new[] { "hi" })]
        [TestCase("Please, come home quickly!", new[] { "Please", "come", "home", "quickly" })]
        [TestCase("The \"stereotype\" is 'key'!", new[] { "stereotype", "is", "key" })]
        public void Tokenize_simple(string text, string[] expectedTokens)
        {
            var container = this.CreateContainer();
            var tokenizer = container.GetExport<ITokenizer>();

            var tokens = tokenizer.Tokenize(text).ToArray();

            CollectionAssert.AreEqual(expectedTokens, tokens);
        }
    }
}
