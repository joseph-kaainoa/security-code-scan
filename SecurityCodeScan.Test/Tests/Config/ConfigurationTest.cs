﻿using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Moq;
using SecurityCodeScan.Test.Helpers;

namespace SecurityCodeScan.Test.Config
{
    public abstract class ConfigurationTest : DiagnosticVerifier
    {
        private const string ConfigName = "SecurityCodeScan.config.yml";
        private static readonly Version ConfigVersion = new Version(1,0);
        private const string ConfigText = "Version: {0}\r\n{1}";

        protected static AnalyzerOptions CreateAnalyzersOptionsWithConfig(string  configSource,
                                                                          Version version = null)
        {
            var configText = string.Format(ConfigText, version != null ? version : ConfigVersion, configSource);

            var additionalTextMock = new Mock<AdditionalText>();
            additionalTextMock.Setup(text => text.GetText(CancellationToken.None)).Returns(SourceText.From(configText));
            // doesn't have to be real path, but config manager caches content, so must be unique for each test
            var path = Path.Combine(Guid.NewGuid().ToString(), ConfigName);
            additionalTextMock.Setup(text => text.Path).Returns(path);

            return new AnalyzerOptions(ImmutableArray.Create(additionalTextMock.Object));
        }
    }
}
