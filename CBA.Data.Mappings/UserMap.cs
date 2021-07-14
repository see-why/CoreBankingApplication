using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using CBA.Core;


namespace CBA.Mappings
{
    //public class UserMap : PersonMap<User>
    //{
    //    public UserMap()
    //    {
    //        Map(x => x.UserName).Unique().Not.Nullable();
    //        Map(x => x.Password).Not.Nullable();
    //        References(x => x.Branch);
    //        Map(x => x.IsSuperAdmin);
    //        Map(x => x.DateCreated);
    //        Map(x => x.DateModified);
    //    }
    //}
    public class UserMap :ClassMap<User>
    {
        public UserMap()
        {
            Table("AspNetUsers");
            SchemaAction.None();
            Id(x => x.Id);
            Map(x => x.UserName).Unique().Not.Nullable();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.OtherNames);
            Map(x => x.Email).Unique().Not.Nullable();
            Map(x => x.PhoneNumber).Not.Nullable();
            References(x => x.Branch).Column("BranchId");
            Map(x => x.IsSuperAdmin);        
        }
    }
}
