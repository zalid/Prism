using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Enums;

namespace CX
{
    public class ComponentMetadata
    {
        public ComponentMetadata(string name, Type interfaceType, Type classType, Lifestyle lifestyle)
        {
            this.Name = name;
            this.InterfaceType = interfaceType;
            this.ClassType = classType;
            this.Lifestyle = lifestyle;
        }

        public string Name { get; set; }
        public Type ClassType { get; set; }
        public Type InterfaceType { get; set; }
        public Lifestyle Lifestyle { get; set; }
    }
}
