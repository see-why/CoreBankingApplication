using DAO.Interfaces;
using NHibernate;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using System.Reflection;
using CBA.Core;
using FluentNHibernate.Cfg.Db;
using System.Configuration;
using NHibernate.Tool.hbm2ddl;


namespace CBA.Data
{
    public class EntityDAO<T> : IEntityDAO<T,int> where T : Entity, new()
    {
       // private ISession _session;
        protected  ISession Session
        {
            get 
            {
                return NHibernateHelper.GetSession(); 
            }
            
        }
        
        //Fetch all items
        public virtual IList<T> GetAll()
        {
            IList<T> results = Session.QueryOver<T>().List();
            //Session.Evict(results);       
            return results;
        }

        //Fetch an item using an Id
        public T GetById(int id)
        {            
            T result = Session.Get<T>(id);
            //Session.Flush();
            return result;
        }
        //public T GetByName(int id)
        //{
        //    T result = Session.QueryOver<T>()
        //        .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Exact)).SingleOrDefault();
        //    return category;
            
        //}

        //Insert into the database
        public void Insert(T obj)
        {
            obj.DateCreated= obj.DateModified  = DateTime.Now;
            Session.Save(obj);
            //NHibernateHelper.Commit();
        }

        public void InsertWithCommit(T obj)
        {
            obj.DateCreated = obj.DateModified = DateTime.Now;
            Session.Save(obj);
            NHibernateHelper.Commit();
        }
        //public void Insert(List<T> obj)
        //{
        //   // obj.DateCreated = obj.DateModified = DateTime.Now;
        //    Session.Save(obj);
        //    // NHibernateHelper.Commit();
        //}

        //Save or update
        public void SaveOrUpdate(T obj)
        {           
            //NHibernateHelper.BuildTransaction(session);
            Session.SaveOrUpdate(obj);
            NHibernateHelper.Commit();
        }

        //Update the database
        public void Update(T obj)
        {
            obj.DateModified = DateTime.Now;
            //NHibernateHelper.BuildTransaction(session);
            Session.Update(obj);
        }
        public void UpdateWithCommit(T obj)
        {
            obj.DateModified = DateTime.Now;
            //NHibernateHelper.BuildTransaction(session);
            Session.Update(obj);
            NHibernateHelper.Commit();
        }

        public void Merge(T obj)
        {           
            Session.Merge(obj);  
        }



        public void Update(T obj, int id)
        {
            obj.DateModified = DateTime.Now;
            //Session.Merge(obj);
            Session.Update(obj, id);
            NHibernateHelper.Commit();
        }
    }
}
