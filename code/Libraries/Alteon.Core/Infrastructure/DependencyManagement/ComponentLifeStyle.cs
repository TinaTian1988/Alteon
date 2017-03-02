using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alteon.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public enum ComponentLifeStyle
    {
        Singleton = 0,

        Transient = 1,

        LifetimeScope = 2
    }
}
