namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Takim")]
    public partial class Takim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Takim()
        {
            TakimGorsel = new HashSet<TakimGorsel>();
            TakimSatis = new HashSet<TakimSatis>();
            Urun = new HashSet<Urun>();
        }

        public int takimId { get; set; }

        public int? altkategoriId { get; set; }

        [StringLength(50)]
        public string takimAdi { get; set; }

        [Column(TypeName = "text")]
        public string aciklama { get; set; }

        [StringLength(50)]
        public string aciklamaGorsel { get; set; }

        [StringLength(200)]
        public string aciklamaBaslik { get; set; }

        public double? fiyat { get; set; }

        public virtual Altkategori Altkategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TakimGorsel> TakimGorsel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TakimSatis> TakimSatis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun> Urun { get; set; }
    }
}
