namespace JsonServer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Repository.Pattern.Ef6;

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
