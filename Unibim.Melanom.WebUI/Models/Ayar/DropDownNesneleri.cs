using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unibim.Data.Model;
using Unibim.Melanom.WebUI.Models.Connection;

namespace Unibim.Melanom.WebUI
{
    public class DropDownNesneleri
    {
     
        public SelectList Il(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.iller.AsNoTracking().Select(y => new
                {
                    il = y.sehir
                }), "il", "il");
            }
            else
            {
                return new SelectList(DbModel.EF.iller.AsNoTracking().Select(y => new
                {
                    il = y.sehir
                }), "il", "il", no);
            }
        }


        public SelectList Kategori(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.Kategori.AsNoTracking().Select(y => new
                {
                    y.kategoriId,
                    ad = y.ad
                }), "kategoriId", "ad");
            }
            else
            {
                return new SelectList(DbModel.EF.Kategori.AsNoTracking().Select(y => new
                {
                    y.kategoriId,
                    il = y.ad
                }), "kategoriId", "ad", no);
            }
        }

        public SelectList Altkategori(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.Altkategori.AsNoTracking().Select(y => new
                {
                    y.altkategoriId,
                    ad = y.ad
                }), "altkategoriId", "ad");
            }
            else
            {
                return new SelectList(DbModel.EF.Altkategori.AsNoTracking().Select(y => new
                {
                    y.altkategoriId,
                    il = y.ad
                }), "altkategoriId", "ad", no);
            }
        }


        public SelectList Takim(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.Takim.AsNoTracking().Select(y => new
                {
                    y.takimId,
                    takimAdi = y.takimAdi
                }), "takimId", "takimAdi");
            }
            else
            {
                return new SelectList(DbModel.EF.Takim.AsNoTracking().Select(y => new
                {
                    y.takimId,
                    il = y.takimAdi
                }), "takimId", "takimAdi", no);
            }
        }

        public SelectList OzellikTur(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.OzellikTur.AsNoTracking().Select(y => new
                {
                    y.ozellikTurId,
                    ad = y.ad
                }), "ozellikTurId", "ad");
            }
            else
            {
                return new SelectList(DbModel.EF.OzellikTur.AsNoTracking().Select(y => new
                {
                    y.ozellikTurId,
                    ad = y.ad
                }), "ozellikTurId", "ad", no);
            }
        }

        public SelectList Ozellik(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.Ozellik.AsNoTracking().Select(y => new
                {
                    y.ozellikId,
                    ad = y.kod +" "+y.ad
                }), "ozellikId", "ad");
            }
            else
            {
                return new SelectList(DbModel.EF.Ozellik.AsNoTracking().Select(y => new
                {
                    y.ozellikId,
                    ad = y.kod + " " + y.ad
                }), "ozellikId", "ad", no);
            }
        }

        public SelectList Grup(int? no)
        {
            if (no == null)
            {
                return new SelectList(DbModel.EF.Grup.AsNoTracking().Select(y => new
                {
                    y.grupId,
                    ad = y.ad
                }), "grupId", "ad");
            }
            else
            {
                return new SelectList(DbModel.EF.Grup.AsNoTracking().Select(y => new
                {
                    y.grupId,
                    ad = y.ad
                }), "grupId", "ad", no);
            }
        }
    }
}