
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
        public string orderkey { get; set; }

        [StringLength(30)]
        public string supplier { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitprice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? amount { get; set; }
    }
}
