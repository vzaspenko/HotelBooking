using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form2 f2;
        public Form1()
        {

            InitializeComponent();
            AddComponents();
        }

        public void AddComponents()
        {
            var sql = "select distinct city from location order by city";
            var conn = datain.Connet();
            var command = datain.Command(sql, conn);
            var reader = command.ExecuteReader();
            foreach(DbDataRecord record in reader)
            {
                comboBox1.Items.Add(record[0].ToString());
            }
            reader.Close();
            sql = "select name from category order by IdCategory";
            command = datain.Command(sql, conn);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                comboBox2.Items.Add(record[0].ToString());
            }
            reader.Close();
            conn.Close();

        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            /* var sql = "insert into location (country, region, city,streat,building) values('" + textBox1.Text + "','"
                 + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "')";
             var conn = datain.Connet();
             var command = datain.Command(sql, conn);
             command.ExecuteNonQuery();

             conn.Close();*/
            if(Convert.ToInt32(textBox2.Text.Length)>7)
            {
                MessageBox.Show("Диапазон превышает допустимые значения");
                return;
            }
            if (Convert.ToInt32(textBox1.Text.Length) > 7)
            {
                MessageBox.Show("Диапазон превышает допустимые значения");
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Город не выбран");
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Категория номера не выбрана");
                return;
            }
            if (textBox1.Text==null || textBox1.Text=="") 
            {
                MessageBox.Show("Минимальный диапазон це ны не выбран");
                textBox1.Focus();
                return;
              
            }
            if (textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show("Максимальный диапазон цены не выбран");
                textBox2.Focus();
                return;
            }
            if(Convert.ToInt32(textBox2.Text)< Convert.ToInt32(textBox1.Text))
            {
                MessageBox.Show("Неправильно выбран диапазон");
                return;
            }
            if (dateTimePicker1.Value < DateTime.Now)
            {
                MessageBox.Show("Некорректная дата");
                return;
            }
           
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Некорректная дата выезда");
                return;
            }
            f2 = new Form2(this);
            
            var sql = "select l.city, l.streat, l.building,h.name, h.phone, c.name, r.price, r.roomscount from hotel h "+
                     "inner join location l on h.loc = l.idlocation "+
                   "inner join rooms r on r.idhot = h.idhotel "+
                    "inner join category c on c.idcategory = r.roomscat "+
                   " where l.city = '"+comboBox1.SelectedItem.ToString()+"' and c.name = '"+
                   comboBox2.SelectedItem.ToString() + "' and r.price>="+Convert.ToInt32(textBox1.Text)+" and r.price <="+
                   Convert.ToInt32(textBox2.Text);
            var conn = datain.Connet();
            var command = datain.Command(sql, conn);
            var reader = command.ExecuteReader();
            if(reader.HasRows==false)
            {
                MessageBox.Show("По данному запросу отелей не найдено");
                return;
            }
            Hide();
           
            int i = 0;
            
            foreach (DbDataRecord record in reader)
            {

                var sqlRooms = "select count(r.idroom) from reservation r, rooms, hotel h where h.name = '" +
                reader.GetString(3)+"' and idhot = h.idhotel and datein<='" +
                dateTimePicker1.Value.Year+"."+ dateTimePicker1.Value.Month +"."+ dateTimePicker1.Value.Day+
                "' and dateout >= '" + dateTimePicker2.Value.Year + "." + dateTimePicker2.Value.Month +
                "." + dateTimePicker2.Value.Day + 
                "' and idroom = idrooms  ";
                var conn1 = datain.Connet();
                var command1 = datain.Command(sqlRooms, conn1);
                var count = command1.ExecuteScalar();
               

                f2.tabControl1.TabPages.Add(record.GetString(3));
                conn1.Close();
                Label l = new Label();
                Label l1 = new Label();
                
                l.Text = "Город"+"\nУлица"+"\nДом"+"\nНазвание отеля"+"\nТелефон"+"\nКатегория Номера"+"\nЦена за сутки"
                    +"\nдоступных номеров";
                l.Height = 500;
                l.Width = 500;
                l.ForeColor = Color.Orange;
                //l.Font = new Font(FontFamily.GenericSansSerif,8.0f,FontStyle.Bold);

                string city = comboBox1.SelectedItem.ToString();
                string street = record.GetString(1);
                string building = record.GetString(2);
                string nameHotel = record.GetString(3);
                string tel = record.GetString(4);
                string category = record.GetString(5);
                string price = record.GetInt32(6).ToString();
                var freeRooms = record.GetInt32(7);
                freeRooms = freeRooms - Convert.ToInt32(count);

                f2.HotlList.Items.Add(nameHotel);
                f2.Category.Items.Add(category);
                f2.Datein = dateTimePicker1.Value.Year + "." + dateTimePicker1.Value.Month + "." + dateTimePicker1.Value.Day;
                f2.Dateout = dateTimePicker2.Value.Year + "." + dateTimePicker2.Value.Month + "." + dateTimePicker2.Value.Day;
                f2.Freerooms.Items.Add(count);
                l1.Text = city + "\n" + street + "\n" + building + "\n" + nameHotel + "\n" + tel +
                    "\n" + category + "\n" + price+"\n"+freeRooms;
                l1.Location = new Point(500, 0);
                l1.Height = 500;
                f2.tabControl1.TabPages[i].Controls.Add(l);
                f2.tabControl1.TabPages[i].Controls.Add(l1);
                if(freeRooms == 0 && i==0)
                {
                    f2.button2.Enabled = false;
                }
                i++;
            }

            //f2.tabControl1.TabPages.Add(f2.tabControl1.TabPages[0]);

            conn.Close();
            //Close();
            f2.ShowDialog();

           

        }

      
    }
}
