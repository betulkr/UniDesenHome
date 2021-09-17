namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ozellik")]
    public partial class Ozellik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ozellik()
        {
            UrunOzellik = new HashSet<UrunOzellik>();
        }

        public int ozellikId { get; set; }

        public int? ozellikTurId { get; set; }

        [StringLength(200)]
        public string ad { get; set; }

        [StringLength(50)]
        public string kod { get; set; }

        public virtual OzellikTur OzellikTur { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunOzellik> UrunOzellik { get; set; }
    }
}
