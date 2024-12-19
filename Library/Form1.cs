using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Options();
            GetAllBook();
        }

        private void Options()
        {
            if (!Directory.Exists("D:\\BookStore"))
            {
                Directory.CreateDirectory("D:\\BookStore");
            }
        }

        private void GetAllBook()
        {
            dataGridView1.DataSource = DB.GetBooks().Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Picture"].Visible = false;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            DB.Stop=true;
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel =! true;   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book Book = new Book();
            Book.ShowDialog();
            GetAllBook();
        }
    }
}
