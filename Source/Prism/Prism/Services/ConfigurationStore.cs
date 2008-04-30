//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Configuration;
using System.IO;
using Prism.Configuration;

namespace Prism.Services
{
    public class ConfigurationStore
    {
        private readonly string _baseDirectory;

        public ConfigurationStore()
            : this(null)
        {
        }

        public ConfigurationStore(string baseDirectory)
        {
            _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseDirectory ?? string.Empty);
        }

        public virtual ModulesConfigurationSection GetModuleConfigurationSection()
        {
            foreach (string fileName in Directory.GetFiles(_baseDirectory, "*.config", SearchOption.TopDirectoryOnly))
            {
                System.Configuration.Configuration configuration =
                    GetConfiguration(Path.Combine(_baseDirectory, fileName));

                ModulesConfigurationSection section = (ModulesConfigurationSection)configuration.GetSection("modules");

                if (section != null)
                {
                    return section;
                }
            }

            return null;
        }

        private static System.Configuration.Configuration GetConfiguration(string configFilePath)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }
    }
}
