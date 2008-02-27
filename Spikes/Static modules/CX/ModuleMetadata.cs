using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Enums;

namespace CX
{
    public class ModuleMetadata
    {
        public ModuleMetadata(string name, Type classType)
        {
            this.Name = name;
            this.ClassType = classType;
        }

        public string Name { get; set; }
        public Type ClassType { get; set; }
    }
}
