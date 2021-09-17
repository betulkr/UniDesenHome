namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Urun")]
    public partial class Urun
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urun()
        {
            SepetUrun = new HashSet<SepetUrun>();
            UrunGorsel = new HashSet<UrunGorsel>();
            UrunOzellik = new HashSet<UrunOzellik>();
        }

        public int urunId { get; set; }

        public int? takimId { get; set; }

        public int? grupId { get; set; }

        [StringLength(200)]
        public string ad { get; set; }

        public double? fiyat { get; set; }

        public double? yükseklik { get; set; }

        public double? genişlik { get; set; }

        public double? derinlik { get; set; }

        public int? adet { get; set; }

        [Column(TypeName = "text")]
        public string aciklama { get; set; }

        public virtual Grup Grup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SepetUrun> SepetUrun { get; set; }

        public virtual Takim Takim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunGorsel> UrunGorsel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunOzellik> UrunOzellik { get; set; }
    }
}
