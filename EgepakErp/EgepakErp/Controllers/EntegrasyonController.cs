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
            using (var stream = System.IO.File.Open(@"C:\Users\fika yazılım\Downloads\EgepakAktarim\Ham_Urun_Group_Aktarim_02_08_3.xlsx", FileMode.Open, FileAccess.Read))
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
                    UrunGroup.KalipEtiket = currentRow.ItemArray[14]?.ToString();
                    UrunGroup.HammaddeFormul = currentRow.ItemArray[15]?.ToString();
                    list.Add(UrunGroup);
                }
                catch (Exception ex)
                {

                }

            }
            Db.HamUrunGrup.AddRange(list);
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
                    urun.UrunCinsiId = urunCinsleri.FirstOrDefault(i => i.Kisaltmasi == item.UrunCinsi).UrunCinsiId;
                    urun.UrunNo = item.UrunNo;
                    urun.isAktif = true;
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

                var kaliplar = Db.HamUrunGrup
                    .Where(i => i.KalipNo != "00")
                    .Select(x => new
                    {
                        KalipNo = x.KalipNo,
                        KalipOzellik = x.KalipOzellik,
                        ParcaAdi = x.ParcaAdi,
                        Hammadde = x.Hammadde,
                        Agirlik = x.Agirlik,
                        TeminŞekli = x.TeminSekli,
                        KalıpSayisi = x.KalipSayisi,
                        UretimZamani = x.UretimZamani,
                        Aciklama = x.Aciklama,
                        UrunCinsi = x.UrunCinsi,
                        UrunNo = x.UrunNo,
                        KalipEtiket = x.KalipEtiket,
                        HammaddeFormul = x.HammaddeFormul,
                        YollukTip = x.YollukTip,
                        YollukAgirlik = x.YollukAgirlik,
                        KalipKod = x.KalipKodu
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
                        var kalip = kaliplar.FirstOrDefault(i => i.KalipNo == k.KalipNo && i.Adi == k.ParcaAdi);
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

            var dataset = new DataSet();
            using (var stream = System.IO.File.Open(@"C:\Users\fika yazılım\Downloads\EgepakAktarim\Hammadde_Hareket_Aktarim_02_08_2022_1.xlsx", FileMode.Open, FileAccess.Read))
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
                    hareket.Miktar = Convert.ToDecimal(currentRow.ItemArray[8]);
                    hareket.UrunAdi = currentRow.ItemArray[4].ToString();

                    //tedarikci eşleştirmesi
                    string cariUnvan = currentRow.ItemArray[3].ToString().ToLower();
                    try
                    {
                        hareket.TedarikciId = Cariler.Where(x => x.Unvan.ToLower().Contains(cariUnvan)).FirstOrDefault().CariId;
                    }
                    catch (Exception ex)
                    {

                    }
                    //tedarikci eşleştirmesi son

                    //hammadde birimi eşleştirmesi
                    try
                    {
                        int BirimId = Birimler.FirstOrDefault(x => x.Birimi == currentRow.ItemArray[9].ToString()).TableHammaddeBirimId;
                        hareket.TableHammaddeBirimId = BirimId;
                    }
                    catch (Exception ex)
                    {
                        TableHammaddeBirim hBirim = new TableHammaddeBirim();
                        string _birim = currentRow.ItemArray[9].ToString();
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
                        string cins = currentRow.ItemArray[2].ToString();
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



                    hareket.BirimFiyat = Convert.ToDecimal(currentRow.ItemArray[5]);
                    // doviz eşleştirmesi
                    string para = currentRow.ItemArray[6].ToString();
                    if (para != "TL")
                    {
                        if (para == "USD")
                        {
                            hareket.DovizId = 2;
                            try
                            {
                                if (string.IsNullOrEmpty(currentRow.ItemArray[7].ToString()))
                                {
                                    hareket.DolarKuru = DovizHelper.DovizKuruGetir("USD", hareket.HammaddeGirisTarihi);
                                }
                                else
                                {
                                    hareket.DolarKuru = Convert.ToDecimal(currentRow.ItemArray[7]);
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
                                if (currentRow.ItemArray[7].ToString() == "")
                                {
                                    hareket.EuroKuru = DovizHelper.DovizKuruGetir("EUR", hareket.HammaddeGirisTarihi);
                                }
                                else
                                {
                                    hareket.EuroKuru = Convert.ToDecimal(currentRow.ItemArray[7]);
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




                    list.Add(hareket);
                }
                catch (Exception ex)
                {

                }

            }
            Db.HammaddeHareket.AddRange(list);
            Db.BulkSaveChanges();


        }

        public void UretimBilgiTemizle()
        {
            var urunler = Db.Urun.ToList();
            var kaliplar = Db.Kalip.ToList();
            var kalipurunrel = Db.KalipUrunRelation.ToList();
            var kaliphammadde = Db.KalipHammaddeRelation.ToList();
            var urunCins = Db.UrunCinsi.ToList();
            var hammaddehareket = Db.HammaddeHareket.ToList();
            var hammaddeCins = Db.HammaddeCinsi.ToList();
            var hammaddeCinsFire = Db.HammaddeFire.ToList();


            Db.Urun.RemoveRange(urunler);
            Db.Kalip.RemoveRange(kaliplar);
            Db.KalipUrunRelation.RemoveRange(kalipurunrel);
            Db.KalipHammaddeRelation.RemoveRange(kaliphammadde);
            Db.UrunCinsi.RemoveRange(urunCins);
            Db.HammaddeHareket.RemoveRange(hammaddehareket);
            Db.HammaddeCinsi.RemoveRange(hammaddeCins);
            Db.HammaddeFire.RemoveRange(hammaddeCinsFire);
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
            UrunCinsi();
            Urun();
            HammaddeCinsi();
            HammaddeCinsiFire();
            Kalip();
            KalipUrun();
            HammaddeHareket();
        }


    }
}