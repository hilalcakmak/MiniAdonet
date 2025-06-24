using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAdonet.Pages.Doktorlar
{
    public class EditModel : PageModel
    {
        public Doktorlar DoktorBilgi = new Doktorlar();


        public void OnGet()
        {
            string ID = Request.Query["id"];


            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    string sql = "select * from Doktorlar where DoktorID=@DoktorID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@DoktorID", ID);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            DoktorBilgi.DoktorID = reader.GetInt32(0);
                            DoktorBilgi.Ad = reader.GetString(1);
                            DoktorBilgi.Soyad = reader.GetString(2);
                            DoktorBilgi.UzmanlikAlani = reader.GetString(3);
                            DoktorBilgi.Telefon = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            DoktorBilgi.Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            DoktorBilgi.CalismaSaatiBaslangic = reader.GetTimeSpan(6);
                            DoktorBilgi.CalismaSaatiBitis = reader.GetTimeSpan(7);
                            DoktorBilgi.EklenmeTarihi = reader.GetDateTime(8);

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
            DoktorBilgi.DoktorID = int.Parse(Request.Form["DoktorID"]);
            DoktorBilgi.Ad = Request.Form["Ad"].ToString();
            DoktorBilgi.Soyad = Request.Form["Soyad"].ToString();
            DoktorBilgi.UzmanlikAlani = Request.Form["UzmanlikAlani"].ToString();
            DoktorBilgi.Telefon = Request.Form["Telefon"].ToString();
            DoktorBilgi.Email = Request.Form["Email"].ToString();
            DoktorBilgi.CalismaSaatiBaslangic = TimeSpan.Parse(Request.Form["CalismaSaatiBaslangic"]);
            DoktorBilgi.CalismaSaatiBitis = TimeSpan.Parse(Request.Form["CalismaSaatiBitis"]);

            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE Doktorlar SET 
                                    Ad = @Ad, 
                                    Soyad = @Soyad, 
                                    UzmanlikAlani = @UzmanlikAlani, 
                                    Telefon = @Telefon, 
                                    Email = @Email, 
                                    CalismaSaatiBaslangic = @CalismaSaatiBaslangic, 
                                    CalismaSaatiBitis = @CalismaSaatiBitis
                                   WHERE DoktorID = @DoktorID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Ad", DoktorBilgi.Ad);
                        command.Parameters.AddWithValue("@Soyad", DoktorBilgi.Soyad);
                        command.Parameters.AddWithValue("@UzmanlikAlani", DoktorBilgi.UzmanlikAlani);
                        command.Parameters.AddWithValue("@Telefon", DoktorBilgi.Telefon);
                        command.Parameters.AddWithValue("@Email", DoktorBilgi.Email);
                        command.Parameters.AddWithValue("@CalismaSaatiBaslangic", DoktorBilgi.CalismaSaatiBaslangic);
                        command.Parameters.AddWithValue("@CalismaSaatiBitis", DoktorBilgi.CalismaSaatiBitis);
                        command.Parameters.AddWithValue("@DoktorID", DoktorBilgi.DoktorID);
                        command.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {

            }

            Response.Redirect("/Doktorlar/Index");
        }
    }
}

