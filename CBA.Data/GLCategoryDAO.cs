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
    public class GLCategoryDAO : EntityDAO<GLCategory>
    {
        GLCategory category = new GLCategory();
        
        public bool CheckName(string name)
        {                       
            category = Session.QueryOver<GLCategory>()
                .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Exact)).SingleOrDefault();
            return (category != null);
        }
        //Get GLCategory by name
        public GLCategory GetGLCategoryByName(string name)
        {                        
            category = Session.QueryOver<GLCategory>()
                .Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Exact)).SingleOrDefault();
            return category;
        }

        //Get GLCategory by Main Account category
        public IList<GLCategory> GetGLCategoriesByMainAccount(AccountCategory cat)
        {
            IList<GLCategory> categories = new List<GLCategory>();           
            categories = Session.QueryOver<GLCategory>()
                .Where(x => x.MainCategory.Name == cat).List();
            return categories;
        }

        
    }
}
