using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Web;
using System.Web.Mvc;

using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Autofac.Core;
using Autofac.Builder;

using Alteon.Core.Infrastructure.DependencyManagement;

namespace Alteon.Core.Infrastructure
{
    public sealed class EngineContext
    {
        #region Fileds

        private static ContainerManager _containerManager;
        private static Assembly[] _assemblies;

        #endregion

        #region  Properties

        public static ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        public static Assembly[] CurrentAssemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/bin/")).GetFiles("*.dll")
                                                                                                 .Where(x => x.Name.ToLower().Contains("alteon")) //过滤不必要的dll
                                                                                                 .Select(x => Assembly.LoadFrom(x.FullName))
                                                                                                 .ToArray();
                }

                return _assemblies;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Engine Initialize
        /// </summary>
        public static void Initialize()
        {
            //Ioc
            RegisterDependencies();

            //StartUpTask
            RunStartUpTask();
        }

        #endregion

        #region Ioc Resolve Dependency

        /// <summary>
        /// 从Ioc容器中获取已注入的对象
        /// </summary>
        public static T Resolve<T>(object key = null) where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        ///  从Ioc容器中获取已注入的对象
        /// </summary>
        public static object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// 从Ioc容器中获取已注入的对象
        /// </summary>
        public static T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        /// <summary>
        /// 注入对象(Ioc容器中尚未注入的)
        /// </summary>
        public static T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ContainerManager.ResolveUnregistered<T>(scope);
        }

        /// <summary>
        /// 注入对象(Ioc容器中尚未注入的)
        /// </summary>
        public static object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            return ContainerManager.ResolveUnregistered(type, scope);
        }

        #endregion

        #region Utilities

        private static void RegisterDependencies()
        {
            //Build() or Update() method can only be called once on a ContainerBuilder.
            var builder = new ContainerBuilder();

            //Find IDependencyRegistrar Impls
            var drTypes = CurrentAssemblies.SelectMany(x => x.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDependencyRegistrar))));
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
            {
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }

            //Sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            drInstances.ForEach(_ => _.Register(builder));

            //自动注册特性
            AutoRegister(CurrentAssemblies, builder);

            var container = builder.Build();
            _containerManager = new ContainerManager(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RunStartUpTask()
        {
            var drTaskTypes = CurrentAssemblies.SelectMany(x => x.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IStartupTask))));
            var drTaskInstances = new List<IStartupTask>();
            foreach (var taskType in drTaskTypes)
            {
                drTaskInstances.Add((IStartupTask)Activator.CreateInstance(taskType));
            }

            drTaskInstances = drTaskInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            drTaskInstances.ForEach(_ => _.Execute());
        }

        /// <summary>
        /// 将使用"自动注册类型特性"的类注入容器
        /// </summary>
        private static void AutoRegister(Assembly[] currentAssemblies, ContainerBuilder builder)
        {
            var searchPlatform = ComponentPlatform.Default;
            ComponentPlatform autoFacePlatform = (ComponentPlatform)Common.CommFun.getAppSettings<int>("IocPlatformId");//平台

            var drTypes = currentAssemblies.Where(x => !x.IsDynamic).SelectMany(x => x.GetExportedTypes());

            do//循环首次只注入默认实现，第二次注入autoFacePlatform平台实现
            {
                foreach (var type in drTypes)
                {
                    var autoRegisterAttribute = type.GetCustomAttribute<AutoRegisterAttribute>(false);
                    if (autoRegisterAttribute == null)
                    {
                        continue;
                    }

                    //只注入默认实现
                    if (!autoRegisterAttribute.AutoFacePlatform.Equals(searchPlatform))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(autoRegisterAttribute.Name))
                    {
                        var registrationBuilder = builder.RegisterType(type).As(type.GetInterfaces()).PerLifeStyle(autoRegisterAttribute.LifeStyle);
                        WithParameter(autoRegisterAttribute, registrationBuilder);
                    }
                    else
                    {
                        foreach (var interfaceType in type.GetInterfaces())
                        {
                            var registrationBuilder = builder.RegisterType(type).Keyed(autoRegisterAttribute.Name, interfaceType).PerLifeStyle(autoRegisterAttribute.LifeStyle);
                            WithParameter(autoRegisterAttribute, registrationBuilder);
                        }
                    }
                }

            } while (!searchPlatform.Equals(autoFacePlatform) && (searchPlatform = autoFacePlatform).Equals(autoFacePlatform));
        }

        /// <summary>
        /// 带参数型注入
        /// </summary>
        private static void WithParameter(AutoRegisterAttribute autoRegisterAttribute, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder)
        {
            if (autoRegisterAttribute.Parameters == null
                || autoRegisterAttribute.Parameters.Count == 0)
            {
                return;
            }

            foreach (var parameter in autoRegisterAttribute.Parameters)
            {
                var method = typeof(ResolvedParameter).GetMethod("ForNamed").MakeGenericMethod(parameter.Type);
                registrationBuilder = registrationBuilder.WithParameter((Parameter)method.Invoke(null, new object[] { parameter.Name }));
            }
        }

        #endregion
    }
}
