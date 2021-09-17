namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iller")]
    public partial class iller
    {
        public int id { get; set; }

        [StringLength(255)]
        public string sehir { get; set; }
    }
}
