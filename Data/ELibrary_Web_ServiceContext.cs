using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ELibrary_Web_Service.Data
{
    public class ELibrary_Web_ServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ELibrary_Web_ServiceContext() : base("name=ELibrary_Web_ServiceContext")
        {
        }

        public System.Data.Entity.DbSet<ELibrary_Web_Service.Models.Feed> Feeds { get; set; }
    }
}
