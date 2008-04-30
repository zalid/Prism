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
using System.Xml.Serialization;
using System.Xml;

namespace Modularity.AcceptanceTests.TestInfrastructure
{
    public abstract class DataProviderBase<TEntity> : IDataProvider<TEntity>
    {
        XmlSerializer xmlSerializer = null;
        XmlTextReader xmlReader = null;

        public DataProviderBase()
        {
            xmlSerializer = new XmlSerializer(typeof(List<TEntity>));
            xmlReader = new XmlTextReader(GetDataFilePath());
        }

        public virtual List<TEntity> GetData()
        {
            return (List<TEntity>)(xmlSerializer.Deserialize(xmlReader));
        }

        public virtual List<TEntity> GetDataForId(string id)
        {
            throw new NotImplementedException();
        }

        public virtual int GetCount()
        {
            try
            {
                return ((List<TEntity>)xmlSerializer.Deserialize(xmlReader)).Count;
            }
            catch
            {
                return -1;
            }
        }

        public abstract string GetDataFilePath();
    }
}
