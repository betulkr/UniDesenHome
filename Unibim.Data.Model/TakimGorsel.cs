namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TakimGorsel")]
    public partial class TakimGorsel
    {
        public int takimGorselId { get; set; }

        public int? takimId { get; set; }

        [StringLength(50)]
        public string gorsel { get; set; }

        public virtual Takim Takim { get; set; }
    }
}
