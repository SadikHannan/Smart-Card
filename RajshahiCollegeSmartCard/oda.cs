using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace RajshahiCollegeSmartCard
{
    public class oda
    {
        private string _connstring { get; set; }
        public oda()
        {
            _connstring = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
        }

        public void InsertSamrtCardUIS(String uid,String StudentID,String Datetime)
        {
            
            string datetimestring = DateTime.Now.ToString("yyyyMMddHHmmss");
            String sqlquery = "INSERT INTO studentuid (uid,Student_ID,Last_Modified) values ('" + uid + "','" + StudentID + "','" + datetimestring + "')";
            MySqlConnection mysqlCon = new MySqlConnection();
            mysqlCon.ConnectionString = _connstring;
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                mysqlCon.Open();
                cmd.Connection = mysqlCon;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sqlquery;
                cmd.ExecuteNonQuery();
            }catch(MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                mysqlCon.Dispose();
               

            }
        }

    }
}
