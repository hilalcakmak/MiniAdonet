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
        public Doktorlar doktorbilgi = new Doktorlar();


        public void OnGet()
        {
            string ID = Request.Query["DoktorID"];


            try
            {
                string connectionString = "Server=localhost; Database=SaglikDB; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    string sql = "select * from Doktorlar where ID=@DoktorID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@DoktorID", ID);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            doktorbilgi.ID = "" + reader.GetInt32(0);
                            doktorbilgi.AdSoyad = reader.GetString(1);
                            doktorbilgi.Email = reader.GetString(2);
                            doktorbilgi.Telefon = reader.GetString(3);
                            doktorbilgi.Adres = reader.GetString(4);
                            doktorbilgi.DoktorID = reader.GetInt32(0),
                            doktorbilgi.Ad = reader.GetString(1),
                            doktorbilgi.Soyad = reader.GetString(2),
                            doktorbilgi.UzmanlikAlani = reader.GetString(3),
                                Telefon = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                                CalismaSaatiBaslangic = reader.GetTimeSpan(6),
                                CalismaSaatiBitis = reader.GetTimeSpan(7),
                                EklenmeTarihi = reader.GetDateTime(8)
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
            yoneticibilgi.ID = Request.Form["ID"];
            yoneticibilgi.AdSoyad = Request.Form["AdSoyad"].ToString();
            yoneticibilgi.Email = Request.Form["Email"].ToString();
            yoneticibilgi.Telefon = Request.Form["Telefon"].ToString();
            yoneticibilgi.Adres = Request.Form["Adres"].ToString();

            try
            {
                string connectionString = "Server=localhost; Database=Holding; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "update Yoneticiler set AdSoyad=@AdSoyad, Email=@Email, Telefon=@Telefon, Adres=@Adres where ID=@ID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AdSoyad", yoneticibilgi.AdSoyad);
                        command.Parameters.AddWithValue("@Email", yoneticibilgi.Email);
                        command.Parameters.AddWithValue("@Telefon", yoneticibilgi.Telefon);
                        command.Parameters.AddWithValue("@Adres", yoneticibilgi.Adres);
                        command.Parameters.AddWithValue("@ID", yoneticibilgi.ID);
                        command.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {

            }

            Response.Redirect("/Yoneticiler/Index");
        }
    }
}

