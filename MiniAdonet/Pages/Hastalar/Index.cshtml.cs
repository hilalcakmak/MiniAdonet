using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Hastalar
{
    public class IndexModel : PageModel
    {
        // Listeleme için Hasta nesnelerini tutacak liste
        public List<Hasta> HastaListesi { get; set; } = new List<Hasta>();

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
                            HastaID,
                            Ad,
                            Soyad,
                            DogumTarihi,
                            Cinsiyet,
                            Telefon,
                            Email,
                            Adres,
                            KayitTarihi
                        FROM Hastalar";

                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var hasta = new Hasta
                            {
                                HastaID = reader.GetInt32(0),
                                Ad = reader.GetString(1),
                                Soyad = reader.GetString(2),
                                DogumTarihi = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                Cinsiyet = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Telefon = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Email = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Adres = reader.IsDBNull(7) ? null : reader.GetString(7),
                                KayitTarihi = reader.GetDateTime(8)
                            };

                            HastaListesi.Add(hasta);
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

    // Tabloya karşılık gelen model sınıfı 
    public class Hasta
    {
        public int HastaID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string Cinsiyet { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public DateTime KayitTarihi { get; set; }
    }
}
