namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Musteri")]
    public partial class Musteri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Musteri()
        {
            Siparis = new HashSet<Siparis>();
        }

        public int musteriId { get; set; }

        [StringLength(11)]
        public string tc { get; set; }

        [StringLength(100)]
        public string adSoyad { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? doğumTarihi { get; set; }

        [StringLength(10)]
        public string cinsiyet { get; set; }

        public int? cocuk { get; set; }

        [StringLength(100)]
        public string eposta { get; set; }

        [StringLength(100)]
        public string meslek { get; set; }

        [StringLength(100)]
        public string eğitimDurumu { get; set; }

        [StringLength(10)]
        public string medeniHal { get; set; }

        [StringLength(11)]
        public string cep1 { get; set; }

        [StringLength(11)]
        public string cep2 { get; set; }

        [StringLength(11)]
        public string evtel { get; set; }

        [StringLength(11)]
        public string istel { get; set; }

        [StringLength(500)]
        public string teslimAdresi { get; set; }

        [StringLength(50)]
        public string il { get; set; }

        [StringLength(50)]
        public string ilce { get; set; }

        [StringLength(50)]
        public string semt { get; set; }

        [StringLength(200)]
        public string isAdres { get; set; }

        public int? postaKodu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparis> Siparis { get; set; }
    }
}
