using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;

namespace CX.UnityContainer
{
    public class DefaultKeyMappingStrategy : BuilderStrategy
    {
        public override object BuildUp(IBuilderContext context, object buildKey, object existing)
        {
            

            return base.BuildUp(context, buildKey, existing);
        }
    }
}
