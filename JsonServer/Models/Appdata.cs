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

        public virtual DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .Property(e => e.Orderkey)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .Property(e => e.Supplier)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .Property(e => e.Qty)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.Unitprice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);
        }
    }
}
