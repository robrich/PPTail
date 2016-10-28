﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using PPTail.Entities;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Template
{
    public class TemplateProcessor : ITemplateProcessor
    {
        IServiceProvider _serviceProvider;

        public TemplateProcessor(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public string Process(Entities.Template pageTemplate, Entities.Template itemTemplate, string sidebarContent, string navContent, IEnumerable<ContentItem> posts, string pageTitle, string pathToRoot, bool xmlEncodeContent, int maxPostCount)
        {
            _serviceProvider.ValidateService<ISettings>();

            // TODO: Implement
            throw new NotImplementedException();
        }

        private string ProcessTemplate(IEnumerable<ContentItem> posts, IServiceProvider serviceProvider, Entities.Template pageTemplate, Entities.Template itemTemplate, string sidebarContent, string navContent, string pageTitle, string pathToRoot, bool xmlEncodeContent, int maxPostCount)
        {
            // MaxPosts is not pulled from the SiteSettings because
            // there are 2 possible values that might be used to
            // define the value, they are PostsPerPage & PostsPerFeed
            string content = string.Empty;
            var recentPosts = posts.OrderByDescending(p => p.PublicationDate).Where(pub => pub.IsPublished);

            var settings = serviceProvider.GetService<ISettings>();
            string itemSeparator = settings.ItemSeparator;

            if (maxPostCount > 0)
                recentPosts = recentPosts.Take(maxPostCount);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(ProcessContentItemTemplate(itemTemplate, post, sidebarContent, navContent, pathToRoot, xmlEncodeContent));

            var pageContent = string.Join(itemSeparator, contentItems);
            return ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, pageContent, pageTitle);
        }

        public string ProcessContentItemTemplate(Entities.Template template, ContentItem item, string sidebarContent, string navContent, string pathToRoot, bool xmlEncodeContent)
        {
            // TODO: Re-Implement
            throw new NotImplementedException();

            //return template.Content
            //    .ReplaceContentItemVariables(_serviceProvider, item, pathToRoot, xmlEncodeContent)
            //    .ReplaceNonContentItemSpecificVariables(_serviceProvider, sidebarContent, navContent, string.Empty);
        }

        public string ProcessNonContentItemTemplate(Entities.Template template, string sidebarContent, string navContent, string content, string pageTitle)
        {
            // TODO: Re-Implement
            throw new NotImplementedException();

            //return template.Content
            //    .ReplaceNonContentItemSpecificVariables(_serviceProvider, sidebarContent, navContent, content)
            //    .Replace("{Title}", pageTitle);
        }


    }
}
