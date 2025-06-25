using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Randevular
{
    public class IndexModel : PageModel
    {
        public List<Randevu> RandevuListesi { get; set; } = new List<Randevu>();

        public void OnGet()
        {
            var connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = @"
                        SELECT
                            r.RandevuID,
                            d.Ad + ' ' + d.Soyad AS DoktorAdSoyad,
                            h.Ad + ' ' + h.Soyad AS HastaAdSoyad,
                            r.RandevuTarihi,
                            r.Durum,
                            r.Aciklama,
                            r.OlusturmaTarihi
                        FROM Randevular r
                        JOIN Doktorlar d ON r.DoktorID = d.DoktorID
                        JOIN Hastalar h ON r.HastaID = h.HastaID";

                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var randevu = new Randevu
                            {
                                RandevuID = reader.GetInt32(0),
                                DoktorAdSoyad = reader.GetString(1),
                                HastaAdSoyad = reader.GetString(2),
                                RandevuTarihi = reader.GetDateTime(3),
                                Durum = reader.GetString(4),
                                Aciklama = reader.IsDBNull(5) ? null : reader.GetString(5),
                                OlusturmaTarihi = reader.GetDateTime(6)
                            };

                            RandevuListesi.Add(randevu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }

    public class Randevu
    {
        public int RandevuID { get; set; }
        public int DoktorID { get; set; }       
        public int HastaID { get; set; }      
        public string DoktorAdSoyad { get; set; }
        public string HastaAdSoyad { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public string Durum { get; set; }
        public string Aciklama { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }

}
