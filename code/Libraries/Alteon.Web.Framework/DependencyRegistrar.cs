using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Alteon.Core;
using Alteon.Core.Infrastructure;
using Alteon.Core.Infrastructure.DependencyManagement;
using Alteon.Core.Caching;
using Alteon.Core.Data;
using Alteon.Data;

using Autofac;
using Autofac.Integration.Mvc;

namespace Alteon.Web.Framework
{
    public sealed class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder)
        {
            //httpcontext
            builder.Register(c => (new HttpContextWrapper(HttpContext.Current) as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //controllers，
            builder.RegisterControllers(EngineContext.CurrentAssemblies);

            //dapper
            builder.Register<IDbSession>(_ => new DbSession("AlteonConnectionString")).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DapperRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Keyed<ICacheManager>(typeof(MemoryCacheManager)).SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Keyed<ICacheManager>(typeof(PerRequestCacheManager)).InstancePerLifetimeScope();

            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}

