namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TakimSatis
    {
        public int takimSatisId { get; set; }

        public int? takimId { get; set; }

        public int? satisSekilId { get; set; }

        public virtual SatisSekil SatisSekil { get; set; }

        public virtual Takim Takim { get; set; }
    }
}
