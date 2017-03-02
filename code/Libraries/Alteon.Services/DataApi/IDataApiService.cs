using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alteon.Model;
using Alteon.Model.Api.Extension;

namespace Alteon.Services.DataApi
{
    public interface IDataApiService
    {
        #region 基础方法
        void AddUserGuid(Api_ClientOwner user);
        /// <summary>
        /// 按照指定标识查找用户
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Api_ClientOwner IsExistOwner(string ownerId = "", string phone = "");
        /// <summary>
        /// 根据终端设备id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Api_ClientEquipment IsExistEquipment(int id, string name = "");
        /// <summary>
        /// 切换设备数据可用状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="equipmentDataId"></param>
        /// <returns></returns>
        bool ChangeEquipmentDataState(int state, int equipmentDataId);
        /// <summary>
        /// 增加设备
        /// </summary>
        /// <param name="equipment"></param>
        void AddEquipment(Api_ClientEquipment equipment);
        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="equipment"></param>
        void UpdateEquipment(Api_ClientEquipment equipment);
        void DeleteEquipment(int equipmentId);
        /// <summary>
        /// 增加设备数据
        /// </summary>
        /// <param name="data"></param>
        //void AddEquipmentData(Api_EquipmentData data);
        /// <summary>
        /// 跟新设备数据
        /// </summary>
        /// <param name="data"></param>
        void UpdateEquimentData(Api_EquipmentData model);
        /// <summary>
        /// 根据设备Id和数据标识查找数据
        /// </summary>
        /// <param name="mark">标识</param>
        /// <returns></returns>
        //Api_EquipmentData IsExistEquimentData(int equipmentId, string mark);
        /// <summary>
        /// 更改用户信息
        /// </summary>
        /// <param name="owner"></param>
        void UpdateOwner(Api_ClientOwner owner);
        void InsertEquipmentData(Api_EquipmentData model);
        Api_EquipmentData IsExistEquipmentData(int equipmentId, string mark);
        #endregion

        #region 扩展方法
        /// <summary>
        /// 根据用户id，分页获取设备列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<Equipment> GetEquipmentAndDataByUserId(string userId, int pageIndex, int pageSize);
        /// <summary>
        /// 根据用户id，获取设备列表的总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetEquipmentAndDataByUserIdCount(string userId);
        /// <summary>
        /// 根据设备id获取设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Equipment GetEquipmentById(int id);
        /// <summary>
        /// 根据设备id分页获取该设备数据
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<Api_EquipmentData> GetEquipmentDataByEquipmentId(int equipmentId, int pageIndex, int pageSize);
        ///// <summary>
        ///// 根据设备id获取该设备数据的总数
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //int GetEquipmentDataByEquipmentIdCount(int id);
        /// <summary>
        /// 根据id获取该数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Api_EquipmentData GetDataById(int id);
        /// <summary>
        /// 获取指定设备id指定标识的数据（当天数据）
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        IList<Alteon.Model.Api.Extension.EquipmentData> GetDataByEquipmentIdAndMark(int equipmentId, string mark);


        /// <summary>
        /// 获取设备数据
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        IList<Alteon.Model.Api.Extension.EquipmentData> GetEquipmentData(string ownerId);

        IList<Alteon.Model.Api.Extension.EquipmentData> GetAllDataByEquipmentIdAndMark(int equipmentId, string mark, ChartSearchParam param);

        IEnumerable<Alteon.Model.Api.Extension.EquipmentData> GetAllDataByEquipmentIdAndMark(int equipmentId, string mark, ChartSearchParam param, int pageIndex, int pageSize);
        int GetAllDataByEquipmentIdAndMarkCount(int equipmentId, string mark, ChartSearchParam param);

        EquipmentDataExt GetEquipmentDataByEquipmentIdAndMark(int equipmentId, string mark);
        decimal GetLastDayElectricity(int equipmentId, string mark);
        //IList<Api_EquipmentData> GetElecDataByEquipmentIdAndMark(int equipmentId, string mark, string strDatetime, int flag);
        int DeleteDataByEquipmentIdAndMark(int eid, string mark);
        #endregion
    }
}
