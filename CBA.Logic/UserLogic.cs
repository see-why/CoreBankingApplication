using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class UserLogic //: BaseLogic<User, UserDAO>
    {
        UserDAO userDAO = new UserDAO();
        public IList<User> GetAll()
        {
            return userDAO.GetAll();
        }

        public bool IsUserExistingWithEmail(string email)
        {
            if (userDAO.GetUserByEmail(email) != null)
            {
                return true;
            }
            return false;
        }
        
        public void Insert(User user)
        {
            try
            {
                userDAO.Insert(user);
                NHibernateHelper.Commit();
                //msg = "success";
                //return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();
                
            }
        }
        public void Update(User user)
        {
            try
            {
                userDAO.Update(user);
                NHibernateHelper.Commit();
                //msg = "success";
                //return true;
            }
            catch (Exception e)
            {
                NHibernateHelper.Rollback();

            }
        }


        public bool IsUserNameAvailable(string userName)
        {
            if (userDAO.GetUserByUserName(userName) != null)
            {
                return false;
            }
            return true;
        }

        public User GetById(string id)
        {
            return userDAO.GetById(id);
        }

        ////Get user by username
        //public bool IsUserNameAvailable(string userName)
        //{
        //    if (Dao.GetUserByUserName(userName) == null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public bool IsUserExistingWithEmail(string email)
        //{
        //    if (Dao.GetUserByEmail(email) != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public User GetByUserName(string userName)
        //{
        //    return Dao.GetUserByUserName(userName);
        //}

        //public bool CheckEmail(string email)
        //{
        //    return Dao.CheckEmail(email);
        //}

        ////Authenticate user
        //public bool TryLogin(string userName, string password)
        //{
        //    bool valid = false;
        //    string encrypt = MyEncrypt.CreateHash(password);

        //    User user = Dao.GetUserByUserName(userName);
        //    if (user != null)
        //    {

        //        valid = MyEncrypt.ValidatePassword(password, user.Password);
        //    }
        //    return valid;
        //}

        ////Search all fields
        //public IList<User> SearchAllFields(string text)
        //{
        //    return Dao.SearchAllFields(text);
        //}

        //public string GenerateUserName(string firstname, string lastname)
        //{
        //    string UserName;
        //    do
        //    {
        //        string Rand = Utils.GenerateNumber(2).ToString();
        //        UserName = firstname.Substring(0, 3) + lastname + Rand;
        //    } while (Dao.CheckUserName(UserName));
        //    return UserName;
        //}
               
        //public override bool Insert(User user, out string errmsg)
        //{
        //    string UserName = GenerateUserName(user.FirstName,user.LastName);
        //    user.UserName = UserName;
        //    string password = Utils.GeneratePassword(10);
        //    user.Password = MyEncrypt.CreateHash(password);
        //    Console.WriteLine(password);

        //    try
        //    {
        //        Dao.Insert(user);
        //        NHibernateHelper.Commit();
        //        errmsg = password;
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        NHibernateHelper.Rollback();
        //        errmsg = e.Message;
        //        return false;
        //    }
        //}


        

        //public bool ChangePassword(int id, string oldPassword, string newPassword, string confirmPassword)
        //{
        //    bool IsChanged = false;
        //    User user = Dao.GetById(id);
        //    if (user != null)
        //    {
        //        bool valid = MyEncrypt.ValidatePassword(oldPassword, user.Password);
        //        if (valid && (newPassword == confirmPassword))
        //        {
        //            user.Password = MyEncrypt.CreateHash(newPassword);

        //            try
        //            {
        //                Dao.Update(user);
        //                NHibernateHelper.Commit();
        //                IsChanged = true;
        //            }
        //            catch (Exception)
        //            {
        //                NHibernateHelper.Rollback();
        //                IsChanged = false;
        //            }

        //        }

        //    }
        //    return IsChanged;
        //}

        //public string EncryptPassword(string password)
        //{
        //    return MyEncrypt.GetSHA1HashData(password);
        //}

        //public bool IsPasswordValid(string currentPassword, string password)
        //{
        //    return MyEncrypt.ValidateSHA1HashData(currentPassword,password);
        //}


        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> FindUsers(string userName)
        {
            return userDAO.FindUsers(userName);
        }
    }
}
