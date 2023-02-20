using DataShakeApiLocobuzz.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShakeApiLocobuzz
{
    class DataShakeWrapper
    {

        public async Task<LocobuzzResponse> GetAllUrls(IConfiguration config)
        {
            LocobuzzResponse response;
            try
            {
                string AddProfileUrl = config.GetSection("Url").GetValue<string>("GetProfileUrl");
                string spidermanToken = config.GetSection("Token").GetValue<string>("SpidermanToken");
                string connectionString = config.GetSection("ConnectionStrings").GetValue<string>("MainDatabase");
                //Urls urls= new Urls();
                List<wrapperUrl> urls = new List<wrapperUrl>();
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Get_ECommerceSettings_V21", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach(DataRow dr in dt.Rows)
                    {
                        urls.Add(new wrapperUrl()
                        {
                            url = dr[8].ToString(),
                            brandId = dr[6].ToString()
                        });
                    }
                    con.Close();
                }
                Console.WriteLine(urls);
                response = new(true, "", urls);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = new(false, "", ex.Message);
            }
            return response;

        }
    }
}
