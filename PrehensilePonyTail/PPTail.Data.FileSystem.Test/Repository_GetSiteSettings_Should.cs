﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using PPTail.Entities;
using Moq;
using PPTail.Interfaces;
using System.Xml.Linq;
using PPTail.Exceptions;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetSiteSettings_Should
    {
        const int _defaultPostsPerPage = 3;
        const int _defaultPostsPerFeed = 5;
        const string _dataFolder = "App_Data";

        [Fact]
        public void ThrowSettingNotFoundExceptionIfSettingsCannotBeLoaded()
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            string xml = "<badXml></bad>";

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            Assert.Throws<SettingNotFoundException>(() => target.GetSiteSettings());
        }

        [Fact]
        public void ThrowWithProperSettingNameIfSettingsCannotBeLoaded()
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            string xml = "<badXml></bad>";

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            string expected = typeof(SiteSettings).Name;

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            try
            {
                var actual = target.GetSiteSettings();
            }
            catch (SettingNotFoundException ex)
            {
                Assert.Equal(expected, ex.SettingName);
            }
        }

        [Fact]
        public void ReadsTheProperFileFromTheFileSystem()
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            string expectedPath = System.IO.Path.Combine(rootPath, _dataFolder, "settings.xml");

            string xml = new SettingsFileBuilder().UseRandomValues().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.Is<string>(p => p == expectedPath)))
                .Returns(xml).Verifiable();

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetSiteSettings();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void ReturnTheProperValueForTitle()
        {
            string expected = string.Empty.GetRandom();
            string xml = new SettingsFileBuilder().UseRandomValues()
                .Title(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.Title);
        }

        [Fact]
        public void ThrowSettingNotFoundExceptionIfTitleIsNotSupplied()
        {
            var xml = new SettingsFileBuilder()
                .UseRandomValues()
                .RemoveTitle()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            Assert.Throws<SettingNotFoundException>(() => target.GetSiteSettings());
        }

        [Fact]
        public void ThrowWithTheProperSettingNameIfTitleIsNotSupplied()
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}\\";

            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemoveTitle().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            SiteSettings actual;
            string expected = nameof(actual.Title);
            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            try
            {
                actual = target.GetSiteSettings();
            }
            catch (SettingNotFoundException ex)
            {
                Assert.Equal(expected, ex.SettingName);
            }
        }

        [Fact]
        public void ReturnTheProperValueForDescription()
        {
            string expected = string.Empty.GetRandom();
            string xml = new SettingsFileBuilder().UseRandomValues()
                .Description(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.Description);
        }

        [Fact]
        public void ReturnAnEmptyStringIfDescriptionIsNotSupplied()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemoveDescription().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.True(string.IsNullOrWhiteSpace(actual.Description));
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerPage()
        {
            int expected = 25.GetRandom(5);
            string xml = new SettingsFileBuilder().UseRandomValues()
                .PostsPerPage(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.PostsPerPage);
        }

        [Fact]
        public void ReturnTheDefaultValueIfPostsPerPageIsNotSupplied()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemovePostsPerPage().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(_defaultPostsPerPage, actual.PostsPerPage);
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerFeed()
        {
            int expected = 25.GetRandom(5);
            string xml = new SettingsFileBuilder().UseRandomValues()
                .PostsPerFeed(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.PostsPerFeed);
        }

        [Fact]
        public void ReturnTheDefaultValueIfPostsPerFeedIsNotSupplied()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemovePostsPerFeed().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(_defaultPostsPerFeed, actual.PostsPerFeed);
        }

        [Fact]
        public void ReturnTheProperValueForTheme()
        {
            string expected = string.Empty.GetRandom(25);
            var xml = new SettingsFileBuilder().UseRandomValues()
                .Theme(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.Theme);
        }

        [Fact]
        public void ReturnAnEmptyStringIfThemeNodeIsEmpty()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .Theme(string.Empty).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(string.Empty, actual.Theme);
        }

        [Fact]
        public void ReturnAnEmptyStringIfThemeIsNotSupplied()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemoveTheme().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(string.Empty, actual.Theme);
        }

    }
}
