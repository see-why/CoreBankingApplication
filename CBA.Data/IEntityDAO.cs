using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interfaces
{
    public interface IEntityDAO<T,TID> where T : Entity
    {
        IList<T> GetAll();
        T GetById(int id);
        void Insert(T obj);
       // void Update(T obj);
        void Update(T obj, TID id);       

    }
}
