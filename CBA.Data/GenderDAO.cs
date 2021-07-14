using CBA.Core;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Data
{
    public class GenderDAO : EntityDAO<Gender>
    {

        public Gender GetByName(GenderEnum g)
        {
            Gender gender = Session.QueryOver<Gender>()
                    .Where(x => x.Name == g).SingleOrDefault();
            return gender;
        }

        //public Gender GetByName(GenderEnum g)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
