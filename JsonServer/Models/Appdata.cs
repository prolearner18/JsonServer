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

        public System.Data.Entity.DbSet<Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Work> Works { get; set; }

        public System.Data.Entity.DbSet<Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<BaseCode> BaseCodes { get; set; }
        public System.Data.Entity.DbSet<CodeItem> CodeItems { get; set; }
    }
}