﻿using FubuMVC.Core;
using Jajo.Cms.Endpoints;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class ConfigureEndpointBehaviorChain : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add(x =>
            {
                x.Where.ResourceTypeImplements<ICmsEndpointInput>();
                x.ModifyBy(y => y.Output.AddWriter<CmsEndpointWriter>());
            });
        }
    }
}