using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Configuration;
using System.Web;
using System.Runtime.Remoting.Messaging;

namespace DAO.NHibernateMgt
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;
        private static ISession contextSession { get; set; }

        //public static ISession GetSession()
        //{
        //    if (sessionFactory == null)
        //    {
        //        sessionFactory = CreateSessionFactory();
        //    }
        //    return sessionFactory.OpenSession();
        //}

        public static ISession GetSession()
        {
            GetOrSetFactory();
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session["sessionItem"] == null)
                {
                    contextSession = sessionFactory.OpenSession();
                    HttpContext.Current.Session["sessionItem"] = contextSession;
                }
                else
                {
                    contextSession = (ISession)HttpContext.Current.Session["sessionItem"];
                    if (!contextSession.IsOpen)
                    {
                        contextSession = sessionFactory.OpenSession();
                        HttpContext.Current.Session["sessionItem"] = contextSession;
                    }
                }
            }
            else
            {
                if (CallContext.GetData("sessionItem") == null)
                {
                    contextSession = sessionFactory.OpenSession();
                    CallContext.SetData("sessionItem", contextSession);
                }
                else
                {
                    contextSession = (ISession)CallContext.GetData("sessionItem");
                }
            }
        
            if (contextSession.Transaction != null && !contextSession.Transaction.IsActive)
            {
                contextSession.BeginTransaction();
            }
            return contextSession;
        }


        public static void GetOrSetFactory()
        {
            if (sessionFactory == null)
            {
                sessionFactory = CreateSessionFactory();
            }

        }

        public static ISessionFactory CreateSessionFactory()
        {
            string connectionString = ConfigurationManager.AppSettings["CBAConnectionString"];
            ISessionFactory factory = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BranchMap>())
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                            .BuildConfiguration()
                            .BuildSessionFactory();
            //To recreate table db
            //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
            return factory;
        }

        public static ITransaction BuildTransaction(ISession session)
        {
            if (session.Transaction == null || !session.Transaction.IsActive)
            {
                return session.BeginTransaction();
            }
            return session.Transaction;
        }

        public static void Commit()
        {
            ISession session = GetSession();
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
                //session.Flush();
                //session.Close();
            }
        }

        public static void Rollback()
        {
            ISession session = GetSession();
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
                //session.Close();
            }
        }

        public virtual void ClearCurrentSession()
        {
            GetSession().Clear();

        }
    }
}

