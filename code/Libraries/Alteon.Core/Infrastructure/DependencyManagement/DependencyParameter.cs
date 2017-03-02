using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alteon.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖类型
    /// </summary>
    public sealed class DependencyParameter
    {
        public string Name { get; set; }

        public Type Type { get; set; }
    }
}
