using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace CX.UnityContainer
{
    public class PrismExtension : UnityContainerExtension
    {
        private ExtensionContext context;

        protected override void Initialize(ExtensionContext extensionContext)
        {
            extensionContext.Strategies.AddNew<BuildKeyMappingStrategy>(UnityBuildStage.TypeMapping);

            this.context = extensionContext;

            this.context.RegisteringTypeMapping += new EventHandler<RegisterTypeMappingEventArgs>(context_RegisteringTypeMapping);
        }

        void context_RegisteringTypeMapping(object sender, RegisterTypeMappingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Name))
            {
                //Adds a type mapping so that whenever a new type is registered, it is also set to as the default
                context.Policies.Set<IBuildKeyMappingPolicy>(
                    new BuildKeyMappingPolicy(new NamedTypeBuildKey(e.TypeFrom, e.Name)),
                    new NamedTypeBuildKey(e.TypeFrom));
            }
        }
    }
}
