using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Configuration;
using System.Web;
using System.Runtime.Remoting.Messaging;
using CBA.Mappings;
using FluentNHibernate.Conventions.Helpers;

namespace CBA.Data
{
    public class NHibernateHelper
    {
        private const string SESSION_KEY = "CONTEXT_SESSION";
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private static ISessionFactory sessionFactory;
        private static ISession contextSession { get; set; }

        static NHibernateHelper()
        {
           InitSessionFactory();
           DatabaseInitializer.InitializeDB();
            //I uncomment this part to populate the DB tables with records when I drop the exiting tables
           //DtabaseInitializer.InitializeDB(); 
        }

        private static void InitSessionFactory()
        {
            if (sessionFactory != null) return;
           
            sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(c => c
                .FromConnectionStringWithKey("connectionString")))
              .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AccountTypeMap>()
               // .Mappings(m => m.FluentMappings.Add<LoanAccountMap>()
                //.Conventions.Add(
                //            //ConventionBuilder.Id.Always(x => x.GeneratedBy.Identity()),
                //            //ConventionBuilder.HasMany.Always(x => x.Cascade.All()),
                //            //ConventionBuilder.Property.Always(x => x.Column(x.Property.Name)),
                //            ////Table.Is(o => Inflector.Pluralize(o.EntityType.Name)),
                //            //PrimaryKey.Name.Is(o => "ID"),
                //            //ForeignKey.EndsWith("ID"),
                //            //DefaultLazy.Always(),
                //            //DefaultCascade.All(),

                //            ConventionBuilder.Property.When(
                //                c => c.Expect(x => x.Property.PropertyType.IsEnum),
                //                x => x.CustomType(x.Property.PropertyType))
                //        ))
                )
            //.ExposeConfiguration(cfg => new SchemaExport(cfg).Drop(true, false))
           // .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, false))
            
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false,true))

            .BuildSessionFactory();

        }
        private static ISession Session
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ISession)HttpContext.Current.Session[SESSION_KEY];
                }
                else
                {
                    return (ISession)CallContext.GetData(SESSION_KEY);
                }
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Session[SESSION_KEY] = value;
                }
                else
                {
                    CallContext.SetData(SESSION_KEY, value);
                }
            }
        }
        private static bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        public static ISession GetSession()
        {
            return GetSession(null);
        }
        private static ISession GetSession(IInterceptor interceptor)
        {
            //if (Session != null) return Session;
            //InitSessionFactory();
            
            ISession contextSession = Session;

            if (contextSession == null)
            {
                if (interceptor != null)
                {
                    contextSession = sessionFactory.OpenSession(interceptor);
                }
                else
                {
                    contextSession = sessionFactory.OpenSession();
                }
                
            }
            if (contextSession.Transaction != null && !contextSession.Transaction.IsActive)
            {
                contextSession.BeginTransaction();
            }
            Session = contextSession;

            return contextSession;
        }

        

        //public static ISession GetSession()
        //{
        //    GetOrSetFactory();
        //    if (HttpContext.Current != null)
        //    {
        //        if (HttpContext.Current.Session["session"] == null)
        //        {
        //            contextSession = sessionFactory.OpenSession();
        //            HttpContext.Current.Session["session"] = contextSession;
        //        }
        //        else
        //        {
        //            contextSession = (ISession)HttpContext.Current.Session["session"];
        //            if (!contextSession.IsOpen)
        //            {
        //                contextSession = sessionFactory.OpenSession();
        //                HttpContext.Current.Session["session"] = contextSession;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (CallContext.GetData("session") == null)
        //        {
        //            contextSession = sessionFactory.OpenSession();
        //            CallContext.SetData("session", contextSession);
        //        }
        //        else
        //        {
        //            contextSession = (ISession)CallContext.GetData("session");
        //        }
        //    }
        
        //    if (contextSession.Transaction != null && !contextSession.Transaction.IsActive)
        //    {
        //        contextSession.BeginTransaction();
        //    }
        //    return contextSession;
        //}


        //public static void GetOrSetFactory()
        //{
        //    if (sessionFactory == null)
        //    {
        //        sessionFactory = CreateSessionFactory();
        //    }

        //}

        //public static ISessionFactory CreateSessionFactory()
        //{
        //    string connectionString = ConfigurationManager.AppSettings["CBAConnectionString"];
        //    ISessionFactory factory = Fluently.Configure()
        //                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
        //                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BranchMap>())
        //        //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
        //                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
        //                    .BuildConfiguration()
        //                    .BuildSessionFactory();
        //    //To recreate table db
        //    //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
        //    return factory;
        //}

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
            ISession session = Session;//GetSession();
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
               // session.Flush();
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

