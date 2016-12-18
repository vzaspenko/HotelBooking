using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        string HotelName;
        string category;
        string datein;
        string dateout;
        Form1 _parent;
        public Form3(string h, string c, string di, string dout, Form1 p)
        {
            InitializeComponent();
            HotelName = h;
            category = c;
            datein = di;
            dateout = dout;
            _parent = p;
        }

     
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text==null ||textBox1.Text=="")
            {
                MessageBox.Show("Фамилия не введена");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("Телефон не введен");
                textBox2.Focus();
                return;
            }
            if (textBox3.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("Серия и номер паспорта не введены");
                textBox3.Focus();
                return;
            }
            var sql = "select * from pearsons where document ='" + textBox3.Text + "'";
            var conn = datain.Connet();
            var command = datain.Command(sql, conn);
            var reader = command.ExecuteReader();
            if(reader.HasRows==false)
            {
                sql = "insert into pearsons(`name`, `phone`, `document`, `gender`) values"+ 
                    "('"+textBox1.Text+"', '"+textBox2.Text+"', '"+textBox3.Text+"', '"+comboBox1.SelectedItem+"')";
                var conn1 = datain.Connet();
                var command1 = datain.Command(sql, conn1);
                command1.ExecuteNonQuery();
                conn1.Close();
                MessageBox.Show("Новый человек успешно добавлен в базу");
            }

            conn.Close();

            sql = "select idhotel from hotel where name = '" + HotelName + "'";
            conn = datain.Connet();
            command = datain.Command(sql, conn);
            var idhotel = command.ExecuteScalar();

            sql = "select idcategory from category where name  ='"+category+"' ";
            command = datain.Command(sql, conn);
            var idcategory = command.ExecuteScalar();

            sql = "select idrooms from rooms where idhot  =" + Convert.ToInt32(idhotel)
                + " and roomscat = "+ Convert.ToInt32(idcategory);

            command = datain.Command(sql, conn);
            var idrooms = command.ExecuteScalar();

            sql = "select idpearsons from pearsons where document  ='" + textBox3.Text+"'";
            command = datain.Command(sql, conn);
            var idpearson = command.ExecuteScalar();
            int check1, check2;
            if (checkBox1.Checked)
            {
                check1 = 1;
            }
            else check1 = 0;


            if (checkBox2.Checked)
            {
                check2 = 1;
            }
            else check2 = 0;

            sql = "insert into reservation (datein, dateout, idroom, taxitoairport, taxifromairport , pearson) values ('"+
                datein+"','"+dateout+"',"+idrooms+","+check1+","+check2+","+idpearson+")";
            command = datain.Command(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Номер забронирован");
            Hide();
            _parent.ShowDialog();
            
        }
        
    }
}
