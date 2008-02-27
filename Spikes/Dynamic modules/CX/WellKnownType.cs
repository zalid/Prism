using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CX
{
    public class WellKnownType
    {
        public WellKnownType(Type type, bool isSingleton, bool registerAsConcreteType)
        {
            this.Type = type;
            this.IsSingleton = isSingleton;
            this.RegisterAsConcreteType = registerAsConcreteType;
        }

        public Type Type { get; private set; }
        public bool IsSingleton { get; private set; }
        public bool RegisterAsConcreteType { get; private set; }
    }
}
