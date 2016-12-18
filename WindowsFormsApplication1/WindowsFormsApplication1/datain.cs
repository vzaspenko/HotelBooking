    using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class datain
    {
        public static MySqlConnection Connet()
        {
            var conn = new MySqlConnection("server = localhost; user = root; database = hotel_booking; port = 3306; password = 12345");
            try
            {
                conn.Open();
               // MessageBox.Show("OK");
                return conn;
            }
            catch
            {
                MessageBox.Show("Нет подклчения к БД");
                return null;
            }
            
        }
        public static MySqlCommand Command(string sql,MySqlConnection conn)
        {
            try
            {
                return new MySqlCommand(sql, conn);

            }
            catch
            {
                MessageBox.Show("Ошибка обращения к БД");
                return null;
            }
        }
    }
}
