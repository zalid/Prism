using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Interfaces.Logging
{
    public interface IPrismLogger
    {
        void Log(string message, Category category, Priority priority);
    }
}
