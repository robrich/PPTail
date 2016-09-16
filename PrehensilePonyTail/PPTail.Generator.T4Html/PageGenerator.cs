﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;

namespace PPTail.Generator.T4Html
{
    public class PageGenerator : Interfaces.IPageGenerator
    {
        // private string _contentPageTemplate;
        // private string _dateTimeFormatSpecifier;

        private readonly IServiceProvider _serviceProvider;
        private readonly Settings _settings;
        private readonly IEnumerable<Template> _templates;

        public PageGenerator(IServiceCollection container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            _serviceProvider = container.BuildServiceProvider();
            _settings = _serviceProvider.GetService<Settings>();
            if (_settings == null)
                throw new Exceptions.DependencyNotFoundException(nameof(Settings));

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            if (!_templates.Any())
                throw new Exceptions.DependencyNotFoundException("IEnumerable<Template>");
        }

        private Template ContentPageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.ContentPage);
            }
        }

        private Template PostPageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.PostPage);
            }
        }

        private Template HomePageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.HomePage);
            }
        }

        private Template ItemTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.Item);
            }
        }

        private Template StyleTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.Style);
            }
        }

        private string DateTimeFormatSpecifier
        {
            get
            {
                return _settings.DateTimeFormatSpecifier;
            }
        }

        private string ItemSeparator
        {
            get
            {
                return _settings.ItemSeparator;
            }
        }

        public string GenerateHomepage(SiteSettings siteSettings, IEnumerable<ContentItem> posts)
        {
            return posts.ProcessTemplate(siteSettings, this.HomePageTemplate.Content, this.ItemTemplate.Content, this.DateTimeFormatSpecifier, this.ItemSeparator);
        }

        public string GenerateStylesheet(SiteSettings siteSettings)
        {
            //TODO: Process template against additional data (such as Settings and SiteSettings)
            return this.StyleTemplate.Content;
        }

        public string GenerateContentPage(SiteSettings siteSettings, ContentItem pageData)
        {
            var template = this.ContentPageTemplate;
            if (template == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.ContentPage, string.Empty);

            return pageData.ProcessTemplate(siteSettings, template.Content, this.DateTimeFormatSpecifier);
        }

        public string GeneratePostPage(SiteSettings siteSettings, ContentItem article)
        {
            var template = this.PostPageTemplate;
            if (template == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.PostPage, string.Empty);

            return article.ProcessTemplate(siteSettings, template.Content, this.DateTimeFormatSpecifier);
        }

        //public void LoadContentPageTemplate(string path)
        //{
        //    throw new NotImplementedException();
        //    // _contentPageTemplate = System.IO.File.ReadAllText(path);
        //}

        //public void LoadPostPageTemplate(string path)
        //{
        //    throw new NotImplementedException();
        //    // _postPageTemplate = System.IO.File.ReadAllText(path);
        //}
    }
}
