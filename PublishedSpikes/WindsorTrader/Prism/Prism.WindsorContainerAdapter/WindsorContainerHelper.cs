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
using Castle.Windsor;

namespace Prism.WindsorContainerAdapter
{
    public static class WindsorContainerHelper
    {

        public static bool IsTypeRegistered(this IWindsorContainer container, Type type)
        {
            return container.Kernel.HasComponent(type);
        }

        /// <summary>
        /// Utility method to try to resolve a service from the container avoiding an exception if the container cannot build the type.
        /// </summary>
        /// <param name="container">The cointainer that will be used to resolve the type</param>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>The instance of <typeparamref name="T"/> built up by the container</returns>
        public static T TryResolve<T>(this IWindsorContainer container)
        {
            object result = TryResolve(container, typeof(T));
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        /// <summary>
        /// Utility method to try to resolve a service from the container avoiding an exception if the container cannot build the type.
        /// </summary>
        /// <param name="container">The cointainer that will be used to resolve the type</param>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <returns>The instance of <paramref name="typeToResolve"/> built up by the container</returns>
        public static object TryResolve(this IWindsorContainer container, Type typeToResolve)
        {
            if (container.Kernel.HasComponent(typeToResolve))
            {
                return container.Resolve(typeToResolve);
            }

            return null;
        }
    }
}
