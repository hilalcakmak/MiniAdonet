using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Hastalar
{
    public class EditModel : PageModel
    {
        public Hasta HastaBilgi = new Hasta();

        public void OnGet()
        {
            string ID = Request.Query["id"];

            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Hastalar WHERE HastaID=@HastaID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@HastaID", ID);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            HastaBilgi.HastaID = reader.GetInt32(0);
                            HastaBilgi.Ad = reader.GetString(1);
                            HastaBilgi.Soyad = reader.GetString(2);
                            HastaBilgi.DogumTarihi = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);
                            HastaBilgi.Cinsiyet = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            HastaBilgi.Telefon = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            HastaBilgi.Email = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            HastaBilgi.Adres = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            HastaBilgi.KayitTarihi = reader.GetDateTime(8);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void OnPost()
        {
            HastaBilgi.HastaID = int.Parse(Request.Form["HastaID"]);
            HastaBilgi.Ad = Request.Form["Ad"].ToString();
            HastaBilgi.Soyad = Request.Form["Soyad"].ToString();

            var dogumTarihiStr = Request.Form["DogumTarihi"];
            HastaBilgi.DogumTarihi = string.IsNullOrWhiteSpace(dogumTarihiStr) ? (DateTime?)null : DateTime.Parse(dogumTarihiStr);

            HastaBilgi.Cinsiyet = Request.Form["Cinsiyet"].ToString();
            HastaBilgi.Telefon = Request.Form["Telefon"].ToString();
            HastaBilgi.Email = Request.Form["Email"].ToString();
            HastaBilgi.Adres = Request.Form["Adres"].ToString();

            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE Hastalar SET 
                                    Ad = @Ad, 
                                    Soyad = @Soyad, 
                                    DogumTarihi = @DogumTarihi, 
                                    Cinsiyet = @Cinsiyet, 
                                    Telefon = @Telefon, 
                                    Email = @Email, 
                                    Adres = @Adres
                                   WHERE HastaID = @HastaID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Ad", HastaBilgi.Ad);
                        command.Parameters.AddWithValue("@Soyad", HastaBilgi.Soyad);
                        command.Parameters.AddWithValue("@DogumTarihi", (object)HastaBilgi.DogumTarihi ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Cinsiyet", string.IsNullOrWhiteSpace(HastaBilgi.Cinsiyet) ? (object)DBNull.Value : HastaBilgi.Cinsiyet);
                        command.Parameters.AddWithValue("@Telefon", string.IsNullOrWhiteSpace(HastaBilgi.Telefon) ? (object)DBNull.Value : HastaBilgi.Telefon);
                        command.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(HastaBilgi.Email) ? (object)DBNull.Value : HastaBilgi.Email);
                        command.Parameters.AddWithValue("@Adres", string.IsNullOrWhiteSpace(HastaBilgi.Adres) ? (object)DBNull.Value : HastaBilgi.Adres);
                        command.Parameters.AddWithValue("@HastaID", HastaBilgi.HastaID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            Response.Redirect("/Hastalar/Index");
        }
    }

    
}
