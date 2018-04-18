namespace JsonServer.Models
{
    using global::Repository.Pattern.Ef6;
    using System.Data.Entity;

    public partial class Appdata : DbContext
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
                .Property(e => e.orderkey)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .Property(e => e.supplier)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .Property(e => e.qty)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.unitprice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.amount)
                .HasPrecision(18, 0);
        }
    }
}
