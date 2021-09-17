namespace Unibim.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KullaniciYetki")]
    public partial class KullaniciYetki
    {
        public int kullaniciYetkiId { get; set; }

        public int? kullaniciId { get; set; }

        public int? yetkiId { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Yetki Yetki { get; set; }
    }
}
