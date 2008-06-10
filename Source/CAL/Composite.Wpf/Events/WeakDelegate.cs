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
using System.Reflection;

namespace Microsoft.Practices.Composite.Wpf.Events
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class WeakDelegate<T> where T : class
    {
        private readonly WeakReference _target;
        private readonly MethodInfo _method;
        public WeakDelegate(T @delegate)
        {
            Delegate castedDelegate = @delegate as Delegate;
            if (castedDelegate == null)
                throw new ArgumentNullException("delegate");

            _target = new WeakReference(castedDelegate.Target);
            _method = castedDelegate.Method;
        }

        public T Target
        {
            get
            {
                if (_method.IsStatic)
                {
                    return (T)(object)Delegate.CreateDelegate(typeof(T), null, _method);
                }
                object target = _target.Target;
                if (target != null)
                {
                    return (T)(object)Delegate.CreateDelegate(typeof(T), target, _method);
                }
                return default(T);
            }
        }
    }
}