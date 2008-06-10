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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.UnityExtensions.Properties;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.Composite.UnityExtensions
{
    public abstract class UnityBootstrapper
    {
        private readonly ILoggerFacade _loggerFacade = new TraceLogger();
        private bool _useDefaultConfiguration = true;

        /// <summary>
        /// Returns the default <see cref="IUnityContainer"/> for the application.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Returns the default <see cref="ILoggerFacade"/> for the application.
        /// </summary>
        protected virtual ILoggerFacade LoggerFacade
        {
            get { return _loggerFacade; }
        }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        public void Run()
        {
            Run(true);
        }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="useDefaultConfiguration">If <see langword="true"/>, registers default CAL services in the container. This is the default behavior.</param>
        public void Run(bool useDefaultConfiguration)
        {
            _useDefaultConfiguration = useDefaultConfiguration;
            ILoggerFacade logger = LoggerFacade;
            if (logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }

            logger.Log("Creating Unity container", Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException(Resources.NullUnityContainerException);
            }

            logger.Log("Configuring container", Category.Debug, Priority.Low);

            ConfigureContainer();

            logger.Log("Configuring region adapters", Category.Debug, Priority.Low);

            ConfigureRegionAdapterMappings();

            logger.Log("Creating shell", Category.Debug, Priority.Low);
            DependencyObject shell = CreateShell();

            if (shell != null)
            {
                RegionManager.SetRegionManager(shell, Container.Resolve<IRegionManager>());
            }

            logger.Log("Initializing modules", Category.Debug, Priority.Low);
            InitializeModules();

            logger.Log("Bootstrapper sequence completed", Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configure the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.RegisterInstance<ILoggerFacade>(LoggerFacade);
            Container.RegisterInstance<IUnityContainer>(Container);
            Container.AddNewExtension<UnityBootstrapperExtension>();

            IModuleEnumerator moduleEnumerator = GetModuleEnumerator();
            if (moduleEnumerator != null)
            {
                Container.RegisterInstance<IModuleEnumerator>(moduleEnumerator);
            }
            if (_useDefaultConfiguration)
            {
                RegisterTypeIfMissing(typeof(IContainerFacade), typeof(UnityContainerAdapter), true);
                RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
                RegisterTypeIfMissing(typeof(RegionAdapterMappings), typeof(RegionAdapterMappings), true);
                RegisterTypeIfMissing(typeof(IRegionManager), typeof(RegionManager), true);
                RegisterTypeIfMissing(typeof(IModuleLoader), typeof(ModuleLoader), true);
            }
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns></returns>
        protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = Container.TryResolve<RegionAdapterMappings>();
            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(Selector), new SelectorRegionAdapter());
                regionAdapterMappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
                regionAdapterMappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());
            }

            return regionAdapterMappings;
        }

        /// <summary>
        /// Initialize the modules. May be overwritten in a derived class to use custom module loading and avoid using an <seealso cref="IModuleLoader"/> and <seealso cref="IModuleEnumerator"/>.
        /// </summary>
        protected virtual void InitializeModules()
        {
            IModuleEnumerator moduleEnumerator = Container.TryResolve<IModuleEnumerator>();
            if (moduleEnumerator == null)
            {
                throw new InvalidOperationException(Resources.NullModuleEnumeratorException);
            }

            IModuleLoader moduleLoader = Container.TryResolve<IModuleLoader>();
            if (moduleLoader == null)
            {
                throw new InvalidOperationException(Resources.NullModuleLoaderException);
            }

            ModuleInfo[] moduleInfo = moduleEnumerator.GetStartupLoadedModules();
            moduleLoader.Initialize(moduleInfo);
        }

        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/></returns>
        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        /// <summary>
        /// Returns the module enumerator that will be used to initialize the modules.
        /// </summary>
        /// <remarks>
        /// When using the default initialization behavior, this method must be overwritten by a derived class.
        /// </remarks>
        /// <returns>An instance of <see cref="IModuleEnumerator"/> that will be used to initialize the modules.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual IModuleEnumerator GetModuleEnumerator()
        {
            return null;
        }

        /// <summary>
        /// Utility method to add a service to the container only if that service is not already in the container.
        /// </summary>
        /// <param name="fromType">The interface of type to register</param>
        /// <param name="toType">The type implementing the interface</param>
        /// <param name="registerAsSingleton"></param>
        protected void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton)
        {
            ILoggerFacade logger = LoggerFacade;

            if (Container.IsTypeRegistered(fromType))
            {
                logger.Log(
                    String.Format(CultureInfo.CurrentCulture,
                                  Resources.TypeMappingAlreadyRegistered,
                                  fromType.Name), Category.Debug, Priority.Low);
            }
            else
            {
                if (registerAsSingleton)
                {
                    Container.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
                }
                else
                {
                    Container.RegisterType(fromType, toType);
                }
            }
        }


        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application. This object will be injected with the default <seealso cref="IRegionManager"/> of
        /// the application in order to be able to add regions using the <seealso cref="RegionManager.RegionNameProperty"/> attached
        /// property from XAML.</returns>
        protected abstract DependencyObject CreateShell();
    }
}