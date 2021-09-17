namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SepetUrunOzellik")]
    public partial class SepetUrunOzellik
    {
        [Key]
        [Column("sepetUrunOzellik")]
        public int sepetUrunOzellik1 { get; set; }

        public int? sepetUrunId { get; set; }

        public int? urunOzellikId { get; set; }

        public virtual SepetUrun SepetUrun { get; set; }

        public virtual UrunOzellik UrunOzellik { get; set; }
    }
}
