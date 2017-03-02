using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Autofac.Builder;
using Autofac.Integration.Mvc;

namespace Alteon.Core.Infrastructure.DependencyManagement
{
    public static class ContainerManagerExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PerLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, ComponentLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case ComponentLifeStyle.LifetimeScope:
                    return HttpContext.Current != null ? builder.InstancePerHttpRequest() : builder.InstancePerLifetimeScope();

                case ComponentLifeStyle.Transient:
                    return builder.InstancePerDependency();

                case ComponentLifeStyle.Singleton:
                    return builder.SingleInstance();

                default:
                    return builder.SingleInstance();
            }
        }
    }
}
