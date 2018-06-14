namespace OSLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stock")]
    public partial class Stock
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Product_ID { get; set; }

        public int? Quantity { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Size { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Color { get; set; }

        public virtual Products Products { get; set; }
    }
}
