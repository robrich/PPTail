﻿using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.SiteGenerator
{
    public class Builder
    {
        private readonly IContentRepository _contentRepo;
        private readonly IPageGenerator _pageGen;
        private readonly string _pageFilenameExtension;

        public Builder(IContentRepository contentRepo, IPageGenerator pageGen, string pageFilenameExtension)
        {
            _contentRepo = contentRepo;
            _pageGen = pageGen;
            _pageFilenameExtension = pageFilenameExtension;
        }

        public IEnumerable<SiteFile> Build()
        {
            var result = new List<SiteFile>();

            var settings = _contentRepo.GetSiteSettings();

            // Create Stylesheet page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $".\\Style.css",
                Content = _pageGen.GenerateStylesheet(settings)
            });

            var posts = _contentRepo.GetAllPosts();

            // Create home page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $".\\index.html",
                Content = _pageGen.GenerateHomepage(settings, posts)
            });

            foreach (var post in posts)
            {
                // All all published content pages to the results
                if (post.IsPublished)
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $".\\Posts\\{post.Slug}.{_pageFilenameExtension}",
                        Content = _pageGen.GeneratePostPage(settings, post)
                    });
            }

            var pages = _contentRepo.GetAllPages();
            foreach (var page in pages)
            {
                // All all published content pages to the results
                if (page.IsPublished)
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $".\\Pages\\{page.Slug}.{_pageFilenameExtension}",
                        Content = _pageGen.GenerateContentPage(settings, page)
                    });
            }

            return result;
        }
    }
}
