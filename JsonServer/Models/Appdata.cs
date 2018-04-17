namespace JsonServer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Repository.Pattern.Ef6;
    //xushanshan extend from unitofwork
    public partial class Appdata : DataContext
    {
        public Appdata()
            : base("name=Appdata")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
