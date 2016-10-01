﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Output.FileSystem
{
    public class Repository : IOutputRepository
    {
        const string outputPathSettingName = "outputPath";

        IServiceProvider _serviceProvider;
        IFile _file;
        IDirectory _directory;
        Settings _settings;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (_serviceProvider == null)
                throw new Exceptions.DependencyNotFoundException(nameof(IServiceProvider));

            _file = serviceProvider.GetService<IFile>();
            if (_file == null)
                throw new Exceptions.DependencyNotFoundException(nameof(IFile));

            _directory = serviceProvider.GetService<IDirectory>();
            if (_directory == null)
                throw new Exceptions.DependencyNotFoundException(nameof(IDirectory));

            _settings = serviceProvider.GetService<Settings>();
            if (_settings == null)
                throw new Exceptions.DependencyNotFoundException(nameof(Settings));

            if (_settings.ExtendedSettings == null || !_settings.ExtendedSettings.Any(s => s.Item1 == outputPathSettingName))
                throw new Exceptions.SettingNotFoundException(outputPathSettingName);
        }

        public void Save(IEnumerable<SiteFile> files)
        {
            string outputPath = _settings.ExtendedSettings.Single(t => t.Item1 == outputPathSettingName).Item2;

            foreach (var sitePage in files)
            {
                string fullPath = System.IO.Path.Combine(outputPath, sitePage.RelativeFilePath);
                string folderPath = System.IO.Path.GetDirectoryName(fullPath);

                if (!_directory.Exists(folderPath))
                    _directory.CreateDirectory(folderPath);

                if (sitePage.IsBase64Encoded)
                {
                    try
                    {
                        _file.WriteAllBytes(fullPath, Convert.FromBase64String(sitePage.Content));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //TODO: Log the fact that this file was skipped
                    }
                }
                else
                    _file.WriteAllText(fullPath, sitePage.Content);
            }
        }
    }
}
