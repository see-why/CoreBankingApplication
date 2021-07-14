using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using CBA.Core;


namespace CBA.Data
{
    public class ATMTerminalDAO : EntityDAO<ATMTerminal>
    {
        Customer customer = new Customer();
        public bool CheckCustomerId(string id)
        {
            Customer customer;
            using (var session = NHibernateHelper.GetSession())
            {
                customer = session.QueryOver<Customer>()
                    .Where(x => x.ID.ToString().IsInsensitiveLike(id, MatchMode.Exact)).SingleOrDefault();
            }
            return (customer != null);
        }

        public IList<Customer> Search(string text)
        {
            IList<Customer> customers = new List<Customer>();
            if (!String.IsNullOrEmpty(text))
            {
                using (var session = NHibernateHelper.GetSession())
                {
                    var query = session.QueryOver<Customer>()
                        .Where(y => y.Email.IsInsensitiveLike(text, MatchMode.Anywhere) ||
                        y.ID.ToString().IsInsensitiveLike(text, MatchMode.Anywhere) ||
                        y.FirstName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
                        //y.Gender == (Gender)Enum.Parse(typeof(Gender), text) ||
                        y.LastName.IsInsensitiveLike(text, MatchMode.Anywhere) ||
                        y.OtherNames.IsInsensitiveLike(text, MatchMode.Anywhere) ||
                        y.PhoneNumber.IsInsensitiveLike(text, MatchMode.Anywhere));
                    customers = query.List();
                }
            }
            return customers;
        }

        public int GetLastCustomerId()
        {
            //int lastCustomerId = Session.QueryOver<Customer>().
            //                    Select(x => x.CustomerID).List<int>().FirstOrDefault();
            int lastCustomerId = Session.QueryOver<Customer>()
                                .List().OrderByDescending(x=>x.CustomerID).Select(x=>x.CustomerID).FirstOrDefault();
            return lastCustomerId;
        }

        public IList<Customer> GetByCustomerId(int customerID)
        {
            //There should only be one customer. Its a list for UI purposes
            IList<Customer> customer = Session.QueryOver<Customer>().Where(x => x.CustomerID == customerID).List();
            return customer;
        }

        public Customer GetCustomerByEmail(string email)
        {
            customer = Session.QueryOver<Customer>()
                    .Where(x => x.Email.IsInsensitiveLike(email, MatchMode.Exact)).SingleOrDefault();
            return customer;
        }

        public IList<Customer> GetByFirstAndLastName(string firstName, string lastName)
        {
            IList<Customer> customers = Session.QueryOver<Customer>()
                            .Where(x => x.FirstName.IsInsensitiveLike(firstName, MatchMode.Anywhere))
                            .And(x => x.LastName.IsInsensitiveLike(lastName, MatchMode.Anywhere)).List();
            return customers;            
        }

        public IList<Customer> GetByFirstName(string firstName)
        {
            IList<Customer> customers = Session.QueryOver<Customer>()
                           .Where(x => x.FirstName.IsInsensitiveLike(firstName, MatchMode.Anywhere))
                           .List();
            return customers; 
        }

        public IList<Customer> GetByLastName(string lastName)
        {
            IList<Customer> customers = Session.QueryOver<Customer>()
                           .Where(x => x.LastName.IsInsensitiveLike(lastName, MatchMode.Anywhere))
                           .List();
            return customers; 
        }

        public ATMTerminal GetTerminalByName(string name)
        {
           ATMTerminal terminal= Session.QueryOver<ATMTerminal>()
                .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Exact)).SingleOrDefault();
           return terminal;
        }

        public IList<ATMTerminal> FindTerminalByName(string name)
        {
            IList<ATMTerminal> terminals = Session.QueryOver<ATMTerminal>()
                           .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Anywhere))
                           .List();
            return terminals;
        }
    }
}
