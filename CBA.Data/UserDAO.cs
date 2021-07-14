using CBA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate;

namespace CBA.Data
{
    public class UserDAO 
    {
        User user = new User();
        protected ISession Session
        {
            get
            {
                return NHibernateHelper.GetSession();
            }

        }

        public IList<User> GetAll()
        {                       
            IList<User> results = Session.QueryOver<User>().List();
            //Session.Evict(results);       
            return results;
        }

        public User GetById(string id)
        {
            user = Session.QueryOver<User>()
                    .Where(x => x.Id.IsInsensitiveLike(id, MatchMode.Exact)).SingleOrDefault();
            return user;
        }
        public User GetUserByEmail(string email)
        {
            user = Session.QueryOver<User>()
                    .Where(x => x.Email.IsInsensitiveLike(email, MatchMode.Exact)).SingleOrDefault();
            return user;
        }

        public void Update(User user)
        {
            //NHibernateHelper.BuildTransaction(session);
            Session.Update(user);
        }

        public void Insert(User user)
        {
            Session.Save(user);
        }
        public User GetUserByUserName(string userName)
        {
            user = Session.QueryOver<User>()
                    .Where(x => x.UserName.IsInsensitiveLike(userName, MatchMode.Exact)).SingleOrDefault();
            return user;
            //User user = new User();
            //using (var session = NHibernateHelper.GetSession())
            //{

            //}
            //return user;
        }
        public IList<User> FindUsers(string userName)
        {
            //IList<Branch> branches = new List<Branch>();
            IList<User> users = Session.QueryOver<User>()
                           .Where(x => x.UserName.IsInsensitiveLike(userName, MatchMode.Anywhere))
                           .List();
            return users;
        }
    }
    //public class UserDAO : EntityDAO<User>
    //{
    //    User user = new User();
    //    public User GetUserByEmail(string email)
    //    {
    //        user = Session.QueryOver<User>()
    //                .Where(x => x.Email.IsInsensitiveLike(email, MatchMode.Exact)).SingleOrDefault();
    //        return user;
    //    }

    //    //Check if email exists
    //    public bool CheckEmail(string email)
    //    {
    //        //User user = new User();
    //        using (var session = NHibernateHelper.GetSession())
    //        {
    //            user = session.QueryOver<User>()
    //                .Where(x => x.Email.IsInsensitiveLike(email, MatchMode.Exact)).SingleOrDefault();
    //        }
    //        return (user != null);
    //    }
        //Get User by username
       

    //    //Search all fields
    //    public IList<User> SearchAllFields(string text)
    //    {
    //        IList<User> users = new List<User>();
    //        if (!string.IsNullOrEmpty(text))
    //        {
    //            var session = NHibernateHelper.GetSession();                
    //                users = session.QueryOver<User>()
    //                    .Where(x => x.Email.IsInsensitiveLike(text, MatchMode.Anywhere) ||
    //                        x.FirstName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
    //                        x.LastName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
    //                        x.OtherNames.IsInsensitiveLike(text, MatchMode.Anywhere) ||
    //                        x.PhoneNumber.IsInsensitiveLike(text, MatchMode.Anywhere) ||
    //                        x.UserName.IsInsensitiveLike(text, MatchMode.Anywhere)
    //                        //||x.Branch.Name.IsInsensitiveLike(text, MatchMode.Anywhere)
    //                        ).List();                
    //        }
    //        return users;
    //    }

    //    //Get user by Login Details
    //    public User GetUserByLoginDetails(string userName, string password)
    //    {
    //        //User user;
    //        using (var session = NHibernateHelper.GetSession())
    //        {
    //            user = session.QueryOver<User>()
    //                .Where(x => x.UserName == userName).SingleOrDefault();
    //        }
    //        return user;
    //    }

    //    //Authenticate user
    //    public bool ValidateUser(string username, string password)
    //    {
    //        //User user;
    //        using (var session = NHibernateHelper.GetSession())
    //        {
    //            user = session.QueryOver<User>().Where(x => x.UserName.IsInsensitiveLike(username, MatchMode.Exact) && x.Password.IsInsensitiveLike(username, MatchMode.Exact)).SingleOrDefault();
    //        }
    //        return (user != null);
    //    }




    //    public bool CheckUserName(string UserName)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
