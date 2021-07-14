using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CBA.Logic
{
    public class BaseLogic<T, Y>
        where T : Entity, new()
        where Y : EntityDAO<T>, new()
    {
        public Y Dao;
        public BaseLogic()
        {
            Dao = new Y();
        }

        public IList<T> GetAll()
        {
            return Dao.GetAll();
        }

        public T GetById(int id)
        {
            return Dao.GetById(id);
        }

        public virtual bool Insert(T obj, out string msg)
        {
            try
            {
                Dao.Insert(obj);
                NHibernateHelper.Commit();
                msg = "success";
                return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
                msg = e.Message;
                return false;
            }
        }
        public virtual bool Insert(T obj)
        {
            try
            {
                Dao.Insert(obj);
                NHibernateHelper.Commit();
                //msg = "success";
                return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
               // msg = e.Message;
                return false;
            }
        }

        public bool Update(T obj, out string msg)
        {
            try
            {
                Dao.Update(obj);
                NHibernateHelper.Commit();
                msg = "success";
                return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
                msg = e.Message;
                return false;
            }


        }

        public bool Update(T obj)
        {
            try
            {
                Dao.Update(obj);
                NHibernateHelper.Commit();
                //msg = "success";
                return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
               // msg = e.Message;
                return false;
            }


        }
        public bool Update(T obj, int id)
        {
            try
            {
                Dao.Update(obj, id);
                NHibernateHelper.Commit();
                //msg = "success";
                return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
                // msg = e.Message;
                return false;
            }


        }

        //public bool Merge(T obj, out string msg)
        //{
        //    try
        //    {
        //        anyDAO.Merge(obj);
        //        NHibernateHelper.Commit();
        //        msg = "success";
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        NHibernateHelper.Rollback();
        //        msg = ex.Message;
        //        return false;
        //    }
        //}
    }
}
