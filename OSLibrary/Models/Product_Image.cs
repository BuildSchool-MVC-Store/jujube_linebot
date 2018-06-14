namespace OSLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Image
    {
        [Key]
        public int Product_Image_ID { get; set; }

        public int Product_ID { get; set; }

        [StringLength(5)]
        public string Image_Only { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public virtual Products Products { get; set; }
    }
}
