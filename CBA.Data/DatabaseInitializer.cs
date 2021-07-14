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
using FluentNHibernate.Cfg.Db;
using System.Configuration;
using NHibernate.Tool.hbm2ddl;
using CBA.Core;


namespace CBA.Data
{
    public class DatabaseInitializer
    {
        public static void InitializeDB(){
            //addMainAccounts();
            //addAccountTypes();
            //addGLCategories();
            //addGLAccounts();
            //addFinancialDate();
            //addGender();
            //addCustomer();
            //addLoanStatus();
            //addEOD();
        }

        private static void addEOD()
        {
            EODDAO eodDAO = new EODDAO();
            FinancialDateDAO finDateDAO = new FinancialDateDAO();
            EOD eod = new EOD 
            {
                BusinessStatus= Business.OPEN,
                FinancialDate = finDateDAO.GetById(1).CurrentFinancialDate
                
            };
            eodDAO.InsertWithCommit(eod);

        }

        //private static void addTeller() 
        //{
        //    TellerDAO tellerDAO = new TellerDAO();
        //}
        private static void addLoanStatus()
        {
            LoanStatusDAO loanStatusDAO = new LoanStatusDAO();
            LoanStatus loanStatus1 = new LoanStatus { Name = LoanStatusEnum.UNPAID };
            LoanStatus loanStatus2 = new LoanStatus { Name = LoanStatusEnum.PAID };
            LoanStatus loanStatus3 = new LoanStatus { Name = LoanStatusEnum.DEFAULTED };
            loanStatusDAO.InsertWithCommit(loanStatus1);
            loanStatusDAO.InsertWithCommit(loanStatus2);
            loanStatusDAO.InsertWithCommit(loanStatus3);
            //throw new NotImplementedException();
        }
        static GLCategoryDAO glCategoryDAO = new GLCategoryDAO();

