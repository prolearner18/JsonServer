
namespace JsonServer.Models
{
    using global::Repository.Pattern.Ef6;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order:Entity
    {
        [StringLength(10)]
        public string Id { get; set; }

        [StringLength(20)]
        public string Orderkey { get; set; }

        [StringLength(30)]
        public string Supplier { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Unitprice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }
    }
}
