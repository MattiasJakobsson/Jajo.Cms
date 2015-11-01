﻿using FubuMVC.Core;
using FubuMVC.Core.Registration;
using Jajo.Cms.Components;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using Jajo.Cms.Views;

namespace Jajo.Cms.FubuMVC1
{
    public class CmsFubuRegistry : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x =>
            {
                x.Scan(y =>
                {
                    y.Applies.ToAllPackageAssemblies();
                    y.Applies.ToThisAssembly();
                    y.Applies.ToAssemblyContainingType(registry.GetType());
                    y.Applies.ToAssemblyContainingType<ICmsEndpoint>();

                    y.AddAllTypesOf<ICmsComponent>();
                    y.AddAllTypesOf<ICmsEndpointInput>();
                    y.AddAllTypesOf<ICmsEndpoint>();
                    y.AddAllTypesOf<ITheme>();
                    y.AddAllTypesOf<ICmsViewEngine>();

                    y.ConnectImplementationsToTypesClosing(typeof(ICmsRenderer<>));
                });

                x.SetServiceIfNone<ICmsRenderer, DefaultCmsRenderer>();
            });
        }
    }
}