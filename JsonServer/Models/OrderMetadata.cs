













using System;
using System.ComponentModel.DataAnnotations;

namespace JsonServer.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    public partial class OrderMetadata
    {

        [Required(ErrorMessage = "Please enter : Id")]

        [Display(Name = "Id")]

        public string Id { get; set; }


        [Display(Name = "Orderkey")]

        public string Orderkey { get; set; }


        [Display(Name = "Supplier")]

        public string Supplier { get; set; }


        [Display(Name = "Qty")]

        public decimal Qty { get; set; }


        [Display(Name = "Unitprice")]

        public decimal Unitprice { get; set; }


        [Display(Name = "Amount")]

        public decimal Amount { get; set; }


    }
}
