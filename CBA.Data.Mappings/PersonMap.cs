using CBA.Core;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class PersonMap<T> : EntityMap<T> where T:Person
    {
        public PersonMap(){
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
           Map(x => x.OtherNames);
            Map(x => x.Email).Unique().Not.Nullable();
           Map(x => x.PhoneNumber).Not.Nullable();            
        }
        
    }
}
