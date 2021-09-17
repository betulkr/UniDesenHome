namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Altkategori")]
    public partial class Altkategori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Altkategori()
        {
            Takim = new HashSet<Takim>();
        }

        public int altkategoriId { get; set; }

        public int? kategoriId { get; set; }

        [StringLength(200)]
        public string ad { get; set; }

        [StringLength(50)]
        public string icon { get; set; }

        public virtual Kategori Kategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Takim> Takim { get; set; }
    }
}
