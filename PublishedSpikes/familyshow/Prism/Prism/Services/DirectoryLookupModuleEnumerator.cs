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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Prism.Interfaces;
using Prism.Properties;

namespace Prism.Services
{
    public class DirectoryLookupModuleEnumerator : IModuleEnumerator
    {
        private readonly string path;
        private IEnumerable<ModuleInfo> _modules;

        public DirectoryLookupModuleEnumerator(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "path"));

            this.path = path;
        }

        public ModuleInfo[] GetModules()
        {
            EnsureModulesDiscovered();
            return _modules.ToArray();
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            EnsureModulesDiscovered();
            return _modules.Where(moduleInfo => moduleInfo.StartupLoaded == true).ToArray();
        }

        public ModuleInfo[] GetModule(string moduleName)
        {
            EnsureModulesDiscovered();
            return _modules.Where(moduleInfo => moduleInfo.ModuleName == moduleName).ToArray();
        }

        private void EnsureModulesDiscovered()
        {
            if (_modules == null)
            {
                AppDomain childDomain = AppDomain.CreateDomain("DiscoveryRegion");
                try
                {
                    List<string> loadedAssemblies = new List<string>();
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        loadedAssemblies.Add(assembly.Location);
                    }

                    Type loaderType = typeof(InnerModuleInfoLoader);

                    if (loaderType.Assembly != null)
                    {
                        var loader =
                            (InnerModuleInfoLoader)childDomain.CreateInstanceFrom(loaderType.Assembly.Location, loaderType.FullName).Unwrap();
                        loader.LoadAssemblies(loadedAssemblies);
                        _modules = loader.GetModuleInfos(path);
                    }
                }
                finally
                {
                    AppDomain.Unload(childDomain);
                }
            }
        }

        class InnerModuleInfoLoader : MarshalByRefObject
        {
            public ModuleInfo[] GetModuleInfos(string path)
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                Assembly prismInterfacesReflectionOnlyAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().First(asm => asm.FullName == typeof(IModule).Assembly.FullName);
                Type IModuleType = prismInterfacesReflectionOnlyAssembly.GetType(typeof(IModule).FullName);

                var modules = directory.GetFiles("*.dll")
                    .SelectMany(file =>
                                Assembly.ReflectionOnlyLoadFrom(file.FullName)
                                    .GetExportedTypes()
                                    .Where(IModuleType.IsAssignableFrom)
                                    .Where(t => t != IModuleType)
                                    .Select(type => CreateModuleInfo(type)));

                var array = modules.ToArray();
                return array;
            }

            public void LoadAssemblies(IEnumerable<string> assemblies)
            {
                foreach (string assemblyPath in assemblies)
                {
                    Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                }
            }

            static ModuleInfo CreateModuleInfo(Type type)
            {
                string moduleName = type.Name;
                List<string> dependsOn = new List<string>();
                bool startupLoaded = true;
                var attribute = CustomAttributeData.GetCustomAttributes(type).FirstOrDefault(cad => cad.Constructor.DeclaringType.FullName == typeof(ModuleAttribute).FullName);

                if (attribute != null)
                {
                    foreach (CustomAttributeNamedArgument argument in attribute.NamedArguments)
                    {
                        string argumentName = argument.MemberInfo.Name;
                        if (argumentName == "ModuleName")
                            moduleName = (string)argument.TypedValue.Value;
                        else if (argumentName == "DependsOn")
                        {
                            if (argument.TypedValue.Value != null)
                                foreach (CustomAttributeTypedArgument value in ((IEnumerable)argument.TypedValue.Value))
                                {
                                    dependsOn.Add((string)value.Value);
                                }
                        }
                        else if (argumentName == "StartupLoaded")
                            startupLoaded = (bool)argument.TypedValue.Value;
                    }
                }
                return new ModuleInfo(type.Assembly.Location, type.FullName, moduleName, startupLoaded, dependsOn.ToArray());
            }
        }
    }
}
