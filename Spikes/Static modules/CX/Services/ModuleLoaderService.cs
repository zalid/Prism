using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using System.Reflection;
using CX.Enums;
using System.IO;
using System.Xml;
using System.Configuration;
using CX.Facades;

namespace CX.Services
{
    public class ModuleLoaderService : IModuleLoaderService
    {
        ICXContainerFacade container;

        public ModuleLoaderService(ICXContainerFacade container)
        {
            this.container = container;
        }

        public ModuleMetadata[] LookForModules()
        {
            List<ModuleMetadata> knownModules = new List<ModuleMetadata>();
            string[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.dll");

            foreach (string file in files)
            {
                Assembly assembly = Assembly.LoadFile(file);

                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (typeof(IModule).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                         knownModules.Add(new ModuleMetadata(type.Name, type));
                }
            }

            return knownModules.ToArray(); ;
        }

        public void InitializeModules()
        {
            List<IModule> modules = new List<IModule>();

            //First pass: build modules and register services and views
            foreach (ModuleMetadata moduleMetadata in LookForModules())
            {
                container.Register(typeof(IModule), moduleMetadata.ClassType, moduleMetadata.Name);
                IModule module = container.Resolve<IModule>(moduleMetadata.Name);
                module.RegisterServices();
                module.RegisterViews();
                modules.Add(module);
            }

            //Second pass: initialize module which usually puts views in regions
            foreach (IModule module in modules)
            {
                module.Initialize();
            }
        }
    }
}
