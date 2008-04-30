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
using Prism.Interfaces;
using StockTraderRI.Modules.Market;
using StockTraderRI.Modules.News;
using StockTraderRI.Modules.Position;
using StockTraderRI.Modules.Watch;

namespace StockTraderRI
{
    public class StockTraderRIModuleEnumerator : IModuleEnumerator
    {
        public ModuleInfo[] GetModules()
        {
            return GetModuleList().ToArray();
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            return GetModuleList().ToArray();
        }

        public ModuleInfo[] GetModule(string moduleName)
        {
            throw new NotImplementedException();
        }

        private static List<ModuleInfo> GetModuleList()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>();

            modules.Add(GetModuleInfo(typeof(NewsModule)));
            modules.Add(GetModuleInfo(typeof(MarketModule)));
            modules.Add(GetModuleInfo(typeof(WatchModule), new[] { "MarketModule" }));
            modules.Add(GetModuleInfo(typeof(PositionModule), new[] { "MarketModule", "NewsModule" }));
            return modules;
        }

        private static ModuleInfo GetModuleInfo(Type moduleType, params String[] dependsOn)
        {
            return new ModuleInfo(moduleType.Assembly.Location
                                  , moduleType.FullName, moduleType.Name, dependsOn);
        }

    }
}
