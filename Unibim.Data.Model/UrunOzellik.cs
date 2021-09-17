namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunOzellik")]
    public partial class UrunOzellik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UrunOzellik()
        {
            SepetUrunOzellik = new HashSet<SepetUrunOzellik>();
        }

        public int urunOzellikId { get; set; }

        public int? urunId { get; set; }

        public int? ozellikId { get; set; }

        public virtual Ozellik Ozellik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SepetUrunOzellik> SepetUrunOzellik { get; set; }

        public virtual Urun Urun { get; set; }
    }
}
