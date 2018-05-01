namespace JsonServer.Models
{
    using global::Repository.Pattern.Ef6;
    using System.Data.Entity;

    public partial class Appdata : DataContext
    {
        public Appdata()
            : base("name=Appdata")
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<JsonServer.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<JsonServer.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<JsonServer.Models.Work> Works { get; set; }

        public System.Data.Entity.DbSet<JsonServer.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<JsonServer.Models.BaseCode> BaseCodes { get; set; }
        public System.Data.Entity.DbSet<JsonServer.Models.CodeItem> CodeItems { get; set; }
    }
}