using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Doktorlar
{
    public class CreateModel : PageModel
    {

        public Doktorlar DoktorBilgi = new Doktorlar();
        public string ErrorMessage = "";
        public string SuccesMessage = "";
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            DoktorBilgi.Ad = Request.Form["Ad"].ToString();
            DoktorBilgi.Soyad = Request.Form["Soyad"].ToString();
            DoktorBilgi.UzmanlikAlani = Request.Form["UzmanlikAlani"].ToString();
            DoktorBilgi.Telefon = Request.Form["Telefon"].ToString();
            DoktorBilgi.Email = Request.Form["Email"].ToString();
            DoktorBilgi.CalismaSaatiBaslangic = TimeSpan.Parse(Request.Form["CalismaSaatiBaslangic"]);
            DoktorBilgi.CalismaSaatiBitis = TimeSpan.Parse(Request.Form["CalismaSaatiBitis"]);

            if (
                string.IsNullOrWhiteSpace(DoktorBilgi.Ad) ||
                string.IsNullOrWhiteSpace(DoktorBilgi.Soyad) ||
                string.IsNullOrWhiteSpace(DoktorBilgi.UzmanlikAlani) ||
                string.IsNullOrWhiteSpace(DoktorBilgi.Telefon) ||
                string.IsNullOrWhiteSpace(DoktorBilgi.Email) ||
                DoktorBilgi.CalismaSaatiBaslangic == default ||
                DoktorBilgi.CalismaSaatiBitis == default
            )
            {
                ErrorMessage = "Tüm Alanlara Veri Girilmelidir!";
                return Page();
            }

            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"insert into Doktorlar
                        (Ad, Soyad, UzmanlikAlani, Telefon, Email, CalismaSaatiBaslangic, CalismaSaatiBitis)
                       values
                        (@Ad, @Soyad, @UzmanlikAlani, @Telefon, @Email, @CalismaSaatiBaslangic, @CalismaSaatiBitis)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Ad", DoktorBilgi.Ad);
                        command.Parameters.AddWithValue("@Soyad", DoktorBilgi.Soyad);
                        command.Parameters.AddWithValue("@UzmanlikAlani", DoktorBilgi.UzmanlikAlani);
                        command.Parameters.AddWithValue("@Telefon", DoktorBilgi.Telefon);
                        command.Parameters.AddWithValue("@Email", DoktorBilgi.Email);
                        command.Parameters.AddWithValue("@CalismaSaatiBaslangic", DoktorBilgi.CalismaSaatiBaslangic);
                        command.Parameters.AddWithValue("@CalismaSaatiBitis", DoktorBilgi.CalismaSaatiBitis);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            DoktorBilgi.Ad = "";
            DoktorBilgi.Soyad = "";
            DoktorBilgi.UzmanlikAlani = "";
            DoktorBilgi.Telefon = "";
            DoktorBilgi.Email = "";
            DoktorBilgi.CalismaSaatiBaslangic = default;
            DoktorBilgi.CalismaSaatiBitis = default;
            SuccesMessage = "Kayıt Başarılı";
            return Redirect("/Doktorlar/Index"); // veya return RedirectToPage("/Doktorlar/Index");


        }
    }
}
