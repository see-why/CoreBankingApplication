using CBA.Core;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class TellerDAO : EntityDAO<Teller>
    {
        Teller teller = new Teller();
        //Check if Teller has been assigned a Till account
        public bool isAssignedTill(User user, GLAccount account)
        {
            ISession session = NHibernateHelper.GetSession();
            var result = session.QueryOver<Teller>()
                    .Where(x => x.TillAccount == account || x.User == user).List();
            return (result.Count > 0);

        }

        //Get Teller By Till User
        public Teller GetTellerByUser(User user)
        {                        
            teller = Session.QueryOver<Teller>()
                .Where(x => x.User == user).SingleOrDefault();
            
            return teller;
        }

        //Get Teller By Till Username
        public Teller GetTellerByUsername(string username)
        {
            Teller teller = new Teller();
            var session = NHibernateHelper.GetSession();
            teller = session.QueryOver<Teller>().JoinQueryOver<User>(t => t.User).Where(u => u.UserName == username).SingleOrDefault();

            return teller;
        }
        //Get Teller By assigned account
        public Teller GetTellerByAccount(GLAccount account)
        {
            ISession session = NHibernateHelper.GetSession();
            Teller teller = session.QueryOver<Teller>()
                .Where(x => x.TillAccount == account).SingleOrDefault();
            return teller;

        }


        public IList<GLAccount> GetAllTills()
        {
            IList<GLAccount> accounts = Session.QueryOver<GLAccount>()
                .Where(x => x.Name.IsInsensitiveLike("TIll Account", MatchMode.Start)).List();
            return accounts;           
        }

        public object GetTellerByTillAccount(GLAccount tillAccount)
        {
            Teller teller = Session.QueryOver<Teller>()
                .Where(x => x.TillAccount == tillAccount).SingleOrDefault();
            return teller;
        }

        public IList<User> GetUnAssignedUsers()
        {
            //var query = "SELECT * FROM AspNetUsers JOIN Teller ON Teller.User_id <> AspNetUsers.id";
            //var query = "SELECT  [dbo].[AspNetUsers].[Id],[FirstName],[LastName],[OtherNames],[PhoneNumber],[Email],[BranchId] ,[IsSuperAdmin]" +
            //        ",[EmailConfirmed] ,[PasswordHash] ,[SecurityStamp] ,[PhoneNumberConfirmed] ,[TwoFactorEnabled] ,[LockoutEndDateUtc]"+
            //        " ,[LockoutEnabled] ,[AccessFailedCount] ,[UserName]  FROM [CBA_DB].[dbo].[AspNetUsers] INNER JOIN Teller ON Teller.User_id <> AspNetUsers.id";
            //var result = Session.CreateSQLQuery(query).List<User>();

           // Teller teller = null;
           // User user = null;
            //var users = Session.QueryOver<User>(() => user)
            //            .JoinQueryOver<Teller>(() => teller)                        
            //            .Where(() => teller.User != user).List<User>();
            //IList<User> users = null;
            var tellers = Session.QueryOver<Teller>().Select(x => x.User.Id).List<string>();
            var users = Session.QueryOver<User>().List();
            var unassignedUsers = users.Where(p => !tellers.Contains(p.Id)).ToList();
            return unassignedUsers;
        }
        //public IList<GLAccount> GetUnAssignedTills() 
        //{
        //    var tellers = Session.QueryOver<Teller>().Select(x => x.TillAccount.ID).List<int>();
        //    var tills = GetAllTills();
        //    var unassignedTills = tills.Where(p => !tellers.Contains(p.ID)).ToList();
        //    return unassignedTills;
        //}
        public IList<GLAccount> GetUnAssignedTills()
        {
            var tellers = Session.QueryOver<Teller>().Select(x => x.TillAccount.ID).List<int>();
            GLAccountDAO glAccountDAO = new GLAccountDAO();
            var tills = glAccountDAO.GetGLAccountsByMainAccount(AccountCategory.ASSET);
           // var tills = GetAllTills();
            var unassignedTills = tills.Where(p => !tellers.Contains(p.ID)).ToList();
            return unassignedTills;
        }

        public IList<Teller> FindTellersByUsername(string userName)
        {
            IList<Teller> tellers = GetAll()
                                    .Where(x => x.User.UserName.Contains(userName))
                                    .ToList();
            return tellers;
        }

        public IList<Teller> FindTellersByTillAccountName(string tillName)
        {
            IList<Teller> tellers = GetAll()
                                    .Where(x => x.TillAccount.Name.Contains(tillName))
                                    .ToList();
            return tellers;
        }
    }
}
