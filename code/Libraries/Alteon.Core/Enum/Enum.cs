using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Enum
{
    /// <summary>
    /// Json日期格式化
    /// </summary>
    public enum JsonDateTimeFormat
    {
        /// <summary>
        /// eg:2012-12-06T14:02:00
        /// </summary>
        IsoDateTime,
        /// <summary>
        /// eg:2012-12-06 14:02:00
        /// </summary>
        IsoDateTimeLong,
        /// <summary>
        /// eg:2012-12-06
        /// </summary>
        IsoDateTimeShort,
        /// <summary>
        /// eg:Date(1198908717056)
        /// </summary>
        JavaScriptDateTime
    }

    /// <summary>
    /// 指定对项列表进行排序的方向。
    /// </summary>
    public enum SortDirectionEnum
    {
        /// <summary>
        /// 升序 从小到大排序。例如，从 A 到 Z。
        /// </summary>
        Ascending = 0,
        /// <summary>
        /// 降序 从大到小排序。例如，从 Z 到 A。
        /// </summary>
        Descending = 1,
    }

    /// <summary>
    /// 操作日志类型
    /// </summary>
    public enum LogTypeEnum
    {
        /// <summary>
        /// 查看
        /// </summary>
        查看,
        /// <summary>
        /// 新增
        /// </summary>
        新增,
        /// <summary>
        /// 修改
        /// </summary>
        修改,
        /// <summary>
        /// 删除
        /// </summary>
        删除,
        /// <summary>
        /// 新增/修改/删除
        /// </summary>
        新增修改删除,
        /// <summary>
        /// 执行（审核之类的功能）
        /// </summary>
        执行,
        /// <summary>
        /// 导出或导入
        /// </summary>
        导出或导入,
        /// <summary>
        /// 其它
        /// </summary>
        其它
    }

    /// <summary>
    /// 系统配置目录或缓存目录
    /// </summary>
    public enum ConfigDir
    {
        /// <summary>
        /// 系统
        /// </summary>
        Sys,
        /// <summary>
        /// 微信
        /// </summary>
        WeiXin
    }

    /// <summary>
    /// 系统常用枚举
    /// </summary>
    public enum SysEnums
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        SysConfig
    }

    /// <summary>
    /// 系统缓存标签(Key)
    /// </summary>
    public enum CacheTags
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        SysConfig,
        /// <summary>
        /// 系统默认样式
        /// </summary>
        DefaultStyle,
        /// <summary>
        /// 系统数据库连接配置
        /// </summary>
        SysDbConnection
    }
}
