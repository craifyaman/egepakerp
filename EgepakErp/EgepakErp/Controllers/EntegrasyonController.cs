using EgepakErp.Helper;
using EgepakErp.Models.Custom;
using EgePakErp.Custom;
using EgePakErp.Helper;
using EgePakErp.Models;
using EgePakErp.Models.Audit;
using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Data.Entity;
using EgepakErp.Enums;

namespace EgePakErp.Controllers
{
    public class EntegrasyonController : BaseController
    {
        public CultureInfo culture { get; set; }
        public EntegrasyonController()
        {
            culture = new CultureInfo("tr-TR");
        }
        /// <summary>
        /// excellden cari
        /// </summary>
        /// <returns></returns>
        /// 

        //[Menu("Entegrasyon", "flaticon-squares icon-xl", "Entegrasyon", 0, 0)]
        public ActionResult Index()
        {
            return View();
        }
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
        public void HamUrunGroup()
        {
            var dataset = new DataSet();
            using (var stream = System.IO.File.Open(@"C:\Users\fika yazılım\Downloads\EgepakAktarim\Ham_Urun_Group_Aktarim_11_08_2022_3.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataset = reader.AsDataSet();
                }
            }
            var dataTable = dataset.Tables[0];
            var list = new List<HamUrunGrup>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var currentRow = dataTable.Rows[i];
                try
                {
                    var UrunGroup = new HamUrunGrup();
                    UrunGroup.UrunCinsi = currentRow.ItemArray[0]?.ToString();
                    UrunGroup.UrunNo = currentRow.ItemArray[1]?.ToString();
                    UrunGroup.UrunKodu = UrunGroup.UrunCinsi + UrunGroup.UrunNo;
                    UrunGroup.KalipNo = currentRow.ItemArray[2]?.ToString();
                    UrunGroup.KalipOzellik = currentRow.ItemArray[3]?.ToString();
                    UrunGroup.KalipKodu = currentRow.ItemArray[4]?.ToString();
                    UrunGroup.ParcaAdi = currentRow.ItemArray[5]?.ToString();
                    UrunGroup.Hammadde = currentRow.ItemArray[6]?.ToString();
                    UrunGroup.Agirlik = currentRow.ItemArray[7]?.ToString();
                    UrunGroup.YollukTip = currentRow.ItemArray[8]?.ToString();
                    UrunGroup.YollukAgirlik = currentRow.ItemArray[9]?.ToString();
                    UrunGroup.TeminSekli = currentRow.ItemArray[10]?.ToString();
                    UrunGroup.KalipSayisi = currentRow.ItemArray[11]?.ToString();
                    UrunGroup.UretimZamani = currentRow.ItemArray[12]?.ToString();
                    UrunGroup.Aciklama = currentRow.ItemArray[13]?.ToString();
                    UrunGroup.Yaldiz = currentRow.ItemArray[14]?.ToString();
                    UrunGroup.KoliIciAdet = currentRow.ItemArray[15]?.ToString();
                    UrunGroup.HammaddeFormul = currentRow.ItemArray[16]?.ToString();
                    UrunGroup.SicakBaskiAdet = currentRow.ItemArray[17]?.ToString();
                    UrunGroup.EgePakMontajAdet = currentRow.ItemArray[18]?.ToString();
                    UrunGroup.EvMontajMaliyet = currentRow.ItemArray[19]?.ToString();
                    UrunGroup.KromPlastMetalizeBrFiyat = currentRow.ItemArray[20]?.ToString();

                    list.Add(UrunGroup);
                }
                catch (Exception ex)
                {

                }

            }
            Db.HamUrunGrup.AddRange(list);
            Db.BulkSaveChanges();

        }
        public void YanSanayiStok()
        {
            var dataset = new DataSet();
            using (var stream = System.IO.File.Open(@"C:\Users\fika yazılım\Downloads\EgepakAktarim\Yan_Sanayi_Stok_Aktarim_10_08_2022_1.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataset = reader.AsDataSet();
                }
            }
            var dataTable = dataset.Tables[0];
            var list = new List<YanSanayiStok>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var currentRow = dataTable.Rows[i];
                var dovizListe = Db.Doviz.ToList();
                var birimListe = Db.TableHammaddeBirim.ToList();
                string birimFiyat = "";
                string doviz = "";
                string birim = "";
                try
                {
                    var stok = new YanSanayiStok();
                    stok.UrunAdi = currentRow.ItemArray[0].ToString().Trim();
                    stok.Kod = currentRow.ItemArray[1].ToString().Trim();
                    stok.YanMamul = currentRow.ItemArray[2].ToString().Trim();

                    try
                    {
                        birimFiyat = currentRow.ItemArray[3].ToString().Trim();
                        stok.BirimFiyat = Convert.ToDecimal(birimFiyat);
                    }
                    catch
                    {

                    }
                    try
                    {
                        doviz = currentRow.ItemArray[4].ToString().Trim();
                        stok.DovizId = dovizListe.FirstOrDefault(x => x.Kisaltma == doviz).DovizId;

                    }
                    catch
                    {

                    }
                    try
                    {
                        birim = currentRow.ItemArray[5].ToString().Trim();
                        stok.TableHammaddeBirimId = birimListe.FirstOrDefault(x => x.Birimi == birim).TableHammaddeBirimId;

                    }
                    catch
                    {

                    }



                    list.Add(stok);
                }
                catch (Exception ex)
                {

                }

            }
            Db.YanSanayiStok.AddRange(list);
            Db.BulkSaveChanges();
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
            Db.BulkSaveChanges();
        }
        public void Urun()
        {
            var urunCinsleri = Db.UrunCinsi.ToList();
            var urnCins = urunCinsleri.Select(f => f.Kisaltmasi).ToList();
            var YanSanayiStok = Db.YanSanayiStok.FirstOrDefault(x => x.YanMamul == "PİM");
            var PimUrunListe = YanSanayiStok.UrunAdi.Split(',');

            var YanSanayiTutkal = Db.YanSanayiStok.FirstOrDefault(x => x.YanMamul == "TUTKAL");
            var TutkalUrunListe = YanSanayiTutkal.UrunAdi.Split(',');


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
                try
                {

                    var urun = new Urun();
                    var urunCinsi = urunCinsleri.FirstOrDefault(i => i.Kisaltmasi == item.UrunCinsi);
                    urun.UrunCinsiId = urunCinsi.UrunCinsiId;
                    urun.UrunNo = item.UrunNo;
                    urun.isAktif = true;
                    if (PimUrunListe.Contains(urunCinsi.Kisaltmasi + urun.UrunNo))
                    {
                        urun.isPimUsed = true;
                    }
                    if (TutkalUrunListe.Contains(urunCinsi.Kisaltmasi + urun.UrunNo))
                    {
                        urun.isTutkalUsed = true;
                    }

                    list.Add(urun);
                }
                catch (Exception ex)
                {

                }
            }

            Db.Urun.AddRange(list);
            Db.BulkSaveChanges();
        }
        public void HammaddeCinsi()
        {
            var cins = Db.HamUrunGrup
                .Where(i => !string.IsNullOrEmpty(i.Hammadde))
                .GroupBy(i => i.Hammadde)
                .Select(i => i.FirstOrDefault().Hammadde)
                .ToList();
            var hammaddeBirimList = Db.TableHammaddeBirim.ToList();
            int birimId = 1;
            var list = new List<HammaddeCinsi>();
            foreach (var item in cins)
            {
                list.Add(new HammaddeCinsi
                {
                    Kisaltmasi = item,
                    Adi = item,
                    isAktive = true,
                    TableHammaddeBirimId = birimId
                });
            }

            Db.HammaddeCinsi.AddRange(list);
            Db.BulkSaveChanges();
        }

        public void HammaddeCinsiFire()
        {
            var HammaddeCinsListe = Db.HammaddeCinsi.ToList();

            var dataset = new DataSet();
            using (var stream = System.IO.File.Open(@"C:\Users\fika yazılım\Downloads\EgepakAktarim\Hammadde_ilave_fire_yuzdeleri.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataset = reader.AsDataSet();
                }
            }

            var dataTable = dataset.Tables[0];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var currentRow = dataTable.Rows[i];
                try
                {
                    var HammaddeCins = currentRow.ItemArray[0].ToString();
                    var Oran = currentRow.ItemArray[1].ToString();

                    var fire = new HammaddeFire();
                    fire.Oran = Convert.ToDouble(Oran);
                    Db.HammaddeFire.Add(fire);
                    Db.SaveChanges();

                    var Hammadde = HammaddeCinsListe.FirstOrDefault(x => x.Adi == HammaddeCins);
                    if (Hammadde != null)
                    {
                        Hammadde.HammaddeFireId = fire.HammaddeFireId;
                    }
                    else
                    {
                        var HammaddeCinsi = new HammaddeCinsi(HammaddeCins, HammaddeCins, HammaddeCins, true);
                        HammaddeCinsi.HammaddeFireId = fire.HammaddeFireId;
                        HammaddeCinsi.TableHammaddeBirimId = 1;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            Db.BulkSaveChanges();
        }
        public void Kalip()
        {
            try
            {
                var hammaddeCinsleri = Db.HammaddeCinsi.ToList();
                var uretimTeminSekli = Db.UretimTeminSekli.ToList();
                var HazirMalzemeListe = Db.Fiyat.ToList();

                var kaliplar = Db.HamUrunGrup
                    .Where(i => i.KalipNo != "00")
                    .GroupBy(x => x.KalipKodu).ToList()
                    .Select(x => new
                    {
                        KalipNo = x.FirstOrDefault().KalipNo,
                        KalipOzellik = x.FirstOrDefault().KalipOzellik,
                        ParcaAdi = x.FirstOrDefault().ParcaAdi,
                        Hammadde = x.FirstOrDefault().Hammadde,
                        Agirlik = x.FirstOrDefault().Agirlik,
                        TeminŞekli = x.FirstOrDefault().TeminSekli,
                        KalıpSayisi = x.FirstOrDefault().KalipSayisi,
                        UretimZamani = x.FirstOrDefault().UretimZamani,
                        Aciklama = x.FirstOrDefault().Aciklama,
                        UrunCinsi = x.FirstOrDefault().UrunCinsi,
                        UrunNo = x.FirstOrDefault().UrunNo,
                        KalipEtiket = x.FirstOrDefault().KalipEtiket,
                        HammaddeFormul = x.FirstOrDefault().HammaddeFormul,
                        YollukTip = x.FirstOrDefault().YollukTip,
                        YollukAgirlik = x.FirstOrDefault().YollukAgirlik,
                        KalipKod = x.FirstOrDefault().KalipKodu,
                        Yaldiz = x.FirstOrDefault().Yaldiz,
                        KoliIciAdet = x.FirstOrDefault().KoliIciAdet,
                        SicakBaskiAdet = x.FirstOrDefault().SicakBaskiAdet,
                        EgePakMontajAdet = x.FirstOrDefault().EgePakMontajAdet,
                        EvMontajMaliyet = x.FirstOrDefault().EvMontajMaliyet,
                        KromPlastMetalizeBrFiyat = x.FirstOrDefault().KromPlastMetalizeBrFiyat

                    })
                    .ToList();


                var list = new List<Kalip>();

                foreach (var item in kaliplar)
                {
                    var kalip = new Kalip();
                    kalip.Aciklama = item.Aciklama;
                    kalip.Adi = item.ParcaAdi;
                    kalip.isAktive = true;

                    try
                    {
                        kalip.KalipGozSayisi = Convert.ToInt32(item.KalıpSayisi);
                    }
                    catch
                    {
                        kalip.KalipGozSayisi = 0;
                    }
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

                    try
                    {
                        kalip.ParcaAgirlik = Convert.ToDecimal(item.Agirlik, culture);
                    }
                    catch
                    {
                        kalip.ParcaAgirlik = 0;
                    }
                    //kalip.UretimTeminSekliId = uretimTeminSekli.FirstOrDefault(i => i.Kisaltmasi == item.TeminŞekli).UretimTeminSekliId;
                    try
                    {
                        kalip.UretimZamani = Convert.ToInt32(item.UretimZamani);
                    }
                    catch
                    {
                        kalip.UretimZamani = 0;
                    }
                    //kalıp etiket eşleştirme
                    try
                    {
                        kalip.KalipEtiket = item.KalipEtiket;
                    }
                    catch
                    {
                    }
                    //hammadde formül eşleştirme
                    try
                    {
                        kalip.HammaddeFormul = item.HammaddeFormul;
                    }
                    catch
                    {
                    }

                    //Yolluk tip eşleştirme
                    try
                    {
                        kalip.YollukTipi = item.YollukTip;
                    }
                    catch
                    {
                    }
                    //Yolluk Ağırlık eşleştirme
                    try
                    {
                        kalip.YollukAgirlik = item.YollukAgirlik;
                    }
                    catch
                    {
                    }

                    //Kalıp Kodu eşleştirme
                    try
                    {
                        kalip.ParcaKodu = item.KalipKod;
                    }
                    catch
                    {
                    }
                    //Uretim temin şekli eşleştirme
                    try
                    {
                        kalip.UretimTeminSekliId = uretimTeminSekli.FirstOrDefault(x => x.Kisaltmasi == item.TeminŞekli).UretimTeminSekliId;
                    }
                    catch
                    {
                    }
                    //Yaldiz eşleştirme
                    try
                    {
                        kalip.Yaldiz = item.Yaldiz;
                    }
                    catch
                    {
                    }
                    //Koli içi adet eşleştirme
                    try
                    {
                        kalip.KoliIciAdet = item.KoliIciAdet;
                    }
                    catch
                    {
                    }

                    //Hazır Malzememi bulma
                    try
                    {
                        var malzeme = HazirMalzemeListe.FirstOrDefault(x => x.Kod == item.KalipKod);
                        if (malzeme != null)
                        {
                            kalip.isHazirMalzeme = true;
                        }
                        else
                        {
                            kalip.isHazirMalzeme = false;
                        }
                    }
                    catch
                    {
                    }

                    //SicakBaskiAdet eşleştirme
                    try
                    {
                        kalip.SicakBaskiAdet = item.SicakBaskiAdet;
                    }
                    catch
                    {
                    }

                    //EgePakMontajAdet eşleştirme
                    try
                    {
                        kalip.EgePakMontajAdet = item.EgePakMontajAdet;
                    }
                    catch
                    {
                    }

                    //EvMontajMaliyet eşleştirme
                    try
                    {
                        kalip.EvMontajMaliyet = item.EvMontajMaliyet;
                    }
                    catch
                    {
                    }
                    //KromPlastMetalizeBrFiyat eşleştirme
                    try
                    {
                        kalip.KromPlastMetalizeBrFiyat = item.KromPlastMetalizeBrFiyat;
                    }
                    catch
                    {
                    }
                    list.Add(kalip);
                }

                Db.Kalip.AddRange(list);
                Db.BulkSaveChanges();
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

                var HamUrunListe = Db.HamUrunGrup

                    .Where(i => !string.IsNullOrEmpty(i.UrunCinsi))
                    .GroupBy(x => new { x.UrunCinsi, x.UrunNo }, (key, group) => new
                    {

                        UrunCinsi = key.UrunCinsi,
                        UrunNo = key.UrunNo,
                        Kaliplar = group
                    })
                    .ToList();

                var relations = new List<KalipUrunRelation>();

                foreach (var item in HamUrunListe)
                {
                    var urunKod = item.UrunCinsi + item.UrunNo;
                    var urun = urunler.FirstOrDefault(f => f.UrunCinsi.Kisaltmasi == item.UrunCinsi && f.UrunNo == item.UrunNo);
                    if (urun == null) continue;
                    foreach (var k in item.Kaliplar)
                    {
                        //var kalip = kaliplar.FirstOrDefault(i => i.KalipNo == k.KalipNo && i.Adi == k.ParcaAdi);
                        var kalip = kaliplar.FirstOrDefault(i => i.ParcaKodu == k.KalipKodu);
                        if (kalip == null) continue;
                        relations.Add(new KalipUrunRelation
                        {
                            KalipId = kalip.KalipId,
                            UrunId = urun.UrunId
                        });
                    }
                }

                Db.KalipUrunRelation.AddRange(relations);
                Db.BulkSaveChanges();
            }
            catch (Exception ex)
            {

                var a = ex;
            }
        }

        public void HammaddeHareket()
        {
            var HammaddeCinsleri = Db.HammaddeCinsi.ToList();
            var Cariler = Db.Cari.Include("BaglantiTipi").ToList();
            var Dovizler = Db.Doviz.ToList();
            var Birimler = Db.TableHammaddeBirim.ToList();
            var Kategoriler = Db.Kategori.ToList();

            var dataset = new DataSet();
            using (var stream = System.IO.File.Open(@"C:\Users\fika yazılım\Downloads\EgepakAktarim\Hammadde_Hareket_Aktarim_11_08_2022_2.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataset = reader.AsDataSet();
                }
            }
            var dataTable = dataset.Tables[0];
            var list = new List<HammaddeHareket>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var currentRow = dataTable.Rows[i];
                try
                {
                    var hareket = new HammaddeHareket();
                    hareket.FaturaNo = currentRow.ItemArray[0].ToString();
                    hareket.KayitTarihi = (DateTime)currentRow.ItemArray[1];
                    hareket.HammaddeGirisTarihi = (DateTime)currentRow.ItemArray[1];
                    hareket.Miktar = Convert.ToDecimal(currentRow.ItemArray[9]);
                    hareket.UrunAdi = currentRow.ItemArray[5].ToString();

                    //tedarikci eşleştirmesi
                    string cariUnvan = currentRow.ItemArray[4].ToString().ToLower();
                    try
                    {
                        hareket.TedarikciId = Cariler.Where(x => x.Unvan.ToLower().Contains(cariUnvan)).FirstOrDefault().CariId;
                    }
                    catch (Exception ex)
                    {

                    }
                    //tedarikci eşleştirmesi son

                    //Kategori eşleştirmesi
                    string kategori = currentRow.ItemArray[2].ToString().Trim().ToLower();
                    try
                    {
                        var kat = Kategoriler.FirstOrDefault(x => x.Adi == kategori);
                        if (kat != null)
                        {
                            hareket.KategoriId = kat.KategoriId;
                        }
                        else
                        {
                            Kategori _kategori = new Kategori();
                            _kategori.Adi = kategori;
                            Db.Kategori.Add(_kategori);
                            Db.SaveChanges();
                            Kategoriler.Add(_kategori);
                            hareket.KategoriId = _kategori.KategoriId;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    //Kategori eşleştirmesi son

                    //hammadde birimi eşleştirmesi
                    try
                    {
                        int BirimId = Birimler.FirstOrDefault(x => x.Birimi == currentRow.ItemArray[10].ToString()).TableHammaddeBirimId;
                        hareket.TableHammaddeBirimId = BirimId;
                    }
                    catch (Exception ex)
                    {
                        TableHammaddeBirim hBirim = new TableHammaddeBirim();
                        string _birim = currentRow.ItemArray[10].ToString();
                        hBirim.Birimi = _birim;

                        Db.TableHammaddeBirim.Add(hBirim);
                        Db.SaveChanges();
                        Birimler.Add(hBirim);
                        hareket.TableHammaddeBirimId = hBirim.TableHammaddeBirimId;

                    }
                    //hammadde birimi eşleştirmesi son

                    //hammaddecins eşleştirmesi
                    try
                    {
                        string cins = currentRow.ItemArray[3].ToString();
                        var hammaddeCins = HammaddeCinsleri.FirstOrDefault(x => x.Kisaltmasi.ToLower() == cins.ToLower());
                        if (hammaddeCins != null)
                        {
                            hareket.HammaddeCinsiId = hammaddeCins.HammaddeCinsiId;
                        }
                        else
                        {
                            HammaddeCinsi _cins = new HammaddeCinsi();
                            _cins.Aciklamasi = cins;
                            _cins.Adi = cins;
                            _cins.Kisaltmasi = cins;
                            _cins.isAktive = true;
                            _cins.TableHammaddeBirimId = hareket.TableHammaddeBirimId;
                            Db.HammaddeCinsi.Add(_cins);
                            Db.SaveChanges();
                            HammaddeCinsleri.Add(_cins);
                            hareket.HammaddeCinsiId = _cins.HammaddeCinsiId;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    //hammaddecins eşleştirmesi son



                    hareket.BirimFiyat = Convert.ToDecimal(currentRow.ItemArray[6]);
                    // doviz eşleştirmesi
                    string para = currentRow.ItemArray[7].ToString();
                    if (para != "TL")
                    {
                        if (para == "USD")
                        {
                            hareket.DovizId = 2;
                            try
                            {
                                if (string.IsNullOrEmpty(currentRow.ItemArray[8].ToString()))
                                {
                                    hareket.DolarKuru = DovizHelper.DovizKuruGetir("USD", hareket.HammaddeGirisTarihi);
                                }
                                else
                                {
                                    hareket.DolarKuru = Convert.ToDecimal(currentRow.ItemArray[8]);
                                }
                                hareket.BirimFiyat = hareket.BirimFiyat * hareket.DolarKuru.Value;
                            }
                            catch (Exception ex)
                            {

                            }

                        }

                        else if (para == "EUR")
                        {
                            try
                            {
                                hareket.DovizId = 3;
                                if (currentRow.ItemArray[8].ToString() == "")
                                {
                                    hareket.EuroKuru = DovizHelper.DovizKuruGetir("EUR", hareket.HammaddeGirisTarihi);
                                }
                                else
                                {
                                    hareket.EuroKuru = Convert.ToDecimal(currentRow.ItemArray[8]);
                                }

                                hareket.BirimFiyat = hareket.BirimFiyat * hareket.EuroKuru.Value;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                        hareket.DovizId = 1;

                    hareket.ToplamTutar = hareket.BirimFiyat * hareket.Miktar;

                    // doviz eşleştirmesi son

                    //Extra data ekleme
                    try
                    {
                        var extraData = currentRow.ItemArray[12].ToString();
                        hareket.ExtraData = extraData;
                    }
                    catch (Exception ex)
                    {
                    }
                    //Extra data ekleme son


                    list.Add(hareket);
                }
                catch (Exception ex)
                {

                }

            }
            Db.HammaddeHareket.AddRange(list);
            Db.BulkSaveChanges();


        }

        public void FiyatListe(string YanMamul)
        {
            var stokListe = Db.YanSanayiStok
                .Where(x => x.YanMamul.Contains(YanMamul))
                .ToList();
            var kalipListe = Db.Kalip.ToList();
            List<Fiyat> fiyatList = new List<Fiyat>();

            foreach (var item in stokListe)
            {
                Fiyat _fiyat = new Fiyat();
                _fiyat.Aciklama = item.YanMamul;
                _fiyat.KayitTarih = DateTime.Now;
                _fiyat.Tutar = item.BirimFiyat;
                _fiyat.ToplamTutar = item.BirimFiyat;
                _fiyat.Kod = item.Kod;
                _fiyat.DovizId = item.DovizId;
                _fiyat.TableHammaddeBirimId = item.TableHammaddeBirimId;

                if (item.DovizId != (int)EDoviz.TL)
                {
                    if (item.DovizId == (int)EDoviz.USD)
                    {
                        try
                        {
                            var kur = DovizHelper.DovizKuruGetir("USD", _fiyat.KayitTarih);
                            _fiyat.ToplamTutar = kur * _fiyat.Tutar;
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    else if (item.DovizId == (int)EDoviz.EUR)
                    {
                        try
                        {
                            var kur = DovizHelper.DovizKuruGetir("EUR", _fiyat.KayitTarih);
                            _fiyat.ToplamTutar = kur * _fiyat.Tutar;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                fiyatList.Add(_fiyat);

                try
                {
                    var kalip = kalipListe.FirstOrDefault(x => x.ParcaKodu == _fiyat.Kod);
                    if (kalip != null)
                    {
                        kalip.isHazirMalzeme = true;
                    }
                }
                catch { }

            }

            Db.Fiyat.AddRange(fiyatList);
            Db.BulkSaveChanges();
        }

        public void HammaddeCinsiKategori()
        {
            var KategoriListe = Db.Kategori.ToList();
            var HammaddeCinsListe = Db.HammaddeCinsi.ToList();
            var HammaddeHareketListe = Db.HammaddeHareket.Include(x => x.Kategori).Include(x => x.HammaddeCinsi).ToList();

            foreach (var item in HammaddeCinsListe)
            {
                var hareket = HammaddeHareketListe.FirstOrDefault(x => x.HammaddeCinsiId == item.HammaddeCinsiId);
                if (hareket != null)
                {
                    item.KategoriId = hareket.KategoriId;
                }
                else
                {
                    item.KategoriId = KategoriListe.FirstOrDefault(x => x.Adi == "hammadde").KategoriId;
                }
            }
            Db.BulkSaveChanges();
        }

        public void TumUrun(string urunAd, string kisaltma)
        {
            try
            {
                var stok = Db.YanSanayiStok.Where(x => x.UrunAdi == urunAd).ToList();
                var urunCinsId = Db.UrunCinsi.FirstOrDefault(x => x.Kisaltmasi == kisaltma).UrunCinsiId;
                var urunList = Db.Urun.Where(x => x.UrunCinsiId == urunCinsId).ToList();
                var kalipUrunRelations = Db.KalipUrunRelation.Include(x => x.Kalip).ToList();
                var kalipList = Db.Kalip.ToList();
                List<KalipUrunRelation> kalipUrunRelList = new List<KalipUrunRelation>();
                foreach (var s in stok)
                {
                    foreach (var item in urunList)
                    {
                        var rel = kalipUrunRelations.FirstOrDefault(x => x.Kalip.ParcaKodu == s.Kod && x.UrunId == item.UrunId);
                        if (rel == null)
                        {
                            try
                            {
                                KalipUrunRelation relation = new KalipUrunRelation();
                                var kalipId = kalipList.FirstOrDefault(x => x.ParcaKodu == s.Kod).KalipId;
                                var urunId = item.UrunId;

                                relation.KalipId = kalipId;
                                relation.UrunId = urunId;

                                kalipUrunRelList.Add(relation);
                            }
                            catch { }

                        }
                    }
                }

                Db.KalipUrunRelation.AddRange(kalipUrunRelList);
                Db.BulkSaveChanges();
            }
            catch { }
        }

        public void RujSilikonYagiTiner()
        {
            try
            {
                var urunCinsId = Db.UrunCinsi.FirstOrDefault(x => x.Kisaltmasi == "RJ").UrunCinsiId;
                var urunList = Db.Urun.Where(x => x.UrunCinsiId == urunCinsId).ToList();
                List<KalipUrunRelation> kalipUrunRelList = new List<KalipUrunRelation>();

                foreach (var item in urunList)
                {
                    item.isSilikonYagiUsed = true;
                    if (item.UrunNo != "43")
                    {
                        item.isTinerUsed = true;
                    }
                }
                Db.BulkSaveChanges();
            }
            catch { }
        }

        public void UretimSabitlerEkle()
        {
            List<UretimSabitler> list = new List<UretimSabitler>()
            {
                new UretimSabitler()
                {
                    Aciklama = "ENJ. MAKİNA PERSONEL SAAT MALİYETİ",
                    Maliyet="240",
                    Birim = "TL/SAAT",
                    Kod=1
                },
                new UretimSabitler()
                {
                    Aciklama = "SICAK BASKI + TAMPON PERSONEL SAAT MALİYETİ",
                    Maliyet="80",
                    Birim = "TL/SAAT",
                    Kod=2
                },
                new UretimSabitler()
                {
                    Aciklama = "MONTAJ PERSONEL SAAT MALİYETİ",
                    Maliyet="80",
                    Birim = "TL/SAAT",
                    Kod=3
                },
                new UretimSabitler()
                {
                    Aciklama = "SPREY BOYA KAPLAMA PERSONEL SAAT MALİYETİ",
                    Maliyet="80",
                    Birim = "TL/SAAT",
                    Kod=4
                },

            };
            Db.UretimSabitler.AddRange(list);
            Db.BulkSaveChanges();
        }
        public void KoliTurEkle()
        {
            List<KoliTur> list = new List<KoliTur>()
            {
                new KoliTur()
                {
                    Tur = "60*40*40",
                    Kod="1",
                    KatSayi=0.04M
                },
                new KoliTur()
                {
                    Tur = "43*34*30",
                    Kod="2",
                    KatSayi=0.03M
                }

            };
            Db.KoliTur.AddRange(list);
            Db.BulkSaveChanges();
        }
        public void UretimBilgiTemizle()
        {
            var urunler = Db.Urun.ToList();
            var yanSanayiStok = Db.YanSanayiStok.ToList();
            var kaliplar = Db.Kalip.ToList();
            var kalipurunrel = Db.KalipUrunRelation.ToList();
            var kaliphammadde = Db.KalipHammaddeRelation.ToList();
            var urunCins = Db.UrunCinsi.ToList();
            var hammaddehareket = Db.HammaddeHareket.ToList();
            var hammaddeCins = Db.HammaddeCinsi.ToList();
            var hammaddeCinsFire = Db.HammaddeFire.ToList();
            var kategori = Db.Kategori.ToList();
            var fiyatListe = Db.Fiyat.ToList();
            var uretimSabitler = Db.UretimSabitler.ToList();
            var koliTurler = Db.KoliTur.ToList();


            Db.Urun.RemoveRange(urunler);
            Db.Kalip.RemoveRange(kaliplar);
            Db.YanSanayiStok.RemoveRange(yanSanayiStok);
            Db.KalipUrunRelation.RemoveRange(kalipurunrel);
            Db.KalipHammaddeRelation.RemoveRange(kaliphammadde);
            Db.UrunCinsi.RemoveRange(urunCins);
            Db.HammaddeHareket.RemoveRange(hammaddehareket);
            Db.HammaddeCinsi.RemoveRange(hammaddeCins);
            Db.HammaddeFire.RemoveRange(hammaddeCinsFire);
            Db.Kategori.RemoveRange(kategori);
            Db.Fiyat.RemoveRange(fiyatListe);
            Db.UretimSabitler.RemoveRange(uretimSabitler);
            Db.KoliTur.RemoveRange(koliTurler);
            Db.BulkSaveChanges();

            while (Db.HamUrunGrup.ToList().Count() > 0)
            {
                var hamurun = Db.HamUrunGrup.ToList().Take(900);
                Db.HamUrunGrup.RemoveRange(hamurun);
                Db.BulkSaveChanges();
            }


        }

        public void UretimBilgiKaydet()
        {
            HamUrunGroup();
            YanSanayiStok();
            UrunCinsi();
            Urun();
            HammaddeCinsi();
            HammaddeCinsiFire();
            Kalip();
            KalipUrun();
            HammaddeHareket();
            HammaddeCinsiKategori();
            FiyatListe("ayna");
            FiyatListe("POMPA");
            FiyatListe("ponpon");
            FiyatListe("PİM");
            TumUrun("TÜM MASKARA (MS)", "MS");
            TumUrun("TÜM DIPLINER (DL)", "DL");
            TumUrun("TÜM EYELINER (EL)", "EL");
            TumUrun("TÜM LIPGLOSS (LG)", "LG");
            RujSilikonYagiTiner();
            KoliTurEkle();
            UretimSabitlerEkle();
        }

    }
}