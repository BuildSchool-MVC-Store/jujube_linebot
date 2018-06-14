namespace OSLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shopping_Cart
    {
        [Key]
        public int Shopping_Cart_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Account { get; set; }

        public int Product_ID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public float? Discount { get; set; }

        [Required]
        [StringLength(10)]
        public string size { get; set; }

        [Required]
        [StringLength(50)]
        public string Color { get; set; }

        public virtual Customers Customers { get; set; }

        public virtual Products Products { get; set; }
    }
}
