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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Prism.Exceptions;
using Prism.Interfaces;
using Prism.Interfaces.Logging;

namespace Prism.Services
{
    /// <summary>
    /// Handles initialization of a set of modules based on types.
    /// </summary>
    public class ModuleLoaderService : IModuleLoaderService
    {
        private readonly IDictionary<string, Type> initializedModules = new Dictionary<string, Type>();
        protected readonly IDictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();

        private readonly IPrismContainer _prismContainer;
        private readonly IPrismLogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="logger"></param>
        public ModuleLoaderService(IPrismContainer container, IPrismLogger logger)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            if (logger == null)
                throw new ArgumentNullException("logger");

            _prismContainer = container;
            _logger = logger;
        }


        public void Initialize(ModuleInfo[] moduleInfoList)
        {
            List<ModuleInfo> modules = GetModulesLoadOrder(moduleInfoList);

            IEnumerable<ModuleInfo> newModules = LoadAssembliesAndTypes(modules);

            foreach (ModuleInfo moduleInfo in newModules)
            {
                Assembly assembly = loadedAssemblies[Path.GetFileName(moduleInfo.AssemblyFile)];

                Type type = initializedModules[moduleInfo.ModuleType];

                try
                {
                    IModule module = (IModule)_prismContainer.Resolve(type);
                    module.Initialize();
                }
                catch (Exception e)
                {
                    HandleModuleLoadError(moduleInfo, type.Assembly.FullName, e);
                }
            }
        }

        private static List<ModuleInfo> GetModulesLoadOrder(IEnumerable<ModuleInfo> moduleInfoEnumerator)
        {
            Dictionary<string, ModuleInfo> indexedInfo = new Dictionary<string, ModuleInfo>();
            ModuleDependencySolver solver = new ModuleDependencySolver();
            List<ModuleInfo> result = new List<ModuleInfo>();

            foreach (ModuleInfo data in moduleInfoEnumerator)
            {
                if (indexedInfo.ContainsKey(data.ModuleName))
                    throw new ModuleLoadException(String.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.DuplicatedModule, data.ModuleName));

                indexedInfo.Add(data.ModuleName, data);
                solver.AddModule(data.ModuleName);

                foreach (string dependency in data.DependsOn)
                    solver.AddDependency(data.ModuleName, dependency);
            }

            if (solver.ModuleCount > 0)
            {
                string[] loadOrder = solver.Solve();

                for (int i = 0; i < loadOrder.Length; i++)
                    result.Add(indexedInfo[loadOrder[i]]);
            }

            return result;
        }

        private IEnumerable<ModuleInfo> LoadAssembliesAndTypes(IEnumerable<ModuleInfo> modules)
        {
            var newModules = new List<ModuleInfo>();

            foreach (ModuleInfo module in modules)
            {
                GuardLegalAssemblyFile(module);

                if (!initializedModules.ContainsKey(module.ModuleType))
                {
                    Assembly assembly = LoadAssembly(module.AssemblyFile);

                    Type type = assembly.GetType(module.ModuleType);

                    initializedModules.Add(module.ModuleType, type);
                    newModules.Add(module);
                }
            }
            return newModules;
        }

        private Assembly LoadAssembly(string assemblyFile)
        {
            if (String.IsNullOrEmpty(assemblyFile))
                throw new ArgumentException("assemblyFile");

            assemblyFile = GetModulePath(assemblyFile);

            FileInfo file = new FileInfo(assemblyFile);
            Assembly assembly;

            if (loadedAssemblies.ContainsKey(file.Name))
                return loadedAssemblies[file.Name];

            try
            {
                assembly = Assembly.LoadFrom(file.FullName);

                loadedAssemblies.Add(file.Name, assembly);
            }
            catch (Exception ex)
            {
                throw new ModuleLoadException(null, assemblyFile, ex.Message, ex);
            }

            return assembly;
        }

        private static string GetModulePath(string assemblyFile)
        {
            if (Path.IsPathRooted(assemblyFile) == false)
                assemblyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFile);

            return assemblyFile;
        }

        private static void GuardLegalAssemblyFile(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");

            if (String.IsNullOrEmpty(moduleInfo.AssemblyFile))
                throw new ArgumentNullException("moduleInfo.AssemblyFile");

            string assemblyFilePath = GetModulePath(moduleInfo.AssemblyFile);

            if (File.Exists(assemblyFilePath) == false)
            {
                throw new ModuleLoadException(
                    string.Format(CultureInfo.CurrentCulture,
                                  Properties.Resources.ModuleNotFound, assemblyFilePath));
            }
        }

        public virtual void HandleModuleLoadError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            Exception moduleException = new ModuleLoadException(moduleInfo.ModuleName, assemblyName, exception.Message,
                                                                exception);

            _logger.Log(moduleException.Message, Category.Exception, Priority.High);

            throw moduleException;
        }
    }
}