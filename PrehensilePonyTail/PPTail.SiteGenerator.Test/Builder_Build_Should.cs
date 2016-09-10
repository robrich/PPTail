﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PPTail.Interfaces;
using PPTail.Entities;
using TestHelperExtensions;

namespace PPTail.SiteGenerator.Test
{
    public class Builder_Build_Should
    {
        [Fact]
        public void RequestAllPagesFromTheRepository()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            contentRepo.Verify(c => c.GetAllPages(), Times.AtLeastOnce());
        }

        [Fact]
        public void RequestAllPostsFromTheRepository()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            contentRepo.Verify(c => c.GetAllPosts(), Times.AtLeastOnce());
        }

        [Fact]
        public void ReturnOneItemInFolderPagesIfOnePageIsRetrieved()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains("\\Pages\\")));
        }

        [Fact]
        public void ReturnOneItemInFolderPostsIfOnePostIsRetrieved()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains("\\Posts\\")));
        }

        [Fact]
        public void SetTheFilenameOfTheContentPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"\\Pages\\{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, pageGen, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains(expected)));
        }

        [Fact]
        public void SetTheFilenameOfThePostPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"\\Posts\\{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, pageGen, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains(expected)));
        }

        [Fact]
        public void DontCreateAnyFilesIfAllPagesAreUnpublished()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = new List<ContentItem>();
            for (int i = 0; i < 25.GetRandom(10); i++)
            {
                var item = (null as ContentItem).Create();
                item.IsPublished = false;
                contentItems.Add(item);
            }

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void OnlyCreateAsManyFilesAsThereArePublishedPages()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = new List<ContentItem>();
            for (int i = 0; i < 50.GetRandom(25); i++)
            {
                var item = (null as ContentItem).Create();
                item.IsPublished = true.GetRandom();
                contentItems.Add(item);
            }

            var expected = contentItems.Count(ci => ci.IsPublished);

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPage()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = new List<ContentItem>();
            for (int i = 0; i < 50.GetRandom(25); i++)
            {
                var item = (null as ContentItem).Create();
                item.IsPublished = true;
                contentItems.Add(item);
            }

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }


        [Fact]
        public void DontCreateAnyFilesIfAllPostsAreUnpublished()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = new List<ContentItem>();
            for (int i = 0; i < 25.GetRandom(10); i++)
            {
                var item = (null as ContentItem).Create();
                item.IsPublished = false;
                contentItems.Add(item);
            }

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void OnlyCreateAsManyFilesAsThereArePublishedPosts()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = new List<ContentItem>();
            for (int i = 0; i < 50.GetRandom(25); i++)
            {
                var item = (null as ContentItem).Create();
                item.IsPublished = true.GetRandom();
                contentItems.Add(item);
            }

            var expected = contentItems.Count(ci => ci.IsPublished);

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPost()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = new List<ContentItem>();
            for (int i = 0; i < 50.GetRandom(25); i++)
            {
                var item = (null as ContentItem).Create();
                item.IsPublished = true;
                contentItems.Add(item);
            }

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }

    }
}
