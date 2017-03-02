using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alteon.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Ioc自动注入特性
    /// </summary>
    public sealed class AutoRegisterAttribute : Attribute
    {
        public AutoRegisterAttribute(ComponentLifeStyle lifeStyle, ComponentPlatform autoFacePlatform = ComponentPlatform.Default)
        {
            this.LifeStyle = lifeStyle;
            this.AutoFacePlatform = autoFacePlatform;
        }

        /// <summary>
        /// 平台类型
        /// </summary>
        public ComponentPlatform AutoFacePlatform { get; private set; }

        public ComponentLifeStyle LifeStyle { get; private set; }

        public string Name { get; set; }

        public List<DependencyParameter> Parameters { get; set; }
    }
}
