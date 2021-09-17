namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunGorsel")]
    public partial class UrunGorsel
    {
        public int urunGorselId { get; set; }

        public int? urunId { get; set; }

        [StringLength(50)]
        public string gorsel { get; set; }

        public virtual Urun Urun { get; set; }
    }
}
