using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Events
{
    public interface IProcessListener
    {
        void Processed();
    }
}
