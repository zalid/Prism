using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CX.Interfaces
{
    public interface IRegion
    {
        void AddElement<TElement>(TElement element, string name);
    }
}
