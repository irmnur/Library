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
            string[] files = Directory.GetFiles("C:\\BookStore\\Picture");
            foreach (string file in files)
            {
                File.Delete(file);

            }

            this.Text = "Book Store" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void GetAllBook()
        {
            var booksTable = DB.GetBooks().Tables[0];
            if (booksTable != null)
            {
                dataGridView1.DataSource = booksTable;
                if (dataGridView1.Columns.Contains("ID"))
                {
                    dataGridView1.Columns["ID"].Visible = false;
                }
                if (dataGridView1.Columns.Contains("Picture"))
                {
                    dataGridView1.Columns["Picture"].Visible = false;
                }
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            DB.Stop = true;
            System.Windows.Forms.Application.Exit();
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
            // Geçerli bir satır seçildi mi?
            if (Row >= 0 && Row < dataGridView1.Rows.Count)
            {
                // BookID değerini al
                int BookID = Convert.ToInt32(dataGridView1.Rows[Row].Cells["BookID"].Value);

                // BookID'nin geçerli bir değer olup olmadığını kontrol et
                if (BookID > 0)
                {
                    // Silme işlemi için onay penceresi
                    if (MessageBox.Show("The selected book will be deleted.\n Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        // Veritabanında silme işlemi
                        DB.DeleteBook(BookID);
                        // Kitaplar listesini yeniden yükle
                        GetAllBook();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Book ID.");
                }
            }
            else
            {
                MessageBox.Show("Please select a valid book to delete.");
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
                if (c is System.Windows.Forms.RadioButton && (c as System.Windows.Forms.RadioButton).Checked)
                {
                    rbName = c.Text;
                }
            }
            dataGridView1.DataSource = DB.SearchBook((sender as TextBox).Text, rbName).Tables[0];
        }

        private void button7_Click(object sender, EventArgs e)
        {
            User_Form User_Form = new User_Form();
            User_Form.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateBook book = new UpdateBook();
            book.ShowDialog();
            GetAllBook();
        }
    }
}