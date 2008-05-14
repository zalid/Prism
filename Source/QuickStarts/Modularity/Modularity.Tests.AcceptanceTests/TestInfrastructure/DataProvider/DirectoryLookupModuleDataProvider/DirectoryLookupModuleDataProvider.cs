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
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Modularity.AcceptanceTests.TestInfrastructure
{
    public class DirectoryLookupModuleDataProvider : DataProviderBase<Module>
    {
        public override string GetDataFilePath()
        {
            // data is not read from a config file in this case
            return String.Empty;
        }

        public override List<Module> GetData()
        {
            List<Module> modules = new List<Module>();
            Module module = null;
            
            string thisAssemblyLocation = Assembly.GetExecutingAssembly().Location;
            //from the assembly location knock-off the assembly file name along with ".dll" extension
            string path = thisAssemblyLocation.Substring(0, thisAssemblyLocation.Length - Assembly.GetExecutingAssembly().GetName().Name.Length - ".dll".Length);
            path += TestDataInfrastructure.GetTestInputData("ModulesFolder");
            
            DirectoryInfo directory = new DirectoryInfo(path);
            string fileName;

            foreach (FileInfo file in directory.GetFiles("*.dll"))
            {
                fileName = file.Name.Substring(0, file.Name.Length - ".dll".Length);
                module = new Module(fileName);

                //reflect and find the value of the StartupLoaded property from the custom attributes
                //assumption: the module file (implementing IModule interface) is named after the module and so is the namespace 
                object customAttribute = Assembly.LoadFrom(file.FullName)
                                            .GetType(fileName + "." + fileName)
                                            .GetCustomAttributes(true)[0];
                Type type = customAttribute.GetType();
                
                object startupLoaded = type.InvokeMember(TestDataInfrastructure.GetTestInputData("StartupLoadingAttributeDirLookup")
                                                        , BindingFlags.GetProperty
                                                        , null
                                                        , customAttribute
                                                        , null);
                module.AllowsDelayLoading = !bool.Parse(startupLoaded.ToString());

                object[] dependsOn = (object[])type.InvokeMember(TestDataInfrastructure.GetTestInputData("DependencyAttributeDirLookup")
                                                                , BindingFlags.GetProperty
                                                                , null
                                                                , customAttribute
                                                                , null);
                if (null != dependsOn)
                {
                    foreach (string dependency in dependsOn)
                    {
                        module.Dependencies.Add(dependency);
                    }
                }

                modules.Add(module);
            }

            return modules;
        }
    }
}
