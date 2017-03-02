using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Enum
{
    public enum UserType
    {
        /// <summary>
        /// 免费开放
        /// </summary>
        UserGradeFeer = 0,
        /// <summary>
        /// 普通会员
        /// </summary>
        UserGradeRegular = 10,
        /// <summary>
        /// 高级会员
        /// </summary>
        UserGradeVip = 20,
        /// <summary>
        /// 内部人员
        /// </summary>
        UserGradeInterior=800,
        /// <summary>
        /// 管理员
        /// </summary>
        UserGradeAdmin =900,


        UserType

    }
}
