﻿using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Enumerations;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;

namespace PPTail.Generator.ContentPage
{
    public class PageGenerator: IContentItemPageGenerator
    {
        IServiceProvider _serviceProvider;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string Generate(string sidebarContent, string navContent, ContentItem pageData, TemplateType templateType, string pathToRoot, bool xmlEncodeContent)
        {
            var templates = _serviceProvider.GetService<IEnumerable<Template>>();
            var template = templates.Find(templateType);
            var categories = _serviceProvider.GetService<IEnumerable<Category>>();
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.ProcessContentItemTemplate(template, pageData, sidebarContent, navContent, pathToRoot, xmlEncodeContent);
        }
    }
}
