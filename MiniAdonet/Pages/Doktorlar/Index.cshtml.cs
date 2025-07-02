using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Doktorlar
{
    public class IndexModel : PageModel
    {
        // Listeleme için Doktorlar nesnelerini tutacak liste
        public List<Doktorlar> DoktorListesi { get; set; } = new List<Doktorlar>();

        public void OnGet()
        {
            var connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123.; TrustServerCertificate=True;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = @"
                        SELECT
                            DoktorID,
                            Ad,
                            Soyad,
                            UzmanlikAlani,
                            Telefon,
                            Email,
                            CalismaSaatiBaslangic,
                            CalismaSaatiBitis,
                            EklenmeTarihi
                        FROM Doktorlar";

                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var doktor = new Doktorlar
                            {
                                DoktorID = reader.GetInt32(0),
                                Ad = reader.GetString(1),
                                Soyad = reader.GetString(2),
                                UzmanlikAlani = reader.GetString(3),
                                Telefon = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                                CalismaSaatiBaslangic = reader.GetTimeSpan(6),
                                CalismaSaatiBitis = reader.GetTimeSpan(7),
                                EklenmeTarihi = reader.GetDateTime(8)
                            };

                            DoktorListesi.Add(doktor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi: loglama veya kullanıcıya bildirme ekleyebilirsin
                Console.Error.WriteLine(ex.Message);
            }
        }
    }

    // Tabloya karşılık gelen model sınıfı
    public class Doktorlar
    {
        public int DoktorID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string UzmanlikAlani { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public TimeSpan CalismaSaatiBaslangic { get; set; }
        public TimeSpan CalismaSaatiBitis { get; set; }
        public DateTime EklenmeTarihi { get; set; }
    }
}
