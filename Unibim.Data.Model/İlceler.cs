namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class İlceler
    {
        public int id { get; set; }

        [StringLength(255)]
        public string il { get; set; }

        [StringLength(255)]
        public string ilce { get; set; }
    }
}
