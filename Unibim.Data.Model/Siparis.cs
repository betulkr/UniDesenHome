namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Siparis
    {
        public int siparisId { get; set; }

        public int? bayiId { get; set; }

        public int? musteriId { get; set; }

        public int? sepetId { get; set; }

        [StringLength(50)]
        public string siparisNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? siparisTarihi { get; set; }

        public bool? onaylandiMi { get; set; }

        [StringLength(50)]
        public string durum { get; set; }

        public double? tutar { get; set; }

        public virtual Bayi Bayi { get; set; }

        public virtual Musteri Musteri { get; set; }

        public virtual Sepet Sepet { get; set; }
    }
}
