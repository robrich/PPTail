﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;
using PPTail.Interfaces;

namespace PPTail.Generator.Encoder.Test
{
    public class ContentEncoder_XMLEncode_Should
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("<html/>", "&lt;html/&gt;")]
        [InlineData("“special” quotes", "&quot;special&quot; quotes")]
        [InlineData("\"regular\" quotes", "&quot;regular&quot; quotes")]
        [InlineData("John Doe's possesive", "John Doe&apos;s possesive")]
        [InlineData("Ampersands & Stuff", "Ampersands &amp; Stuff")]
        public void ProperlyEncodeTheData(string source, string expected)
        {
            var container = (null as IServiceCollection).Create();
            var target = new ContentEncoder(container.BuildServiceProvider());
            Assert.Equal(expected, target.XmlEncode(source));
        }
    }
}
