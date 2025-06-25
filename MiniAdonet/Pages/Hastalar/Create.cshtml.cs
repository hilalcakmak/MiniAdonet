using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Hastalar
{
    public class CreateModel : PageModel
    {
        public Hasta HastaBilgi = new Hasta();
        public string ErrorMessage = "";
        public string SuccesMessage = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Zorunlu alanları al
            HastaBilgi.Ad = Request.Form["Ad"].ToString();
            HastaBilgi.Soyad = Request.Form["Soyad"].ToString();

            // Opsiyonel alanları al
            var dogumTarihiStr = Request.Form["DogumTarihi"];
            HastaBilgi.DogumTarihi = string.IsNullOrWhiteSpace(dogumTarihiStr) ? (DateTime?)null : DateTime.Parse(dogumTarihiStr);

            HastaBilgi.Cinsiyet = Request.Form["Cinsiyet"].ToString();
            HastaBilgi.Telefon = Request.Form["Telefon"].ToString();
            HastaBilgi.Email = Request.Form["Email"].ToString();
            HastaBilgi.Adres = Request.Form["Adres"].ToString();

            // zorunlu alanlar kontrolü (Ad, Soyad)
            if (string.IsNullOrWhiteSpace(HastaBilgi.Ad) || string.IsNullOrWhiteSpace(HastaBilgi.Soyad))
            {
                ErrorMessage = "Ad ve Soyad alanları zorunludur!";
                return Page();
            }

            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO Hastalar
                        (Ad, Soyad, DogumTarihi, Cinsiyet, Telefon, Email, Adres)
                        VALUES
                        (@Ad, @Soyad, @DogumTarihi, @Cinsiyet, @Telefon, @Email, @Adres)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Ad", HastaBilgi.Ad);
                        command.Parameters.AddWithValue("@Soyad", HastaBilgi.Soyad);
                        command.Parameters.AddWithValue("@DogumTarihi", (object)HastaBilgi.DogumTarihi ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Cinsiyet", string.IsNullOrWhiteSpace(HastaBilgi.Cinsiyet) ? (object)DBNull.Value : HastaBilgi.Cinsiyet);
                        command.Parameters.AddWithValue("@Telefon", string.IsNullOrWhiteSpace(HastaBilgi.Telefon) ? (object)DBNull.Value : HastaBilgi.Telefon);
                        command.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(HastaBilgi.Email) ? (object)DBNull.Value : HastaBilgi.Email);
                        command.Parameters.AddWithValue("@Adres", string.IsNullOrWhiteSpace(HastaBilgi.Adres) ? (object)DBNull.Value : HastaBilgi.Adres);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            // Formu temizle
            HastaBilgi.Ad = "";
            HastaBilgi.Soyad = "";
            HastaBilgi.DogumTarihi = null;
            HastaBilgi.Cinsiyet = "";
            HastaBilgi.Telefon = "";
            HastaBilgi.Email = "";
            HastaBilgi.Adres = "";
            SuccesMessage = "Kayıt Başarılı";
            return Redirect("/Hastalar/Index");
        }
    }

}
