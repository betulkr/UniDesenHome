namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bayi")]
    public partial class Bayi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bayi()
        {
            Siparis = new HashSet<Siparis>();
        }

        public int bayiId { get; set; }

        public int? kullaniciId { get; set; }

        [StringLength(500)]
        public string bayiAd { get; set; }

        [StringLength(50)]
        public string ulke { get; set; }

        [StringLength(50)]
        public string il { get; set; }

        [StringLength(50)]
        public string ilce { get; set; }

        [StringLength(20)]
        public string telefon1 { get; set; }

        [StringLength(20)]
        public string telefon2 { get; set; }

        [StringLength(20)]
        public string fax { get; set; }

        [Column(TypeName = "text")]
        public string adres { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparis> Siparis { get; set; }
    }
}
