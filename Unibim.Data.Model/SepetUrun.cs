namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SepetUrun")]
    public partial class SepetUrun
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SepetUrun()
        {
            SepetUrunOzellik = new HashSet<SepetUrunOzellik>();
        }

        public int sepetUrunId { get; set; }

        public int? sepetId { get; set; }

        public int? urunId { get; set; }

        public int? adet { get; set; }

        public int? indirimOranı { get; set; }

        public double? indirimliFiyat { get; set; }

        public virtual Sepet Sepet { get; set; }

        public virtual Urun Urun { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SepetUrunOzellik> SepetUrunOzellik { get; set; }
    }
}
