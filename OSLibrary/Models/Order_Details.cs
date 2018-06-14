namespace OSLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order_Details
    {
        [Key]
        public int Order_Details_ID { get; set; }

        public int Order_ID { get; set; }

        public int Product_ID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public float? Discount { get; set; }

        [Required]
        [StringLength(10)]
        public string size { get; set; }

        [Required]
        [StringLength(50)]
        public string Color { get; set; }

        public virtual Orders Orders { get; set; }

        public virtual Products Products { get; set; }
    }
}
