﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Entities;
using PPTail.Interfaces;
using System.Text.RegularExpressions;
using PPTail.Exceptions;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;

namespace PPTail.Generator.Syndication.Test
{
    public class SyndicationProvider_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotProvided()
        {
            IServiceProvider serviceProvider = null;
            Assert.Throws<ArgumentNullException>(() => (null as ISyndicationProvider).Create(serviceProvider));
        }

        [Fact]
        public void ReturnTheCorrectArgumentNameIfTheServiceProviderIsNotProvided()
        {
            IServiceProvider serviceProvider = null;

            string actual = string.Empty;
            try
            {
                var target = (null as ISyndicationProvider).Create(serviceProvider);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            Assert.Equal("serviceProvider", actual);
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSiteSettingsAreNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<SiteSettings>();

            Assert.Throws<DependencyNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectDependencyNameIfTheSiteSettingsAreNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<SiteSettings>();

            string actual = string.Empty;
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal("SiteSettings", actual);
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSettingsAreNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            Assert.Throws<DependencyNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectDependencyNameIfTheSettingsAreNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            string actual = string.Empty;
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal("ISettings", actual);
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheSyndicationTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>();
            templates.Add(new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.SyndicationItem });
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            Assert.Throws<TemplateNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectTemplateTypeIfTheSyndicationTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>();
            templates.Add(new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.SyndicationItem });
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            TemplateType actual = TemplateType.Archive; // Anything but Syndication
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(TemplateType.Syndication, actual);
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheSyndicationItemTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>();
            templates.Add(new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.Syndication });
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            Assert.Throws<TemplateNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectTemplateTypeIfTheSyndicationItemTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>();
            templates.Add(new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.Syndication });
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            TemplateType actual = TemplateType.Archive; // Anything but Syndication
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(TemplateType.SyndicationItem, actual);
        }
    }
}
