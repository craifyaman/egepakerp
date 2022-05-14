using EgePakErp.Custom;
using EgePakErp.Helper;
using EgePakErp.Models;
using EgePakErp.Models.Audit;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace EgePakErp.Controllers
{
    public class EntegrasyonController : BaseController
    {
        /// <summary>
        /// excellden cari
        /// </summary>
        /// <returns></returns>
        public void Cari()
        {
            var ulkeler = Db.Ulke.ToList();
            ulkeler.ForEach(f => f.Adi = f.Adi.ToUpper());
            var iller = Db.Il.ToList();
            var ilceler = Db.Ilce.ToList();


            var dataset = new DataSet();
            using (var stream = System.IO.File.Open(@"C:\Users\raify\Downloads\egepakcari.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataset = reader.AsDataSet();
                }
            }

            var dataTable = dataset.Tables[0];
            var list = new List<Cari>();
            for (int i = 1; i <= dataTable.Rows.Count; i++)
            {
                try
                {
                    var cari = new Cari();
                    cari.Kod = dataTable.Rows[i].ItemArray[0].ToString();
                    cari.Unvan = dataTable.Rows[i].ItemArray[1].ToString();
                    var kisi = dataTable.Rows[i].ItemArray[2].ToString();
                    if (!string.IsNullOrEmpty(kisi))
                    {
                        cari.Kisi = new List<Kisi>
                        {

                            new Kisi
                            {
                                AdSoyad=kisi,
                                Aktif=true,
                                Birincil=true,
                                KayitTarihi=DateTime.Now
                            }
                        };
                    }

                    var cadde = dataTable.Rows[i].ItemArray[3].ToString();
                    var sokak = dataTable.Rows[i].ItemArray[4].ToString();
                    cari.Adres = cadde + " " + sokak;

                    var ilce = dataTable.Rows[i].ItemArray[6].ToString();
                    var ilceId = !string.IsNullOrEmpty(ilce) ? ilceler.FirstOrDefault(f => f.Adi.ToLower() == ilce.ToLower())?.IlceId : (int?)null;
                    cari.IlceId = ilceId;
                    var il = dataTable.Rows[i].ItemArray[7].ToString();
                    var ilId = !string.IsNullOrEmpty(il) ? iller.FirstOrDefault(f => f.Adi.ToLower() == il.ToLower())?.IlId : (int?)null;
                    cari.IlId = ilId;
                    var ulke = dataTable.Rows[i].ItemArray[8].ToString().ToUpper();
                    var ulkeId = !string.IsNullOrEmpty(ulke) ? ulkeler.FirstOrDefault(f => f.Adi.Trim() == ulke.Trim())?.UlkeId : (int?)null;
                    cari.UlkeId = ulkeId;

                    cari.Telefon = dataTable.Rows[i].ItemArray[9].ToString().Replace("(", "").Replace(")", "").Replace("+", "").Replace(" ", "");
                    cari.Efatura = Convert.ToBoolean(dataTable.Rows[i].ItemArray[10].ToString());

                    cari.MuhasebeKodu1 = dataTable.Rows[i].ItemArray[14].ToString();
                    cari.MuhasebeKodu2 = dataTable.Rows[i].ItemArray[15].ToString();
                    cari.MuhasebeKodu3 = dataTable.Rows[i].ItemArray[16].ToString();

                    cari.VergiDairesi = dataTable.Rows[i].ItemArray[17].ToString();
                    cari.VergiNumarasi = dataTable.Rows[i].ItemArray[18].ToString();

                    cari.AnaDovizBakiye = Convert.ToDecimal(dataTable.Rows[i].ItemArray[21].ToString());
                    cari.AlternatifDovizBakiye = Convert.ToDecimal(dataTable.Rows[i].ItemArray[22].ToString());

                    cari.Bakiye1 = Convert.ToDecimal(dataTable.Rows[i].ItemArray[23].ToString());

                    if (!string.IsNullOrEmpty(dataTable.Rows[i].ItemArray[24].ToString()))
                    {
                        cari.Doviz1Id = Convert.ToInt32(dataTable.Rows[i].ItemArray[24].ToString());
                    }

                    cari.Bakiye2 = Convert.ToDecimal(dataTable.Rows[i].ItemArray[25].ToString());
                    if (!string.IsNullOrEmpty(dataTable.Rows[i].ItemArray[26].ToString()))
                    {
                        cari.Doviz2Id = Convert.ToInt32(dataTable.Rows[i].ItemArray[26].ToString());
                    }


                    cari.Bakiye3 = Convert.ToDecimal(dataTable.Rows[i].ItemArray[27].ToString());

                    if (!string.IsNullOrEmpty(dataTable.Rows[i].ItemArray[28].ToString()))
                    {
                        cari.Doviz3Id = Convert.ToInt32(dataTable.Rows[i].ItemArray[28].ToString());
                    }


                    cari.BaglantiTipiId = Convert.ToInt32(dataTable.Rows[i].ItemArray[29].ToString());

                    list.Add(cari);

                }
                catch (Exception ex)
                {
                    continue;
                }

            };
            Db.Cari.AddRange(list);
            Db.SaveChanges(1);
        }
        public void UrunCinsi()
        {

            var cins = Db.HamUrunGrup.Where(i => !string.IsNullOrEmpty(i.UrunCinsi)).GroupBy(i => i.UrunCinsi).Select(i => i.FirstOrDefault()).ToList();
            var list = new List<UrunCinsi>();
            foreach (var item in cins)
            {
                list.Add(new Models.UrunCinsi
                {
                    Kisaltmasi = item.UrunCinsi,
                    Adi = item.UrunCinsi
                });
            }

            Db.UrunCinsi.AddRange(list);
            Db.SaveChanges();
        }
        public void Urun()
        {
            var urunCinsleri = Db.UrunCinsi.ToList();
            var urnCins = urunCinsleri.Select(f => f.Kisaltmasi).ToList();

            var urunler = Db.HamUrunGrup
                .Where(i => urnCins.Contains(i.UrunCinsi))
                .Select(i => new
                {
                    i.UrunCinsi,
                    i.UrunNo
                })
                .GroupBy(x => new { x.UrunCinsi, x.UrunNo }, (key, group) => new
                {
                    UrunCinsi = key.UrunCinsi,
                    UrunNo = key.UrunNo
                }).ToList()
                ;

            var list = new List<Urun>();

            foreach (var item in urunler)
            {
                var urun = new Urun();
                urun.UrunCinsiId = urunCinsleri.FirstOrDefault(i => i.Kisaltmasi == item.UrunCinsi).UrunCinsiId;
                urun.UrunNo = item.UrunNo;
                list.Add(urun);
            }

            Db.Urun.AddRange(list);
            Db.SaveChanges();
        }
        public void HammaddeCinsi()
        {
            var cins = Db.HamUrunGrup
                .Where(i => !string.IsNullOrEmpty(i.Hammadde))
                .GroupBy(i => i.Hammadde)
                .Select(i => i.FirstOrDefault().Hammadde)
                .ToList();
            var list = new List<HammaddeCinsi>();
            foreach (var item in cins)
            {
                list.Add(new HammaddeCinsi
                {
                    Kisaltmasi = item,
                    Adi = item
                });
            }

            Db.HammaddeCinsi.AddRange(list);
            Db.SaveChanges();
        }
        public void Kalip()
        {
            try
            {
                var hammaddeCinsleri = Db.HammaddeCinsi.ToList();
                var uretimTeminSekli = Db.UretimTeminSekli.ToList();
                var urunler = Db.Urun.Include("UrunCinsi").ToList();

                var kaliplar = Db.HamUrunGrup
                    .Where(i => i.KalipNo != "00" || i.KalipOzellik != "00")
                    .GroupBy(x => new { x.KalipNo, x.KalipOzellik }, (key, group) => new
                    {
                        KalipNo = key.KalipNo,
                        KalipOzellik = key.KalipOzellik,
                        ParcaAdi = group.FirstOrDefault().ParcaAdi,
                        Hammadde = group.FirstOrDefault().Hammadde,
                        Agirlik = group.FirstOrDefault().Agirlik,
                        TeminŞekli = group.FirstOrDefault().TeminŞekli,
                        KalıpSayisi = group.FirstOrDefault().KalıpSayisi,
                        UretimZamani = group.FirstOrDefault().UretimZamani,
                        Aciklama = group.FirstOrDefault().Aciklama,
                        UrunCinsi = group.FirstOrDefault().UrunCinsi,
                        UrunNo = group.FirstOrDefault().UrunNo,

                    })
                    .ToList();


                var list = new List<Kalip>();

                foreach (var item in kaliplar)
                {
                    var kalip = new Kalip();
                    kalip.Aciklama = item.Aciklama;
                    kalip.Adi = item.ParcaAdi;
                    kalip.KalipGozSayisi = Convert.ToInt32(item.KalıpSayisi);
                    var hammaddeCins = hammaddeCinsleri.FirstOrDefault(i => i.Kisaltmasi == item.Hammadde);
                    if (hammaddeCins != null)
                    {
                        kalip.KalipHammaddeRelation = new List<KalipHammaddeRelation>
                        {
                            new KalipHammaddeRelation
                            {
                                HammaddeCinsiId=hammaddeCins.HammaddeCinsiId
                            }
                        };
                    }
                    kalip.KalipNo = item.KalipNo;
                    kalip.KalipOzellik = item.KalipOzellik;
                    kalip.ParcaAgirlik = Convert.ToDecimal(item.Agirlik);
                    kalip.UretimTeminSekliId = uretimTeminSekli.FirstOrDefault(i => i.Kisaltmasi == item.TeminŞekli).UretimTeminSekliId;
                    kalip.UretimZamani = Convert.ToInt32(item.UretimZamani);



                    list.Add(kalip);
                }

                Db.Kalip.AddRange(list);
                Db.SaveChanges(1);
            }
            catch (Exception ex)
            {

                var a = ex;
            }
        }

        public void KalipUrun()
        {
            try
            {

                var urunler = Db.Urun.Include("UrunCinsi").ToList();
                var kaliplar = Db.Kalip.ToList();

                var list = Db.HamUrunGrup
                   
                    .Where(i => !string.IsNullOrEmpty(i.UrunCinsi))
                    .GroupBy(x => new { x.UrunCinsi, x.UrunNo }, (key, group) => new
                    {
                         
                        UrunCinsi = key.UrunCinsi,
                        UrunNo = key.UrunNo,
                        Kaliplar = group

                    })
                    .ToList();

                var relations = new List<KalipUrunRelation>();
                foreach (var item in list)
                {
                    var urun = urunler.FirstOrDefault(f => f.UrunCinsi.Kisaltmasi == item.UrunCinsi && f.UrunNo == item.UrunNo);
                    if (urun == null) continue;
                    foreach (var k in item.Kaliplar)
                    {
                        var kalip = kaliplar.FirstOrDefault(i => i.KalipNo == k.KalipNo && i.KalipOzellik == k.KalipOzellik);
                        if (kalip == null) continue;

                        relations.Add(new KalipUrunRelation
                        {
                            KalipId = kalip.KalipId,
                            UrunId = urun.UrunId
                        });
                    }
                }

                Db.KalipUrunRelation.AddRange(relations);
                Db.SaveChanges(1);
            }
            catch (Exception ex)
            {

                var a = ex;
            }
        }


        //TEST

    }
}