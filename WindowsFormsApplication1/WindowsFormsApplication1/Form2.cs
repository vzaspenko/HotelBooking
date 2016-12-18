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
    public partial class Form2 : Form
    {
        private readonly Form1 _parent;
        public ListBox HotlList { get; set; }
        public ListBox Category { get; set; }
        public string  Datein;
        public string  Dateout ;
        public ListBox Freerooms { get; set; }

        public Form2(Form1 form)
        {
            InitializeComponent();
            _parent = form;

            HotlList = new ListBox();
            Category = new ListBox();
            Freerooms = new ListBox();

    }
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            _parent.Activate();
            Hide();
            _parent.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Form3 f = new Form3(Convert.ToString(HotlList.Items[tabControl1.SelectedIndex]), Convert.ToString(Category.Items[tabControl1.SelectedIndex]),Datein,Dateout,_parent);
            this.Close();
            f.ShowDialog();
            
            
        }

     

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if(Convert.ToInt32(Freerooms.Items[tabControl1.SelectedIndex]) == 0 )
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }
    }
}
