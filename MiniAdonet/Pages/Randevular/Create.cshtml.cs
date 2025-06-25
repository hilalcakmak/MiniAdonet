using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Randevular
{
    public class CreateModel : PageModel
    {
        public Randevu RandevuBilgi { get; set; } = new Randevu();
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccesMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            var doktorIdStr = Request.Form["DoktorAdSoyad"];
            var hastaIdStr = Request.Form["HastaAdSoyad"];
            var tarihStr = Request.Form["RandevuTarihi"];
            var durum = Request.Form["Durum"].ToString();
            var aciklama = Request.Form["Aciklama"].ToString();

            // Zorunlu alan girişleri
            if (!int.TryParse(doktorIdStr, out int doktorID) ||
                !int.TryParse(hastaIdStr, out int hastaID) ||
                !DateTime.TryParse(tarihStr, out DateTime randevuTarihi))
            {
                ErrorMessage = "DoktorID, HastaID ve Randevu Tarihi doğru formatta girilmelidir.";
                return Page();
            }

            // Modeli dolduruyoruz
            RandevuBilgi.DoktorID = doktorID;
            RandevuBilgi.HastaID = hastaID;
            RandevuBilgi.RandevuTarihi = randevuTarihi;
            RandevuBilgi.Durum = string.IsNullOrWhiteSpace(durum) ? "Beklemede" : durum;
            RandevuBilgi.Aciklama = aciklama;

            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = @"INSERT INTO Randevular
                                (DoktorID, HastaID, RandevuTarihi, Durum, Aciklama)
                                VALUES(@DoktorID, @HastaID, @RandevuTarihi, @Durum, @Aciklama)";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@DoktorID", RandevuBilgi.DoktorAdSoyad);
                        command.Parameters.AddWithValue("@HastaID", RandevuBilgi.HastaAdSoyad);
                        command.Parameters.AddWithValue("@RandevuTarihi", RandevuBilgi.RandevuTarihi);
                        command.Parameters.AddWithValue("@Durum", RandevuBilgi.Durum);
                        command.Parameters.AddWithValue("@Aciklama", string.IsNullOrWhiteSpace(RandevuBilgi.Aciklama) ? (object)DBNull.Value : RandevuBilgi.Aciklama);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            SuccesMessage = "Randevu başarıyla oluşturuldu.";
            return RedirectToPage("Index");
        }
    }
}