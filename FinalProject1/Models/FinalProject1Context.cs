using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FinalProject1.Models
{
    public class FinalProject1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public FinalProject1Context() : base("name=FinalProject1Context")
        {
        }

        public System.Data.Entity.DbSet<FinalProject1.Models.Site> Sites { get; set; }
    
    }
}
