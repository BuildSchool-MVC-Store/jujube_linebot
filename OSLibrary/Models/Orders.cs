namespace OSLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            Order_Details = new HashSet<Order_Details>();
        }

        [Key]
        public int Order_ID { get; set; }

        public DateTime Order_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Account { get; set; }

        [Required]
        [StringLength(10)]
        public string Pay { get; set; }

        [StringLength(10)]
        public string Transport { get; set; }

        [StringLength(10)]
        public string Order_Check { get; set; }

        [Column(TypeName = "money")]
        public decimal? Total { get; set; }

        [Column(TypeName = "money")]
        public decimal? TranMoney { get; set; }

        public virtual Customers Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }
    }
}
