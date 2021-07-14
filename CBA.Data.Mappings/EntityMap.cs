using CBA.Core;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.Mappings
{
    public class EntityMap<T> : ClassMap<T> where T : Entity
    {
        public EntityMap()
        {
            LazyLoad();
             
            Id(x => x.ID).GeneratedBy.Identity();
            Map(x => x.DateCreated).Default("Getdate()").Not.Update();
            Map(x => x.DateModified).Default("Getdate()");
        }
        
    }
}
