using CBA.Core;
using CBA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Logic
{
    public class CustomerLogic : BaseLogic<Customer, CustomerDAO>
    {
        public IList<Customer> Search(string text)
        {
            return Dao.Search(text);
        }

        //Generate Unique Account Number
        //public string GenerateCustomerId(string firstname,string lastname)
        //{
        //    string CustomerId = "";
        //    do
        //    {
        //        double RandomNumber = Utils.GenerateNumber(3);
        //        CustomerId = "CBA." + firstname + lastname + RandomNumber.ToString();
        //    } while (Dao.CheckCustomerId(CustomerId)); //if account number exists , it enters loop again

        //    return CustomerId;
        //}
        public int GenerateCustomerId() 
        {
            int lastCustomerID = Dao.GetLastCustomerId();
            //int customerID = 0;
            //if (lastCustomerID == 0)
            //{
            //    lastCustomerID = int.Parse(lastCustomerID.ToString("D6"));
            //}

            return ++lastCustomerID;
        }

        //There should only be one customer. Its a list for UI purposes
        public IList<Customer> GetByCustomerId(int customerID)
        {
            return Dao.GetByCustomerId(customerID);          
        }

        public IList<Customer> GetByFirstAndLastName(int p)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> GetByFirstName(int p)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> GetByLastName(int p)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> GetByFirstAndLastName(string firstName, string lastName)
        {
           return Dao.GetByFirstAndLastName(firstName, lastName);
        }

        public IList<Customer> GetByFirstName(string firstName)
        {
            return Dao.GetByFirstName(firstName);
        }

        public IList<Customer> GetByLastName(string lastName)
        {
            return Dao.GetByLastName(lastName);
        }

        public bool IsCustomerExistingWithEmail(string email)
        {
            if (Dao.GetCustomerByEmail(email) != null)
            {
                return true;
            }
            return false;
        }
    }
}