        private static void addGender() 
        {
            GenderDAO genderDAO = new GenderDAO();
            Gender maleGender = new Gender{ Name= GenderEnum.MALE};
            Gender femaleGender = new Gender { Name = GenderEnum.FEMALE };
            genderDAO.InsertWithCommit(maleGender);
            genderDAO.InsertWithCommit(femaleGender);

        }
        private static void addCustomer()
        {
            CustomerDAO customerDAO = new CustomerDAO();
            GenderDAO genderDAO = new GenderDAO();

            Customer customer = new Customer
            {
                CustomerID = 1,
                FirstName = "Eden",
                LastName = "Hazard",
                OtherNames = "Eden",
                PhoneNumber = "11111111",
                Email = "a@f.com",
                Address = "london, England",
                Gender = genderDAO.GetById(1)
            };
            Customer customer1 = new Customer
            {
                CustomerID = 2,
                FirstName = "Diego",
                LastName = "Costa",
                OtherNames = "",
                PhoneNumber = "222222222222",
                Email = "a@s.com",
                Address = "london, England",
                Gender = genderDAO.GetById(2)
            };
            customerDAO.InsertWithCommit(customer);
            customerDAO.InsertWithCommit(customer1);
        }
        private static void addAccountTypes() 
        {
            AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
            AccountType accountType1 = new AccountType { Name = AccountTypeEnum.SAVINGS };
            AccountType accountType2 = new AccountType { Name = AccountTypeEnum.CURRENT };
            AccountType accountType3 = new AccountType { Name = AccountTypeEnum.LOAN };
            accountTypeDAO.InsertWithCommit(accountType1);
            accountTypeDAO.InsertWithCommit(accountType2);
            accountTypeDAO.InsertWithCommit(accountType3);

        }
        private static void addFinancialDate() 
        {
            FinancialDateDAO finDateDAO = new FinancialDateDAO();
            FinancialDate finDate = new FinancialDate 
            {
                CurrentFinancialDate = DateTime. Now
            };
            //FinancialDate finDate2 = new FinancialDate
            //{
            //    CurrentFinancialDate = DateTime.Parse("7/21/2015")
            //};
            finDateDAO.InsertWithCommit(finDate);
            //finDateDAO.Insert(finDate2);
        }
        private static void addMainAccounts()
        {
            MainAccountDAO mainAccountDAO = new MainAccountDAO();

            //List<MainAccount> mainAccounts = new List<MainAccount>{
            //    new MainAccount { Name = AccountCategory.ASSET},
            //    new MainAccount { Name = AccountCategory.CAPITAL},
            //    new MainAccount { Name = AccountCategory.EXPENSE},
            //    new MainAccount { Name = AccountCategory.INCOME},
            //    new MainAccount { Name = AccountCategory.LIABILITY}
            //};

            MainAccount mainAccount1 = new MainAccount
            {
                Name = AccountCategory.ASSET
            };
            MainAccount mainAccount2 = new MainAccount
            {
                Name = AccountCategory.LIABILITY
                
            };
            MainAccount mainAccount3 = new MainAccount
            {
                Name = AccountCategory.CAPITAL
                
            };
            MainAccount mainAccount4 = new MainAccount
            {
                Name = AccountCategory.INCOME
            };
            MainAccount mainAccount5 = new MainAccount
            {
                Name = AccountCategory.EXPENSE
            };

            mainAccountDAO.InsertWithCommit(mainAccount1);
            mainAccountDAO.InsertWithCommit(mainAccount2);
            mainAccountDAO.InsertWithCommit(mainAccount3);
            mainAccountDAO.InsertWithCommit(mainAccount4);
            mainAccountDAO.InsertWithCommit(mainAccount5);

            //mainAccountDAO
        }
        private static void addGLCategories()
        {
            MainAccountDAO mainAccDAO = new MainAccountDAO();


            MainAccount mainAcccount1 = mainAccDAO.GetMainAccountByName(AccountCategory.ASSET);
            MainAccount mainAcccount2 = mainAccDAO.GetMainAccountByName(AccountCategory.INCOME);
            MainAccount mainAcccount3 = mainAccDAO.GetMainAccountByName(AccountCategory.EXPENSE);
            MainAccount mainAcccount4 = mainAccDAO.GetMainAccountByName(AccountCategory.CAPITAL);
            GLCategory category1 = new GLCategory
            {
                Name="Cash Assets",
                MainCategory= mainAcccount1,
                Description = "This is for holding cash"
            };
            GLCategory category2 = new GLCategory
            {
                Name = "Notes",
                MainCategory = mainAcccount1,
                Description = "This is for holding notes"
            };
            GLCategory category3 = new GLCategory
            {
                Name = "Interest Income",
                MainCategory = mainAcccount2,
                Description = "This is for holding interest income"
            };
            GLCategory category4 = new GLCategory
            {
                Name = "Interest Expense",
                MainCategory = mainAcccount3,
                Description = "This is for holding interest expense"
            };
            GLCategory category5 = new GLCategory
            {
                Name = "Capital category",
                MainCategory = mainAcccount4,
                Description = "This is for holding Capital"
            };
            glCategoryDAO.InsertWithCommit(category1);
            glCategoryDAO.InsertWithCommit(category2);
            glCategoryDAO.InsertWithCommit(category3);
            glCategoryDAO.InsertWithCommit(category4);
            glCategoryDAO.InsertWithCommit(category5);
        }
        private static void addGLAccounts() 
        {
            GLAccountDAO glAccountDAO = new GLAccountDAO();
            BranchDAO branchDAO = new BranchDAO();

            GLCategory glCategory1 = glCategoryDAO.GetById(1);
            GLCategory glCategory2 = glCategoryDAO.GetById(2);
            GLCategory glCategory3 = glCategoryDAO.GetById(3);
            GLCategory glCategory4 = glCategoryDAO.GetById(4);
            GLCategory glCategory5 = glCategoryDAO.GetById(5);

            GLAccount account10 = new GLAccount
            {
                Name = "Capital Account",
                Balance = 10000000, //Vault and cash have the same balance
                GLCategory = glCategory5,
                Code = glAccountDAO.GetCode(glCategory5),
                Branch = branchDAO.GetById(2),
            };
            glAccountDAO.InsertWithCommit(account10);

            GLAccount account4 = new GLAccount
            {
                Name = "Vault",
                Balance = 10000000,
                GLCategory = glCategory2,
                Code = glAccountDAO.GetCode(glCategory2),
                Branch = branchDAO.GetById(2),

            };
            glAccountDAO.InsertWithCommit(account4);

            GLAccount account1 = new GLAccount 
            {
                Name = "Till Account 1",
                GLCategory = glCategory1,
                Code = glAccountDAO.GetCode(glCategory1),
                Branch = branchDAO.GetById(1),
               
            };
            glAccountDAO.InsertWithCommit(account1);
            GLAccount account2 = new GLAccount
            {
                Name = "Till Account 2",
                GLCategory = glCategory1,
                Code = glAccountDAO.GetCode(glCategory1),
                Branch = branchDAO.GetById(1),


            };
            glAccountDAO.InsertWithCommit(account2);
            GLAccount account3 = new GLAccount
            {
                Name = "Till Account 3",
                GLCategory = glCategory1,
                Code = glAccountDAO.GetCode(glCategory1),
                Branch = branchDAO.GetById(1),

            };
            glAccountDAO.InsertWithCommit(account3);
            
            GLAccount account5 = new GLAccount
            {
                Name = "Interest Income 1",
                GLCategory = glCategory3,
                Code = glAccountDAO.GetCode(glCategory3),
                Branch = branchDAO.GetById(1),

            };                       
            glAccountDAO.InsertWithCommit(account5);
            GLAccount account6 = new GLAccount
            {
                Name = "Interest Income 2",
                GLCategory = glCategory3,
                Code = glAccountDAO.GetCode(glCategory3),
                Branch = branchDAO.GetById(1),

            };
            glAccountDAO.InsertWithCommit(account6);
            GLAccount account7 = new GLAccount
            {
                Name = "COT Income Account 1",
                GLCategory = glCategory3,
                Code = glAccountDAO.GetCode(glCategory3),
                Branch = branchDAO.GetById(1),

            };
            glAccountDAO.InsertWithCommit(account7);
            GLAccount account8 = new GLAccount
            {
                Name = "Interest Expense 1",
                GLCategory = glCategory4,
                Code = glAccountDAO.GetCode(glCategory4),
                Branch = branchDAO.GetById(2),
            };
            glAccountDAO.InsertWithCommit(account8);
            GLAccount account9 = new GLAccount
            {
                Name = "Interest Expense 2",
                GLCategory = glCategory4,
                Code = glAccountDAO.GetCode(glCategory4),
                Branch = branchDAO.GetById(2),
            };
            glAccountDAO.InsertWithCommit(account9);
            
        }
    }
}
