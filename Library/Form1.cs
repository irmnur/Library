using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

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
            if (!Directory.Exists("C:\\BookStore"))
            {
                Directory.CreateDirectory("C:\\BookStore");
                Directory.CreateDirectory("C:\\BookStore\\Picture");
            }
            this.Text = "Book Store" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void GetAllBook()
        {
            dataGridView1.DataSource = DB.GetBooks().Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Picture"].Visible = false;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            DB.Stop = true;
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !DB.Stop;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book Book = new Book();
            Book.ShowDialog();
            GetAllBook();
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening1(object sender, CancelEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        int Row = 0;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Row = e.RowIndex;
            DB.BookID = Convert.ToInt32(dataGridView1.Rows[Row].Cells["ID"].Value);
            Book Book = new Book();
            Book.ShowDialog();
            GetAllBook();
        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The selected book will be deleted.\n Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int BookID = Convert.ToInt32(dataGridView1.Rows[Row].Cells["ID"].Value);
                DB.DeleteBook(BookID);
                GetAllBook();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DB.Search = !DB.Search;
            if (DB.Search)
            {
                txtSearch.ResetText();
                txtSearch.Focus();
            }
            gbSearch.Visible = DB.Search;
            
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Row = e.RowIndex;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string rbName = ""; 
            foreach (Control c in gbSearch.Controls)
            {
                if (c is RadioButton && (c as RadioButton).Checked)
                {
                    rbName = c.Text;
                }
            }
            dataGridView1.DataSource = DB.SearchBook((sender as TextBox).Text, rbName).Tables[0];
        }
    }
}