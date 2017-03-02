using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Data
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        TEntity Get(int id);

        TEntity GetEntity(string sql, object parameters = null);

        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Update(string sql, object parameters);

        bool Delete(TEntity entity);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetList(string sql, object parameters = null, CommandType cmdType = CommandType.Text);
    }
}
