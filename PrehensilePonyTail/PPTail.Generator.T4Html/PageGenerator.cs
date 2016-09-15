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
                throw new Exceptions.DependencyNotFoundException(typeof(Settings));

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            if (!_templates.Any())
                throw new Exceptions.DependencyNotFoundException(typeof(IEnumerable<Template>));
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

        private string DateTimeFormatSpecifier
        {
            get
            {
                return _settings.DateTimeFormatSpecifier;
            }
        }

        public string GenerateContentPage(ContentItem pageData)
        {
            var template = this.ContentPageTemplate;
            if (template == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.ContentPage, string.Empty);

            return pageData.ProcessTemplate(this.ContentPageTemplate.Content, this.DateTimeFormatSpecifier);
        }

        public string GeneratePostPage(ContentItem article)
        {
            //TODO: Verify required templates are available
            throw new NotImplementedException();
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
